import { Component, OnInit } from '@angular/core';
import { RegistrationService } from 'src/app/core/services/registration.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { NotificationQueueService } from 'src/app/core/services/notification-queue.service';
import {
  Registration, IncidentalsReferral, FoodReferral,
  ClothingReferral, LodgingReferral, TransportationReferral, Evacuee
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
  evacuees: Array<Evacuee> = [];

  // avoid showing validation errors until the NEXT button is clicked
  userClickedNext = false;

  // Is this maker form valid?
  valid = false;

  foodReferrals: Array<ReferralFormControl<Partial<FoodReferral>>> = [];
  lodgingReferrals: Array<ReferralFormControl<LodgingReferral>> = [];
  clothingReferrals: Array<ReferralFormControl<ClothingReferral>> = [];
  transportationReferrals: Array<ReferralFormControl<TransportationReferral>> = [];
  incidentalsReferrals: Array<ReferralFormControl<Partial<IncidentalsReferral>>> = [];

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
          this.evacuees = this.createEvacueeList(r);
        }
      });
  }

  updateReferral(referralForm: ReferralFormControl, value: any): void {
    if (referralForm) {
      referralForm.value = value;
    }
  }

  updateValidationStatus(referralForm: ReferralFormControl, valid: boolean): void {
    if (referralForm) {
      referralForm.valid = valid;
    }
    this.valid = this.calculateStatus();
  }

  // Recalculates the validation status of this maker form
  private calculateStatus(): boolean {
    const allValid = (arr: Array<ReferralFormControl<any>>) => arr.every(e => e.valid);

    // TODO: check the other arrays here...
    const food = true;
    const lodging = true;
    const clothing = true;
    const transportation = true;
    const incidentals = allValid(this.incidentalsReferrals);

    return (food && lodging && clothing && transportation && incidentals);
  }

  // user clicked Back button
  back() {
    // show the editing parts of the form
    this.editMode = true;
    this.userClickedNext = false;
    window.scrollTo(0, 0); // scroll to top
  }

  // user clicked Cancel & Close button
  cancel() {
    // clear the loaded record if available
    this.uniqueKeyService.clearKey();
    // navigate back to evacuee list
    this.router.navigate([`/${this.path}/registrations`]);
  }

  // user clicked Create Referral button
  createReferral() {
    this.userClickedNext = true;

    // Validate here BEFORE going to review portion of this page....
    if (this.valid || true) {
      this.editMode = false;
      window.scrollTo(0, 0); // scroll to top
    }
  }

  // user clicked Finalize button
  finalize() {
    this.submitting = true;

    // TODO: save stuff, etc
    this.notifications.addNotification('Referrals finalized successfully', 'success');

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

  lodgingReferralChange() {
    if (this.lodgingReferrals.length > 0) {
      this.clearLodgingReferrals();
    } else {
      this.addLodgingReferral();
    }
  }

  clothingReferralChange() {
    if (this.clothingReferrals.length > 0) {
      this.clearClothingReferrals();
    } else {
      this.addClothingReferral();
    }
  }

  transportationReferralChange() {
    if (this.transportationReferrals.length > 0) {
      this.clearTransportationReferrals();
    } else {
      this.addTransportationReferral();
    }
  }

  addIncidentalsReferral() {
    const referral: Partial<IncidentalsReferral> = {
      id: null, // is populated by BE after save
      active: true,
      type: 'INCIDENTALS',
      purchaser: this.purchaser,
      dates: { from: null, to: null, days: null },
      evacuees: [],
      supplier: { id: null, active: true, province: 'BC' },
      confirmChecked: false
    };
    this.incidentalsReferrals.push({ value: referral, valid: false });
  }

  addFoodReferral() {
    const referral: Partial<FoodReferral> = {
      id: null, // is populated by BE after save
      active: true,
      type: 'FOOD',
      purchaser: this.purchaser,
      dates: { from: null, to: null, days: null },
      evacuees: [],
      supplier: { id: null, active: true, province: 'BC' },
      confirmChecked: false
    };
    this.foodReferrals.push({ value: referral, valid: false });
  }

  addLodgingReferral() {
    // TODO
  }

  addClothingReferral() {
    // TODO
  }

  addTransportationReferral() {
    // TODO
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

  clearLodgingReferrals(): void {
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Lodging referrals?')) {
      while (this.lodgingReferrals.length > 0) { this.lodgingReferrals.pop(); }
    }
  }

  clearClothingReferrals(): void {
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Clothing referrals?')) {
      while (this.clothingReferrals.length > 0) { this.clothingReferrals.pop(); }
    }
  }

  clearTransportationReferrals(): void {
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Transportation referrals?')) {
      while (this.transportationReferrals.length > 0) { this.transportationReferrals.pop(); }
    }
  }

  // populate evacuees
  private createEvacueeList(reg: Registration): Evacuee[] {
    if (!reg || !reg.headOfHousehold) { return []; }
    const hoh = reg.headOfHousehold;
    const family = hoh.familyMembers || [];
    return [hoh, ...family];
  }

  // --------------------HELPERS-----------------------------------------
  remove(arr: [], i: number) {
    if (arr) { arr.splice(i, 1); }
  }
}
