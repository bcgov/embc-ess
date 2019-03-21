import { Registration } from './registration.model';

export interface MetaRegistration {
    data: Registration[];
    metadata: {
        page: number;
        pageSize: number;
        totalCount: number;
        TotalPages: number;
        links: number;

    };
}