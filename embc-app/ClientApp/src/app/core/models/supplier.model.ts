import { SupplierAddress } from './';

export interface Supplier {
  id: string | null;
  active?: boolean;
  name: string | null;
  address: SupplierAddress;
  phoneNumber: string | null;
  faxNumber: string | null;
}
