import { Community } from './';

interface BaseAddress {
  addressSubtypeCode: string; // one of ['BCAD', 'OTAD'] for BC vs non-BC addresses
  addressLine1: string;
  addressLine2?: string;
  addressLine3?: string;
  postalCodeOrZip: string;
  country: string;
  communityOrCity: string;
  provinceOrState: string;
}

export interface BcAddress extends BaseAddress {
  addressSubtypeCode: 'BCAD';
  // related entities
  community: Community;
}

export interface OtherAddress extends BaseAddress {
  addressSubtypeCode: 'OTAD';
}

export type Address = BcAddress | OtherAddress;
