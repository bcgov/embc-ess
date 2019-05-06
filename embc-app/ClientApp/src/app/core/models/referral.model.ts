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
export interface ClothingReferral extends Referral { }

// tslint:disable-next-line: no-empty-interface
export interface AccommodationReferral extends Referral { }

// tslint:disable-next-line: no-empty-interface
export interface TransportationReferral extends Referral { }
