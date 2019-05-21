import { Evacuee, Supplier } from './';
import { ReferralDate } from './referral-date';
import { ListResult } from './list-result';

export type ReferralType = ('FOOD' | 'INCIDENTALS' | 'CLOTHING' | 'LODGING' | 'TRANSPORTATION');

export interface ReferralBase {
  id: string;
  essNumber: string;
  referralId: string;
  active: boolean;
  type: ReferralType;
  subType?: string;
  purchaser: string;
  dates: ReferralDate;
  evacuees: Array<Evacuee>;
  totalAmount: number; // NB: set to 0 if not used
  supplier: Supplier;
  comments: string;
  confirmChecked: boolean;
}

export interface FoodReferral extends ReferralBase {
  subType?: ('RESTAURANT' | 'GROCERIES');
  numBreakfasts?: number;
  numLunches?: number;
  numDinners?: number;
  numDaysMeals?: number;
}

export interface IncidentalsReferral extends ReferralBase {
  approvedItems: string;
}

// tslint:disable-next-line: no-empty-interface
export interface ClothingReferral extends ReferralBase {
  extremeWinterConditions: boolean;
}

// tslint:disable-next-line: no-empty-interface
export interface LodgingReferral extends ReferralBase {
  subType?: ('HOTEL' | 'BILLETING' | 'GROUP');
  numNights: number;
  numRooms?: number;
}

// tslint:disable-next-line: no-empty-interface
export interface TransportationReferral extends ReferralBase {
  subType?: ('TAXI' | 'OTHER');
  fromAddress?: string;
  toAddress?: string;
  modeTransport?: string;
}

export interface ReferralPost {
  confirmChecked: boolean;
  referrals: ReferralPostItem[];
}
export interface ReferralPostItem {
  id: string;
  essNumber: string;
  referralId: string;
  active: boolean;
  type: ReferralType;
  subType?: string;
  purchaser: string;
  validDates: ReferralDate;
  evacuees: Array<Evacuee>;
  totalAmount: number; // NB: set to 0 if not used
  supplier: Supplier;
  comments: string;
  confirmChecked: boolean;

  numBreakfasts?: number;
  numLunches?: number;
  numDinners?: number;
  numDaysMeals?: number;
  numNights?: number;
  numRooms?: number;
  approvedItems?: string;
  extremeWinterConditions?: boolean;
  fromAddress?: string;
  toAddress?: string;
  modeTransport?: string;
}
export interface ReferralSuccess {
  registrationId: string;
  referrals: [
    { referralId: string }
  ];
}


export type Referral = FoodReferral | IncidentalsReferral | ClothingReferral | LodgingReferral | TransportationReferral;
export interface RawReferralCollection {
  registrationId: string;
  referrals: ListResult<Referral>;
}
// --------------------HELPERS-----------------------------------------

// TODO: the BE should only provide type of type ReferralType
// but for now it returns static data with type='Food'

export function isFoodReferral(referral: Referral): boolean {
  return referral && referral.type.toUpperCase() === 'FOOD';
}

export function isIncidentalsReferral(referral: Referral): boolean {
  return referral && referral.type.toUpperCase() === 'INCIDENTALS';
}

export function isClothingReferral(referral: Referral): boolean {
  return referral && referral.type.toUpperCase() === 'CLOTHING';
}

export function isLodgingReferral(referral: Referral): boolean {
  return referral && referral.type.toUpperCase() === 'LODGING';
}

export function isTransportationReferral(referral: Referral): boolean {
  return referral && referral.type.toUpperCase() === 'TRANSPORTATION';
}

