'use client';

import { useState, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import AuthModal from './AuthModal';
import SpaceBackground from './SpaceBackground';
import { api } from '@/services/api';
import { TodoTask } from '@/models/Task';

// Радиус нашей орбиты в пикселях
const RADIUS = 280;

interface Node {
  id: string;
  name: string;
  colorHex: string;
  todoTask: TodoTask[] | null;
}

// Углы для отрисовки нод на орбите (так как на бэкенде углов обычно нет, задаем их по индексам)
const DEFAULT_ANGLES =[0, 45, 90, 135, 180, 225, 270, 315]; 

export default function OrbitTracker() {
  // Динамические данные из Web API
  const [nodes, setNodes] = useState<Node[]>([]);
  const [tasks, setTasks] = useState<Record<string, TodoTask[]>>({});
  const [isLoading, setIsLoading] = useState<boolean>(true);

  // Авторизованный пользователь
  const [user, setUser] = useState<string | null>(() => {
    if (typeof window !== 'undefined') {
      return localStorage.getItem('user_session');
    }
    return null;
  });

  // Основные состояния интерфейса
  const [activeNode, setActiveNode] = useState<string | null>(null);
  const [editingTaskId, setEditingTaskId] = useState<string | null>(null);
  const [editingText, setEditingText] = useState('');
  const [newTaskText, setNewTaskText] = useState('');

  useEffect(() => {
    if (!user) return;

    async function fetchInitialData() {
      try {
        setIsLoading(true);
        const fetchedNodes = await api.getNodes(); // Скачиваем дерево данных
        setNodes(fetchedNodes);
        
        // Создаем временный объект для распределения задач по nodeId
        const initialTasksRecord: Record<string, TodoTask[]> = {};
        
        fetchedNodes.forEach(node => {
          initialTasksRecord[node.id] = node.todoTask || []; //T
        });
        
        setTasks(initialTasksRecord);

        if (fetchedNodes.length > 0) {
          setActiveNode(fetchedNodes[0].id);
        }
      } catch (err) {
        console.error('Ошибка при получении данных космоса:', err);
      } finally {
        setIsLoading(false);
      }
    }

    fetchInitialData();
  }, [user]);



  const toggleTask = async (nodeId: string, task: TodoTask) => {
    try {
      const updatedTask = await api.updateTask({
        id: task.id,
        title: task.title,
        isCompleted: !task.isCompleted, 
        dueDate: task.dueDate
      });

      setTasks(prev => ({
        ...prev,
        [nodeId]: prev[nodeId].map(t => t.id === task.id ? updatedTask : t)
      }));
    } catch (err) {
      console.error(err);
      alert('Не удалось обновить статус задачи');
    }
  };


  const startEditing = (task: TodoTask) => {
    setEditingTaskId(task.id);
    setEditingText(task.title);
  };

  const saveEditing = async (nodeId: string, task: TodoTask) => {
    if (!editingText.trim() || editingText === task.title) {
      setEditingTaskId(null);
      return;
    }

    try {
      const updatedTask = await api.updateTask({
        id: task.id,
        title: editingText.trim(),
        isCompleted: task.isCompleted,
        dueDate: task.dueDate
      });

      setTasks(prev => ({
        ...prev,
        [nodeId]: prev[nodeId].map(t => t.id === task.id ? updatedTask : t)
      }));
      setEditingTaskId(null);
    } catch (err) {
      console.error(err);
      alert('Не удалось переименовать задачу');
    }
  };

const addTask = async (nodeId: string) => {
  if (!newTaskText.trim()) return;

  const dateInput = document.getElementById('task-due-date') as HTMLInputElement | null;
  const chosenDate = dateInput && dateInput.value ? new Date(dateInput.value).toISOString() : null;

  try {
    const newTask = await api.createTask({
      title: newTaskText.trim(),
      dueDate: chosenDate, 
      idNode: nodeId
    });

    setTasks(prev => ({
      ...prev,
      [nodeId]: [...(prev[nodeId] || []), newTask]
    }));
    
    setNewTaskText('');
    if (dateInput) dateInput.value = '';
  } catch (err) {
    console.error(err);
    alert('Не удалось создать задачу');
  }
};


  const deleteTask = async (nodeId: string, taskId: string) => {
    try {
      await api.deleteTask(taskId);
      setTasks(prev => ({
        ...prev,
        [nodeId]: prev[nodeId].filter(t => t.id !== taskId)
      }));
    } catch (err) {
      console.error(err);
      alert('Не удалось удалить задачу');
    }
  };

  const allTasks = Object.values(tasks).flat();
  const totalGlobalTasks = allTasks.length;
  const completedGlobalTasks = allTasks.filter(t => t.isCompleted).length;
  const globalPercentage = totalGlobalTasks > 0 ? Math.round((completedGlobalTasks / totalGlobalTasks) * 100) : 0;

  const activeNodeInfo = nodes.find(n => n.id === activeNode);
  const activeNodeIndex = nodes.findIndex(n => n.id === activeNode);
  const activeAngle = activeNodeIndex !== -1 ? DEFAULT_ANGLES[activeNodeIndex % DEFAULT_ANGLES.length] : 0;

  const currentCategoryTasks = activeNode ? tasks[activeNode] || [] : [];
    if (isLoading) {
    return (
      <div className="w-screen h-screen bg-[#030313] flex items-center justify-center font-mono text-blue-400 text-xs tracking-widest animate-pulse">
        LOADING ORBITAL STORAGE...
      </div>
    );
  }

  return (
    <div className="relative w-full h-screen bg-[#030313] overflow-hidden flex items-center justify-center font-mono select-none">
      <SpaceBackground />
      {!user && <AuthModal onAuthSuccess={(username) => setUser(username)} />}
      <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_center,rgba(16,16,48,0.4),transparent_70%)]" />

      {user && (
        <motion.div 
          initial={{ opacity: 0, y: -20 }} 
          animate={{ opacity: 1, y: 0 }} 
          className="absolute top-5 right-5 z-30 flex items-center space-x-4 bg-[#06061e]/80 border border-blue-500/30 backdrop-blur-md rounded-xl px-4 py-2.5 text-xs shadow-[0_0_20px_rgba(59,130,246,0.1)]"
        >
          <div className="relative flex h-2 w-2">
            <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-emerald-400 opacity-75"></span>
            <span className="relative inline-flex rounded-full h-2 w-2 bg-emerald-500"></span>
          </div>
          <div className="flex flex-col">
            <span className="text-[10px] text-slate-500 uppercase tracking-widest">Operator Connected</span>
            <span className="text-white font-bold tracking-wide">ID: <span className="text-blue-400">{user}</span></span>
          </div>
          <div className="h-6 w-[1px] bg-slate-800 mx-1" />
          <motion.button 
            whileHover={{ scale: 1.05, color: '#f43f5e' }} 
            whileTap={{ scale: 0.95 }} 
            onClick={() => {
              localStorage.removeItem('user_session');
              localStorage.removeItem('auth_token'); 
              setUser(null);
              setNodes([]);
              setTasks({});
              setActiveNode(null);
            }} 
            className="text-rose-500/80 transition-colors uppercase font-bold tracking-wider text-[10px] bg-rose-950/20 hover:bg-rose-950/40 px-2 py-1 rounded border border-rose-900/30"
          >
            Disconnect
          </motion.button>
        </motion.div>
        
      )}
{/* ЛЕВАЯ ПАНЕЛЬ С ЗАДАЧАМИ */}
<div className="absolute top-10 left-10 z-30 w-80">
  <AnimatePresence mode="wait">
    {activeNode && activeNodeInfo && (
      <motion.div 
        key={activeNode} 
        initial={{ opacity: 0, x: -30 }} 
        animate={{ opacity: 1, x: 0 }} 
        exit={{ opacity: 0, x: -30 }} 
        className="p-5 rounded-xl border bg-[#06061e]/95 backdrop-blur-md shadow-2xl" 
        style={{ borderColor: `${activeNodeInfo.colorHex}30` }}
      >
        {/* Шапка панели */}
        <div className="flex justify-between items-center mb-4 pb-2 border-b border-slate-800">
          <h3 className="text-sm font-bold tracking-widest uppercase" style={{ color: activeNodeInfo.colorHex }}>
            {activeNodeInfo.name}
          </h3>
          <span className="text-xs text-slate-400">
            {currentCategoryTasks.filter(t => t.isCompleted).length}/{currentCategoryTasks.length}
          </span>
        </div>

        {/* Поле добавления новой задачи и выбор Даты */}
        <div className="flex space-x-2 mb-4">
          <input 
            type="text" 
            placeholder="+ Add subtask..." 
            value={newTaskText} 
            onChange={(e) => setNewTaskText(e.target.value)} 
            onKeyDown={(e) => e.key === 'Enter' && addTask(activeNode)} 
            className="flex-1 bg-[#030313] border border-slate-800 rounded px-2 py-1 text-xs text-white focus:outline-none focus:border-slate-600" 
          />
          <input 
            type="date"
            id="task-due-date"
            className="bg-[#030313] border border-slate-800 rounded px-1 py-1 text-[10px] text-slate-400 focus:outline-none focus:border-slate-600 cursor-pointer"
          />
        </div>

        {/* Список задач */}
        <div className="space-y-3 max-h-60 overflow-y-auto pr-1">
          {currentCategoryTasks.length === 0 ? (
            <p className="text-xs text-slate-500 italic">No tasks</p>
          ) : (
            currentCategoryTasks.map((task) => {
              let dueText = '';
              if (task.dueDate) {
                const targetDate = new Date(task.dueDate);
                const targetToday = new Date();
                targetToday.setHours(0, 0, 0, 0);
                const targetDue = new Date(targetDate.getFullYear(), targetDate.getMonth(), targetDate.getDate());
                const diffTime = targetDue.getTime() - targetToday.getTime();
                const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
                
                if (diffDays < 0) dueText = 'Overdue';
                else if (diffDays === 0) dueText = 'Today';
                else if (diffDays === 1) dueText = 'Tomorrow';
                else dueText = `${diffDays} days left`;
              }

              return (
                <div key={task.id} className="flex items-center justify-between text-xs group py-0.5">
                  
                  <div className="flex items-center space-x-3 flex-1 min-w-0">
                    <button 
                      onClick={() => toggleTask(activeNode, task)} 
                      className="w-4 h-4 rounded flex items-center justify-center border transition-colors shrink-0" 
                      style={{ borderColor: task.isCompleted ? activeNodeInfo.colorHex : '#475569' }}
                    >
                      {task.isCompleted && (
                        <svg className="w-2.5 h-2.5" viewBox="0 0 24 24" fill="none" stroke={activeNodeInfo.colorHex} strokeWidth="4">
                          <polyline points="20 6 9 17 4 12" />
                        </svg>
                      )}
                    </button>

                    {editingTaskId === task.id ? (
                      <input 
                        type="text" 
                        value={editingText} 
                        onChange={(e) => setEditingText(e.target.value)} 
                        onBlur={() => saveEditing(activeNode, task)} 
                        onKeyDown={(e) => e.key === 'Enter' && saveEditing(activeNode, task)} 
                        autoFocus 
                        className="bg-[#030313] border border-blue-500 rounded px-1 py-0.5 text-white w-full focus:outline-none" 
                      />
                    ) : (
                      <div className="flex flex-col min-w-0 flex-1">
                        <span 
                          onDoubleClick={() => startEditing(task)} 
                          className={`cursor-pointer transition-all truncate ${task.isCompleted ? 'line-through text-slate-500' : 'text-slate-300'}`}
                        >
                          {task.title}
                        </span>
                        {dueText && !task.isCompleted && (
                          <span className={`text-[9px] font-bold mt-0.5 ${dueText === 'Overdue' ? 'text-rose-500 animate-pulse' : 'text-slate-500'}`}>
                            ⏳ {dueText}
                          </span>
                        )}
                      </div>
                    )}
                  </div>

                  {editingTaskId !== task.id && (
                    <div className="flex items-center space-x-2 opacity-0 group-hover:opacity-100 transition-opacity ml-2 shrink-0">
                      <button onClick={() => startEditing(task)} className="text-slate-500 hover:text-white" title="Редактировать">
                        ✏️
                      </button>
                      <button onClick={() => deleteTask(activeNode, task.id)} className="text-slate-500 hover:text-rose-400" title="Удалить">
                        🗑️
                      </button>
                    </div>
                  )}

                </div>
              );
            })
          )}
        </div>
      </motion.div>
    )}
  </AnimatePresence>
</div>
      <div className="relative w-[700px] h-[700px] flex items-center justify-center">
        <svg className="absolute inset-0 w-full h-full pointer-events-none opacity-20">
          <circle cx="350" cy="350" r={RADIUS} fill="none" stroke="#475569" strokeWidth="1" strokeDasharray="4 4" />
          <circle cx="350" cy="350" r={RADIUS - 80} fill="none" stroke="#475569" strokeWidth="0.5" />
          {activeNodeInfo && (() => {
            const rad = (activeAngle * Math.PI) / 180;
            return <line x1="350" y1="350" x2={350 + RADIUS * Math.cos(rad)} y2={350 + RADIUS * Math.sin(rad)} stroke={activeNodeInfo.colorHex} strokeWidth="1.5" opacity="0.6" />;
          })()}
        </svg>

        <div className="relative z-10 w-48 h-48 rounded-full border border-blue-500/30 bg-[#06061e]/90 backdrop-blur-md flex flex-col items-center justify-center text-center shadow-[0_0_50px_rgba(59,130,246,0.2)]">
          <span className="text-[10px] text-slate-500 tracking-widest uppercase">Completed</span>
          <span className="text-5xl font-bold text-white my-1 tracking-tighter">{completedGlobalTasks}</span>
          <span className="text-[10px] text-slate-400">of {totalGlobalTasks} tasks</span>
          <span className="text-xs text-blue-400 font-semibold bg-blue-950/50 px-2 py-0.5 rounded-full border border-blue-800/30 mt-2">{globalPercentage}%</span>
        </div>

{nodes.map((node, index) => {
  const angle = DEFAULT_ANGLES[index % DEFAULT_ANGLES.length];
  const rad = (angle * Math.PI) / 180;
  
  const x = 350 + RADIUS * Math.cos(rad);
  const y = 350 + RADIUS * Math.sin(rad);
  
  const isActive = activeNode === node.id;
  const categoryTasks = tasks[node.id] || [];

  return (
    <button 
      key={node.id} 
      onClick={() => setActiveNode(node.id)} 
      style={{ 
        left: `${x}px`, 
        top: `${y}px`, 
        transform: 'translate(-50%, -50%)', 
        borderColor: node.colorHex, 
        boxShadow: isActive ? `0 0 25px ${node.colorHex}50` : 'none' 
      }} 
      className="absolute w-16 h-16 rounded-full border bg-[#06061e] flex flex-col items-center justify-center transition-all duration-300 group hover:scale-110 z-20"
    >
      <span className="text-[10px] font-bold text-slate-300 group-hover:text-white transition-colors truncate max-w-[55px]">
        {node.name}
      </span>
      
      <span className="text-[9px] text-slate-500 mt-0.5">
        {categoryTasks.filter(t => t.isCompleted).length}/{categoryTasks.length}
      </span>
      
      {isActive && (
        <span 
          style={{ backgroundColor: node.colorHex }} 
          className="absolute inset-0 rounded-full animate-ping opacity-25 -z-10" 
        />
      )}
    </button>

        );
      })}
          </div> 
  </div> 
 );
}
