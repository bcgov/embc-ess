import { Address } from './address.model';

export interface Profile {
  phoneNumber?: string;
  phoneNumberAlt?: string;
  email?: string;
  primaryResidence: Address;
  mailingAddress: Address | null;
}
