import { Registration, Volunteer, Referral } from 'src/app/core/models';

/**
 * The search results to be displayed on an "<evacuee-search-results>" component.
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
 * The search results to be displayed on an "<referral-search-results>" component.
 */
export interface ReferralSearchResults {
  registrationId: string;
  results: Referral[];
}

/**
 * The query parameters required for Pagination, Searching and Sorting.
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

// Evacuee advanced search fields
export interface EvacueeSearchQueryParameters extends SearchQueryParameters {
  last_name?: string;
  first_name?: string;
  dob?: string;
  task_no?: string;
  ess_file_no?: string;
  evacuated_from?: string;
  evacuated_to?: string;
  registration_completed?: boolean;
  referrals_provided?: boolean;
  self_reg_date_from?: string;
  self_reg_date_to?: string;
  finalization_date_to?: string;
  finalization_date_from?: string;
}
