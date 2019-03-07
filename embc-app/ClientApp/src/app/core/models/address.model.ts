export interface Address {
  addressLine1: string;
  addressLine2?: string;
  addressLine3?: string;
  communityOrCity: string;
  provinceOrState: string;
  postalCodeOrZip: string;
  country: string;
}
