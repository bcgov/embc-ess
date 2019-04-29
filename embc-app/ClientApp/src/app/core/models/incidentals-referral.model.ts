export interface IncidentalsReferral {
  id: string | null;
  active?: boolean;
  validFrom: Date;
  validTo: Date;
  // supplier: Supplier;
  evacuees: Array<string>;
  approvedItems: string;
  maxTotal: number;
  comments: string;
}
