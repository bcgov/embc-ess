import { Evacuee, Supplier } from './';

type ReferralType = ('FOOD' | 'INCIDENTALS' | 'CLOTHING' | 'ACCOMMODATION' | 'TRANSPORTATION');

export interface Referral {
  id: string;
  active: boolean;
  type: ReferralType;
  subType?: string;
  purchaser: string;
  validFrom: Date;
  validTo: Date;
  evacuees: Array<{
    evacuee: Evacuee,
    selected: boolean
  }>;
  totalAmt?: number;
  supplier: Supplier;
  comments: string;
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
