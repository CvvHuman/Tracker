export interface TodoTask {
    id: string;
    title: string;
    isCompleted: boolean;
    createdAt: string;
    dueDate: string | null;
}

export interface TodoTaskCreate{
    title: string;
    dueDate: string | null;
    idNode: string;
}

export interface TodoTaskUpdate{
    id: string;
    title: string;
    isCompleted: boolean;
    dueDate: string | null;
}

export interface TodoTaskDelete{
    id: string;
}