import { Evacuee, Supplier } from './';
import { ReferralDate } from './referral-date';

type ReferralType = ('FOOD' | 'INCIDENTALS' | 'CLOTHING' | 'ACCOMMODATION' | 'TRANSPORTATION');

export interface Referral {
  id: string;
  active: boolean;
  type: ReferralType;
  subType?: string;
  purchaser: string;
  dates: ReferralDate;
  evacuees: Array<{
    evacuee: Evacuee,
    selected: boolean
  }>;
  totalAmount: number; // NB: set to 0 if not used
  supplier: Supplier;
  comments: string;
  confirmChecked: boolean;
}

export interface FoodReferral extends Referral {
  subType?: ('RESTAURANT' | 'GROCERIES');
  numBreakfasts?: number;
  numLunches?: number;
  numDinners?: number;
  numDaysMeals?: number;
}

export interface IncidentalsReferral extends Referral {
  approvedItems: string;
}

// tslint:disable-next-line: no-empty-interface
export interface ClothingReferral extends Referral {
  extremeWinterConditions: boolean;
}

// tslint:disable-next-line: no-empty-interface
export interface AccommodationReferral extends Referral {
  subType?: ('HOTEL' | 'BILLETING' | 'GROUP');
  numNights: number;
  numRooms?: number;
}

// tslint:disable-next-line: no-empty-interface
export interface TransportationReferral extends Referral {
  subType?: ('TAXI' | 'OTHER');
  fromAddress?: string;
  toAddress?: string;
  modeTransport?: string;
}

// --------------------HELPERS-----------------------------------------
export function isFoodReferral(referral: Referral): boolean {
  return referral && referral.type === 'FOOD';
}

export function isIncidentalsReferral(referral: Referral): boolean {
  return referral && referral.type === 'INCIDENTALS';
}

export function isClothingReferral(referral: Referral): boolean {
  return referral && referral.type === 'CLOTHING';
}

export function isAccommodationReferral(referral: Referral): boolean {
  return referral && referral.type === 'ACCOMMODATION';
}

export function isTransportationReferral(referral: Referral): boolean {
  return referral && referral.type === 'TRANSPORTATION';
}
