//для текстирования без подключения к бэкэнду
export interface OrbitNode {
  id: string;
  label: string;
  angle: number;
  color: string;
}

export interface Task {
  id: string;
  text: string;
  completed: boolean;
}
export const INITIAL_MOCK_TASKS: Record<string, Task[]> = {
  home: [
    { id: 'h1', text: 'Clean apartment', completed: true },
    { id: 'h2', text: 'Fix shelf', completed: false },
    { id: 'h3', text: 'Organize closet', completed: true },
  ],
  work: [
    { id: 'w1', text: 'Review pull request', completed: true },
    { id: 'w2', text: 'Team sync meeting', completed: false },
  ],
};