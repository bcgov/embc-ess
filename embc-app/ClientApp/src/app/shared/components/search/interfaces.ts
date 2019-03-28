import { Registration, Volunteer } from 'src/app/core/models';

/**
 * The search results to be displayed on an "<evacuee-search-results>" component
 */
export interface EvacueeSearchResults {
  query: string;
  results: Registration[];
}
export interface VolunteerSearchResults {
  query: string;
  results: Volunteer[];
}

/**
 * Represents the query parameters required for Pagination, Searching and Sorting
 */
export interface SearchQueryParameters {
  // pagination related
  limit?: number;
  offset?: number;
  // search related
  q?: string;
  // sort related - e.g. "name" for ASC, "-name" for DESC
  sort?: string;
}
