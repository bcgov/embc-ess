import { IncidentTask } from './incident-task.model';

export interface MetaIncidentTask {
    data: IncidentTask[];
    metadata: {
        page: number;
        pageSize: number;
        totalCount: number;
        totalPages: number;
        links: object[];
    };
}