import { Node } from '@/models/Node'; 
import { TodoTask, TodoTaskCreate, TodoTaskUpdate } from '@/models/Task';
import { LoginCommand, RegistrationCommand } from '@/models/Auth';

if (typeof window === 'undefined') {
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
}

const API_URL = 'https://localhost:7050/api'; 

// Получение токена 
const getAuthHeader = () => {
  if (typeof window === 'undefined') return {};
  const token = localStorage.getItem('auth_token');
  return token ? { 'Authorization': `Bearer ${token}` } : {};
};

export const api = {
  // NODES (Общие для всех)
  async getNodes(): Promise<Node[]> {
    const res = await fetch(`${API_URL}/Nodes`, 
      { headers: getAuthHeader() as HeadersInit });
    if (!res.ok) throw new Error('Ошибка загрузки категорий');
    return res.json();
  },

  async createTask(task: TodoTaskCreate): Promise<TodoTask> {
    const command = {
      title: task.title,
      dueDate: task.dueDate,
      nodeId: task.idNode 
    };

    const res = await fetch(`${API_URL}/Tasks`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', ...getAuthHeader() } as HeadersInit,
      body: JSON.stringify(command), 
    });

    if (!res.ok) throw new Error('Ошибка создания задачи');
    

    const taskId = await res.json();
    return {
      id: taskId,
      title: task.title,
      isCompleted: false,
      createdAt: new Date().toISOString(),
      dueDate: task.dueDate
    };
  },

  async updateTask(task: TodoTaskUpdate): Promise<TodoTask> {
    const command = {
      Id: task.id,
      Title: task.title,
      IsCompleted: task.isCompleted, 
      DueDate: task.dueDate
    };

    const res = await fetch(`${API_URL}/Tasks`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json', ...getAuthHeader() } as HeadersInit,
      body: JSON.stringify(command),
    });

    if (!res.ok) throw new Error('Ошибка обновления задачи');
    
    const data = await res.json();
    
    return {
      id: data.id || data.Id,
      title: data.title || data.Title,
      isCompleted: data.isCompleted !== undefined ? data.isCompleted : data.IsCompleted,
      createdAt: data.createdAt || data.CreatedAt || new Date().toISOString(),
      dueDate: data.dueDate || data.DueDate
    };
  },

  async deleteTask(id: string): Promise<void> {
    const res = await fetch(`${API_URL}/Tasks/${id}`, {
      method: 'DELETE',
      headers: getAuthHeader() as HeadersInit,
    });
    if (!res.ok) throw new Error('Ошибка удаления задачи');
  },

    async register(command: RegistrationCommand): Promise<{ token: string; nickName: string }> {
    const res = await fetch(`${API_URL}/Auth/register`, { 
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(command),
    });
    if (!res.ok) {
      const errorData = await res.json().catch(() => ({}));
      throw new Error(errorData.message || 'Ошибка регистрации');
    }
    return res.json();
  },

  async login(command: LoginCommand): Promise<{ token: string; nickName: string }> {
    const res = await fetch(`${API_URL}/Auth/login`, { 
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(command),
    });
    if (!res.ok) {
      const errorData = await res.json().catch(() => ({}));
      throw new Error(errorData.message || 'Неверный логин или пароль');
    }
    return res.json();
  }
};
