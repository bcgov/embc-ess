import { Component, OnInit } from '@angular/core';
import { RegistrationService } from 'src/app/core/services/registration.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { NotificationQueueService } from 'src/app/core/services/notification-queue.service';
import {
  Registration, Supplier, IncidentalsReferral, FoodReferral,
  ClothingReferral, LodgingReferral, TransportationReferral
} from 'src/app/core/models';

interface ReferralFormControl<T = any> {
  value: T;
  valid: boolean;
}

@Component({
  selector: 'app-referral-maker',
  templateUrl: './referral-maker.component.html',
  styleUrls: ['./referral-maker.component.scss']
})
export class ReferralMakerComponent implements OnInit {

  editMode = true; // when you first land on this page
  submitting = false;
  registration: Registration = null;
  path: string = null; // for relative routing
  regId: string = null;
  purchaser: string = null;
  evacuees: Array<any> = [];

  foodReferrals: Array<ReferralFormControl<FoodReferral>> = [];
  lodgingReferrals: Array<ReferralFormControl<LodgingReferral>> = [];
  clothingReferrals: Array<ReferralFormControl<ClothingReferral>> = [];
  transportationReferrals: Array<ReferralFormControl<TransportationReferral>> = [];
  incidentalsReferrals: Array<ReferralFormControl<IncidentalsReferral>> = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private registrationService: RegistrationService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private notifications: NotificationQueueService,
  ) { }

  ngOnInit() {
    // get path for routing
    this.authService.path.subscribe(p => this.path = p);

    // get URL params
    this.regId = this.route.snapshot.paramMap.get('id');
    this.purchaser = this.route.snapshot.paramMap.get('purchaser');

    if (!this.regId || !this.purchaser) {
      // error - return to list
      console.log('ERROR - reg id or purchaser was not provided');
      this.router.navigate([`/${this.path}/registrations`]);
    }

    // get registration data
    this.registrationService.getRegistrationSummaryById(this.regId)
      .subscribe(r => {
        if (!r.id || !r.essFileNumber) {
          console.log('ERROR - invalid registration object = ', r);
          this.cancel();
        } else {
          this.registration = r;

          // populate evacuees
          const hoh = this.registration.headOfHousehold;
          const family = hoh && hoh.familyMembers ? hoh.familyMembers : [];
          if (hoh) {
            this.evacuees = [hoh, ...family].map(x => ({ evacuee: x, selected: false }));
          }
        }
      });
  }

  back() {
    // show the editing parts of the form
    this.editMode = true;
    window.scrollTo(0, 0); // scroll to top
  }

  cancel() {
    // clear the loaded record if available
    this.uniqueKeyService.clearKey();
    // navigate back to evacuee list
    this.router.navigate([`/${this.path}/registrations`]);
  }

  createReferral() {

    // TODO: Validate here BEFORE going to review portion of this page....

    this.editMode = false;
    window.scrollTo(0, 0); // scroll to top
  }

  finalize() {
    this.submitting = true;

    // TODO: save stuff, etc
    this.notifications.addNotification('Referrals finalized successfully');

    this.submitting = false;

    // redirect to summary page
    // save the key for lookup
    this.uniqueKeyService.setKey(this.registration.id);
    this.router.navigate([`/${this.path}/registration/summary`]);
  }

  incidentalsReferralChange() {
    if (this.incidentalsReferrals.length > 0) {
      this.clearIncidentalsReferrals();
    } else {
      this.addIncidentalsReferral();
    }
  }

  foodReferralChange() {
    if (this.foodReferrals.length > 0) {
      this.clearFoodReferrals();
    } else {
      this.addFoodReferral();
    }
  }

  addIncidentalsReferral() {
    const referral: IncidentalsReferral = {
      id: null, // is populated by BE after save
      active: true,
      type: 'INCIDENTALS',
      purchaser: this.purchaser,
      dates: {
        from: new Date(2019, 3, 15, 17, 30, 0), // FOR TESTING ONLY
        days: 2, // FOR TESTING ONLY
      },
      evacuees: this.evacuees,
      approvedItems: null,
      totalAmount: null,
      supplier: this.newSupplier,
      comments: 'some comments here',
      confirmChecked: false
    };
    this.incidentalsReferrals.push({ value: referral, valid: false });
  }

  addFoodReferral() {
    const referral: FoodReferral = {
      id: null, // is populated by BE after save
      active: true,
      type: 'FOOD',
      subType: null,
      purchaser: this.purchaser,
      dates: {
        from: new Date(2019, 0, 1),
      },
      evacuees: this.evacuees,
      numBreakfasts: 0,
      numLunches: 0,
      numDinners: 0,
      numDaysMeals: 0,
      totalAmount: 0,
      supplier: this.newSupplier,
      comments: 'some comments here',
      confirmChecked: false
    };
    this.foodReferrals.push({ value: referral, valid: false });
  }

  private get newSupplier(): Supplier {
    return {
      id: null, // for future use
      active: true,
      name: 'Supplier 1', // TODO: for testing only
      address: '1050 Main Street', // TODO: for testing only
      postalCode: 'V8R 1R4', // TODO: for testing only
      city: 'Victoria', // TODO: for testing only
      province: 'BC',
      telephone: '250-123-4567', // TODO: for testing only
      fax: '250-345-7789', // TODO: for testing only
    };
  }

  clearIncidentalsReferrals(): void {
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Incidentals referrals?')) {
      while (this.incidentalsReferrals.length > 0) { this.incidentalsReferrals.pop(); }
    }
  }

  clearFoodReferrals(): void {
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Food referrals?')) {
      while (this.foodReferrals.length > 0) { this.foodReferrals.pop(); }
    }
  }

  // --------------------HELPERS-----------------------------------------
  remove(arr: [], i: number) {
    if (arr) { arr.splice(i, 1); }
  }

  updateValidity(entry: ReferralFormControl, valid: boolean) {
    if (entry) { entry.valid = valid; }
  }

  // Checks that all referrals are valid within their respective categories (food, lodging, etc)
  get formIsValid(): boolean {
    const status = {
      food: true,  // TODO: Include the other arrays here...
      lodging: true,
      clothing: true,
      transportation: true,
      incidentals: this.incidentalsReferrals.every(e => e.valid)
    };
    return Object.keys(status).every(key => !!status[key]);
  }
}
