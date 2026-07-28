import {TodoTask} from '@/models/Task'

export interface Node {
    id: string;
    name: string;
    colorHex: string;
    todoTask: TodoTask[] | null;
}