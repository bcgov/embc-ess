import { Evacuee, Supplier } from './';
import { ReferralDate } from './referral-date';
import { ListResult } from './list-result';

export type ReferralType = ('FOOD' | 'INCIDENTALS' | 'CLOTHING' | 'LODGING' | 'TRANSPORTATION');
export type FoodSubType = ('RESTAURANT' | 'GROCERIES');
export type LodgingSubType = ('HOTEL' | 'BILLETING' | 'GROUP');
export type TransportationSubType = ('TAXI' | 'OTHER');

export interface ReferralBase {
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
}

export interface FoodReferral extends ReferralBase {
  subType?: FoodSubType;
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

export interface LodgingReferral extends ReferralBase {
  subType?: LodgingSubType;
  numNights?: number;
  numRooms?: number;
}

// tslint:disable-next-line: no-empty-interface
export interface TransportationReferral extends ReferralBase {
  subType?: TransportationSubType;
  fromAddress?: string;
  toAddress?: string;
  otherTransportModeDetails?: string;
}

// used to POST (create new) referrals
export interface ReferralPost {
  confirmChecked: boolean;
  referrals: Array<Partial<ReferralPostItem>>;
}

export interface ReferralPostItem {
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
  otherTransportModeDetails?: string;
}

// response from POST API call
export interface ReferralSuccess {
  registrationId: string;
  referrals: Array<{ referralId: string }>;
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
