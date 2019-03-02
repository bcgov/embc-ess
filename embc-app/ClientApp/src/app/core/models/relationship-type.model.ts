export interface RelationshipType {
  code: string | null;  // primary key
  description: string;
  active?: boolean;
}
