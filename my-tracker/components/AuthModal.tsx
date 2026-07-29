'use client';

import React, { useState } from 'react';
import { api } from '@/services/api';

interface AuthModalProps {
  onAuthSuccess: (username: string) => void;
}

export default function AuthModal({ onAuthSuccess }: AuthModalProps) {
  const [isRegister, setIsRegister] = useState(false);
  const [email, setEmail] = useState('');
  const [username, setUsername] = useState(''); 
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setIsSubmitting(true);

    try {
      if (isRegister) {
        const data = await api.register({
          nickName: username,
          email: email,
          password: password,
        });

        // Сохраняем токен и сессию
        localStorage.setItem('auth_token', data.token);
        localStorage.setItem('user_session', data.nickName);
        onAuthSuccess(data.nickName);
      } else {
        const data = await api.login({
          email: email,
          password: password,
        });

        localStorage.setItem('auth_token', data.token);
        localStorage.setItem('user_session', data.nickName);
        onAuthSuccess(data.nickName);
      }
    } catch (err) {
        console.error(err);
        if (err instanceof Error) {
        setError(err.message);
        } 
        else {
        setError('Произошла ошибка при аутентификации');
    } 
  }
    finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/80 backdrop-blur-md font-mono select-none">
      <form 
        onSubmit={handleSubmit} 
        className="w-full max-w-sm p-6 rounded-xl border border-blue-500/30 bg-[#06061e] shadow-[0_0_50px_rgba(59,130,246,0.2)] text-slate-300"
      >
        <h2 className="text-xl font-bold tracking-widest text-white text-center uppercase mb-6">
          {isRegister ? '🌌 System Registration' : '🚀 System Login'}
        </h2>
        {error && (
          <div className="mb-4 p-2 text-xs text-rose-400 bg-rose-950/30 border border-rose-900/50 rounded text-center">
            {error}
          </div>
        )}

        <div className="space-y-4">
          <div>
            <label className="text-[10px] text-slate-500 uppercase tracking-wider block mb-1">Gmail</label>
            <input 
              type="email" 
              required 
              disabled={isSubmitting}
              value={email} 
              onChange={(e) => setEmail(e.target.value)} 
              className="w-full bg-[#030313] border border-slate-800 rounded px-3 py-2 text-sm text-white focus:outline-none focus:border-blue-500 transition-colors disabled:opacity-50" 
            />
          </div>

          {isRegister && (
            <div>
              <label className="text-[10px] text-slate-500 uppercase tracking-wider block mb-1">Nickname</label>
              <input 
                type="text" 
                required 
                disabled={isSubmitting}
                value={username} 
                onChange={(e) => setUsername(e.target.value)} 
                className="w-full bg-[#030313] border border-slate-800 rounded px-3 py-2 text-sm text-white focus:outline-none focus:border-blue-500 transition-colors disabled:opacity-50" 
              />
            </div>
          )}

          <div>
            <label className="text-[10px] text-slate-500 uppercase tracking-wider block mb-1">Password</label>
            <input 
              type="password" 
              required 
              disabled={isSubmitting}
              value={password} 
              onChange={(e) => setPassword(e.target.value)} 
              className="w-full bg-[#030313] border border-slate-800 rounded px-3 py-2 text-sm text-white focus:outline-none focus:border-blue-500 transition-colors disabled:opacity-50" 
            />
          </div>
        </div>

        <button 
          type="submit" 
          disabled={isSubmitting}
          className="w-full mt-6 bg-blue-950/50 hover:bg-blue-900/50 border border-blue-800 text-blue-400 font-bold py-2 rounded text-sm tracking-widest transition-all uppercase disabled:opacity-50"
        >
          {isSubmitting ? 'Connecting...' : isRegister ? 'Create Account' : 'Initialize'}
        </button>

        <p className="text-center text-xs text-slate-500 mt-4">
          {isRegister ? 'Already space-born?' : 'New explorer?'} {' '}
          <button 
            type="button" 
            disabled={isSubmitting}
            onClick={() => {
              setIsRegister(!isRegister);
              setError(null);
            }} 
            className="text-blue-400 hover:underline bg-transparent border-none p-0 cursor-pointer disabled:opacity-50"
          >
            {isRegister ? 'Sign In' : 'Sign Up'}
          </button>
        </p>
      </form>
    </div>
  );
}
