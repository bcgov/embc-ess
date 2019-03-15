import { Community, Country } from './';

interface BaseAddress {
  id: string | null;
  addressSubtype: string; // one of ['BCAD', 'OTAD'] for BC vs non-BC addresses
  addressLine1: string;
  addressLine2?: string;
  addressLine3?: string;
  postalCode: string;
}

export interface BcAddress extends BaseAddress {
  addressSubtype: 'BCAD';
  // related entities
  community: Community;
}

export interface OtherAddress extends BaseAddress {
  addressSubtype: 'OTAD';
  city: string;
  province: string;
  country: Country;
}

export type Address = BcAddress & OtherAddress;

export function isBcAddress(address: Address): boolean {
  return address && address.addressSubtype === 'BCAD';
}

export function isOtherAddress(address: Address): boolean {
  return address && address.addressSubtype === 'OTAD';
}
