/***
 * The API response interface for all list results.
 * This is useful if you have a lot of APIs with the same json contract.
 */
export interface ListResult<T> {
  data: T[];
  metadata: {
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    links: object[];
  };
}
