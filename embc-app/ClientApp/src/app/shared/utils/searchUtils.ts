import { Registration } from 'src/app/core/models';

export interface EvacueeSearchResults {
  query: string;
  results: Registration[];
}
