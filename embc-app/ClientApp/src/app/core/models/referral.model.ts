import { Evacuee, Supplier } from './';

export interface Referral {
  id: string;
  active?: boolean;
  purchaser: string;
  validFrom: Date;
  validTo: Date;
  evacuees: Array<{
    evacuee: Evacuee,
    selected: boolean
  }>;
  supplier: Supplier;
  comments: string;
}

export interface FoodReferral extends Referral {
  foodType: ('RESTAURANT' | 'GROCERIES');
  numBreakfasts?: number;
  numLunches?: number;
  numDinners?: number;
  numDaysMeals?: number;
  totalAmt: number;
}

export interface IncidentalsReferral extends Referral {
  approvedItems: string;
  totalAmt: number;
}

// tslint:disable-next-line: no-empty-interface
export interface ClothingReferral extends Referral { }

// tslint:disable-next-line: no-empty-interface
export interface AccommodationReferral extends Referral { }

// tslint:disable-next-line: no-empty-interface
export interface TransportationReferral extends Referral { }
