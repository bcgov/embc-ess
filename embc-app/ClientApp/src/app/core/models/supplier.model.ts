export interface Supplier {
  id: string | null;
  active: boolean;
  name?: string;
  address?: string;
  city?: string;
  province: 'BC';
  postalCode?: string;
  telephone?: string;
  fax?: string;
}
