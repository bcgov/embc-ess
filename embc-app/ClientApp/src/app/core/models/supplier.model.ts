import { BcAddress } from './';

export interface Supplier {
  id: string | null;
  active?: boolean;
  name: string | null;
  address: BcAddress; // must be in BC
  phoneNumber: string | null;
  faxNumber: string | null;
}
