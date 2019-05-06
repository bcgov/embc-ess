export interface Supplier {
  id: string | null;
  active?: boolean;
  name: string | null;
  address: string;
  city: string;
  province: 'BC';
  postalCode: string;
  telephone: string | null;
  fax: string | null;
}
