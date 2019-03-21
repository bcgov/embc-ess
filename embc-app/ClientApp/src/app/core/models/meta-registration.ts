import { Registration } from './registration.model';

export interface MetaRegistration {
    data: Registration[];
    metadata: {
        page: number;
        pageSize: number;
        totalCountTotalPages: number;
        links: number;

    };
}
// {
//     "data": [],
//     "metadata": {
//       "page": 1,
//       "pageSize": 50,
//       "totalCount": 0,
//       "totalPages": 0,
//       "links": null
//     }
//   }