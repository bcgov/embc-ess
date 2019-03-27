import { Volunteer } from './volunteer.model';

export interface MetaVolunteers {
    data: Volunteer[];
    metadata: {
        page: number;
        pageSize: number;
        totalCount: number;
        totalPages: number;
        links: object[];
    };
}
