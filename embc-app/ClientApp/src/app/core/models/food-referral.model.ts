import { Evacuee, Supplier } from './';

type FoodType = ( 'RESTAURANT' | 'GROCERIES' );

export interface FoodReferral {
  id: string;
  active?: boolean;
  purchaser: string;
  validFrom: Date;
  validTo: Date;
  evacuees: Array<{
    evacuee: Evacuee,
    selected: boolean
  }>;
  foodType: FoodType;
  numBreakfasts?: number;
  numLunches?: number;
  numDinners?: number;
  numDaysMeals?: number;
  totalAmt: number;
  supplier: Supplier;
  comments: string;
}
