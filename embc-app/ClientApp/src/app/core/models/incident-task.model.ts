import { Region, Community } from '.';

export interface IncidentTask {
  id: string | null;
  taskNumber: string;
  details: string;
  active?: boolean;
  totalAssociatedEvacuees?: number;
  region: Region | null;
  community: Community | null;
  startDate: string | null; // datetime
}
