import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';

import { RegistrationService } from 'src/app/core/services/registration.service';
import { ReferralService } from 'src/app/core/services/referral.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { NotificationQueueService } from 'src/app/core/services/notification-queue.service';
import {
  Registration, Evacuee, ReferralPostItem,
  FoodReferral, LodgingReferral, ClothingReferral,
  TransportationReferral, IncidentalsReferral
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
  stepCounter = 1; // counts which step on the page the user is on

  editMode = true; // when you first land on this page
  submitting = false;
  registration: Registration = null;
  path: string = null; // for relative routing
  regId: string = null;
  purchaser: string = null;
  evacuees: Array<Evacuee> = [];
  workingDefaultDate: Date;
  defaultDate: Date;
  showDefaultDatePicker = false;
  confirmChecked = false;

  private triggerSubject = new Subject<void>();
  submitTrigger = this.triggerSubject.asObservable();

  // avoid showing validation errors until the NEXT button is clicked
  userClickedNext = false;

  // Is this maker form valid?
  valid = false;

  foodReferrals: Array<ReferralFormControl<Partial<FoodReferral>>> = [];
  lodgingReferrals: Array<ReferralFormControl<Partial<LodgingReferral>>> = [];
  clothingReferrals: Array<ReferralFormControl<Partial<ClothingReferral>>> = [];
  transportationReferrals: Array<ReferralFormControl<Partial<TransportationReferral>>> = [];
  incidentalsReferrals: Array<ReferralFormControl<Partial<IncidentalsReferral>>> = [];

  get haveReferrals(): boolean {
    return this.foodReferrals.length > 0
      || this.lodgingReferrals.length > 0
      || this.clothingReferrals.length > 0
      || this.transportationReferrals.length > 0
      || this.incidentalsReferrals.length > 0;
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private registrationService: RegistrationService,
    private referralService: ReferralService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private notifications: NotificationQueueService,
  ) { }

  ngOnInit() {
    // get path for routing
    this.authService.path.subscribe((path: string) => this.path = path);

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
      .subscribe((registration: Registration) => {
        if (!registration.id || !registration.essFileNumber) {
          console.log('ERROR - invalid registration object = ', registration);
          this.cancel();
        } else {
          this.registration = registration;
          this.evacuees = this.createEvacueeList(registration);
          this.defaultDate = new Date(registration.incidentTask.startDate);
        }
      }, err => {
        this.notifications.addNotification('Failed to load evacuee summary', 'danger');
        console.log('error getting registration summary =', err);
        // go back to the main dashboard
        this.router.navigate([`/${this.path}/`]);
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
    this.updateFormValidity();
  }

  private updateFormValidity() {
    this.valid = this.calculateStatus();
  }

  // Recalculates the validation status of this maker form
  private calculateStatus(): boolean {
    const allValid = (arr: Array<ReferralFormControl<any>>) => arr.every(e => e.valid);

    const food = allValid(this.foodReferrals);
    const lodging = allValid(this.lodgingReferrals);
    const clothing = allValid(this.clothingReferrals);
    const transportation = allValid(this.transportationReferrals);
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

  // user clicked Create Referrals button
  createReferrals() {
    this.userClickedNext = true;

    // trigger form submission in all sub-components; i.e. the food referrals, the incidentals referrals, etc
    this.triggerSubject.next();

    // Validate here BEFORE going to review portion of this page....
    if (this.valid) {
      this.editMode = false;
      window.scrollTo(0, 0); // scroll to top
    }
  }

  // user clicked Finalize button
  finalize() {
    this.submitting = true;
    if (this.confirmChecked === true) {
      // assemble referrals
      const referrals: Array<Partial<ReferralPostItem>> = [];
      this.foodReferrals.forEach(r => referrals.push({ ...r.value }));
      this.lodgingReferrals.forEach(r => referrals.push({ ...r.value }));
      this.clothingReferrals.forEach(r => referrals.push({ ...r.value }));
      this.transportationReferrals.forEach(r => referrals.push({ ...r.value }));
      this.incidentalsReferrals.forEach(r => referrals.push({ ...r.value }));

      // create referrals
      this.referralService.createReferrals(this.regId, { confirmChecked: true, referrals })
        .subscribe(() => {
          this.submitting = false;
          this.notifications.addNotification('Referrals finalized successfully', 'success');

          // redirect to summary page
          // save the key for lookup
          this.uniqueKeyService.setKey(this.registration.id);
          this.router.navigate([`/${this.path}/registration/summary`]);
        }, err => {
          this.submitting = false;
          this.notifications.addNotification('Failed to finalize referrals', 'danger');
          console.log('error creating referral =', err);
        });
    }
  }

  isReferralValid(r: ReferralFormControl): boolean {
    return this.userClickedNext && r.valid;
  }

  isReferralInvalid(r: ReferralFormControl): boolean {
    return this.userClickedNext && !r.valid;
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
      essNumber: this.regId,
      referralId: null, // is populated by BE after save
      active: true,
      type: 'INCIDENTALS',
      purchaser: this.purchaser,
      validDates: { from: this.defaultDate, to: null, days: null },
      evacuees: [],
      supplier: { id: null, active: true, province: 'BC' }
    };
    this.incidentalsReferrals.push({ value: referral, valid: false });
    this.updateFormValidity();
  }

  addFoodReferral() {
    const referral: Partial<FoodReferral> = {
      essNumber: this.regId,
      referralId: null, // is populated by BE after save
      active: true,
      type: 'FOOD',
      purchaser: this.purchaser,
      validDates: { from: this.defaultDate, to: null, days: null },
      evacuees: [],
      supplier: { id: null, active: true, province: 'BC' }
    };
    this.foodReferrals.push({ value: referral, valid: false });
    this.updateFormValidity();
  }

  addLodgingReferral() {
    const referral: Partial<LodgingReferral> = {
      essNumber: this.regId,
      referralId: null, // is populated by BE after save
      active: true,
      type: 'LODGING',
      purchaser: this.purchaser,
      validDates: { from: this.defaultDate, to: null, days: null },
      evacuees: [],
      supplier: { id: null, active: true, province: 'BC' }
    };
    this.lodgingReferrals.push({ value: referral, valid: false });
    this.updateFormValidity();
  }

  addClothingReferral() {
    const referral: Partial<ClothingReferral> = {
      essNumber: this.regId,
      referralId: null, // is populated by BE after save
      active: true,
      type: 'CLOTHING',
      purchaser: this.purchaser,
      validDates: { from: this.defaultDate, to: null, days: null },
      evacuees: [],
      supplier: { id: null, active: true, province: 'BC' }
    };
    this.clothingReferrals.push({ value: referral, valid: false });
    this.updateFormValidity();
  }

  addTransportationReferral() {
    const referral: Partial<TransportationReferral> = {
      essNumber: this.regId,
      referralId: null, // is populated by BE after save
      active: true,
      type: 'TRANSPORTATION',
      purchaser: this.purchaser,
      validDates: { from: this.defaultDate, to: null, days: null },
      evacuees: [],
      supplier: { id: null, active: true, province: 'BC' }
    };
    this.transportationReferrals.push({ value: referral, valid: false });
    this.updateFormValidity();
  }

  clearIncidentalsReferrals(): void {
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Incidentals referrals?')) {
      while (this.incidentalsReferrals.length > 0) { this.incidentalsReferrals.pop(); }
      this.updateFormValidity();
    }
  }

  clearFoodReferrals(): void {
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Food referrals?')) {
      while (this.foodReferrals.length > 0) { this.foodReferrals.pop(); }
      this.updateFormValidity();
    }
  }

  clearLodgingReferrals(): void {
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Lodging referrals?')) {
      while (this.lodgingReferrals.length > 0) { this.lodgingReferrals.pop(); }
      this.updateFormValidity();
    }
  }

  clearClothingReferrals(): void {
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Clothing referrals?')) {
      while (this.clothingReferrals.length > 0) { this.clothingReferrals.pop(); }
      this.updateFormValidity();
    }
  }

  clearTransportationReferrals(): void {
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Transportation referrals?')) {
      while (this.transportationReferrals.length > 0) { this.transportationReferrals.pop(); }
      this.updateFormValidity();
    }
  }

  // populate evacuees
  private createEvacueeList(reg: Registration): Evacuee[] {
    if (!reg || !reg.headOfHousehold) { return []; }
    const hoh = reg.headOfHousehold;
    const family = hoh.familyMembers || [];
    return [hoh, ...family];
  }

  toggleDefaultDatePicker() {
    if (this.showDefaultDatePicker) {
      // ui element is shown so user is hiding the date picker so we need to reset it back to the incident start time
      this.defaultDate = new Date(this.registration.incidentTask.startDate);
      this.showDefaultDatePicker = false;
    } else {
      // ui element is hidden show the ui element
      this.showDefaultDatePicker = true;
    }
  }
  updateDefaultDate() {
    // this hides and shows the picker
    this.defaultDate = this.workingDefaultDate;
    !this.defaultDate ? this.showDefaultDatePicker = true : this.showDefaultDatePicker = false;
  }
  // --------------------HELPERS-----------------------------------------
  remove(arr: [], i: number) {
    if (arr) {
      arr.splice(i, 1);
      this.updateFormValidity();
    }
  }

}
