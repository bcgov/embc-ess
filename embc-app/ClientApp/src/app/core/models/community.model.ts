import { RegionalDistrict } from './';

export interface Community {
  id: string | null;
  name: string;
  active?: boolean;
  regionalDistrict: RegionalDistrict;
}
