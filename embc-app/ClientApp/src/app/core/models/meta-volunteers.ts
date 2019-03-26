import { Volunteer } from './volunteer.model';

export interface MetaVounteers {
    data: Volunteer[];
    metadata: {
        page: number;
        pageSize: number;
        totalCount: number;
        totalPages: number;
        links: object[];
    };
}
