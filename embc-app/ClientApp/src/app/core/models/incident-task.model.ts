import { Region, RegionalDistrict, Community } from '.';

export interface IncidentTask {
  id: string | null;
  taskNumber: string;
  details: string;
  region: Region | null;
  regionalDistrict: RegionalDistrict | null;
  community: Community | null;
}
