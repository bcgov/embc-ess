import { Region } from './';

export interface RegionalDistrict {
  id: string | null;
  name: string;
  active?: boolean;
  region: Region;
}
