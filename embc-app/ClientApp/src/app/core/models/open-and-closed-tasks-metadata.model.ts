import { PaginationSummary } from './list-result';

export interface OpenAndClosedTasksMetadata {
    openTasks: PaginationSummary;
    closedTasks: PaginationSummary;
}