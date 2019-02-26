import { Address } from './address.model';

export interface Profile {
  id?: string;
  phoneNumber?: string;
  phoneNumberAlt?: string;
  email?: string;
  primaryResidence: Address;
  mailingAddress?: Address;
}
