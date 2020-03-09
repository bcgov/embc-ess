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
  TransportationReferral, IncidentalsReferral, RegistrationSummary
} from 'src/app/core/models';
import { ScrollToService, ScrollToConfigOptions } from '@nicky-lenaers/ngx-scroll-to';

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
  registrationSummary: RegistrationSummary = null;
  path: string = null; // the base path for routing
  regId: string = null;
  purchaser: string = null;
  evacuees: Array<Evacuee> = [];
  workingDefaultDate: Date;
  defaultDate: Date;
  showDefaultDatePicker = false;
  confirmChecked = false;
  isAdmin = false;
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
    private notificationQueueService: NotificationQueueService,
    private scrollToService: ScrollToService,
  ) { }

  ngOnInit() {
    // get path for routing
    this.authService.path.subscribe((path: string) => this.path = path);
    this.authService.isProvincialAdmin$.subscribe(result => this.isAdmin = result);
    // get URL params
    this.regId = this.route.snapshot.paramMap.get('regId');
    this.purchaser = this.route.snapshot.paramMap.get('purchaser');

    if (!this.regId || !this.purchaser) {
      // error - return to list
      console.log('ERROR - reg id or purchaser was not provided');
      this.router.navigate([`/${this.path}/registrations`]);
    }

    // get registration data
    this.registrationService.getRegistrationSummaryById(this.regId)
      .subscribe((registrationSummary: RegistrationSummary) => {
        if (!registrationSummary.id || !registrationSummary.essFileNumber) {
          console.log('ERROR - invalid registration object = ', registrationSummary);
          this.cancel();
        } else {
          // reverse the flags patch (Client didn't prioritize system-wide data fix.)
          registrationSummary.requiresAccommodation = !registrationSummary.requiresAccommodation; // referred to as "lodging"
          registrationSummary.requiresClothing = !registrationSummary.requiresClothing;
          registrationSummary.requiresFood = !registrationSummary.requiresFood;
          registrationSummary.requiresIncidentals = !registrationSummary.requiresIncidentals;
          registrationSummary.requiresTransportation = !registrationSummary.requiresTransportation;
          this.registrationSummary = registrationSummary;
          this.evacuees = this.createEvacueeList(registrationSummary);
          // this.defaultDate = new Date(registration.incidentTask.startDate); // previously default set to incident start time
          this.defaultDate = new Date();
        }
      }, err => {
        this.notificationQueueService.addNotification('Failed to load evacuee summary', 'danger');
        console.log('error getting registration summary =', err);

        // go back to home page
        this.router.navigate([`/${this.path}`]);
      });
  }

  public triggerScrollTo(anchor: string) {
    const config: ScrollToConfigOptions = {
      target: anchor
    };
    // wait for a split second
    setTimeout(() => {
      this.scrollToService.scrollTo(config);
    }, 1);
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
    // go back to registration summary
    this.router.navigate([`/${this.path}/registration/summary`]);
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
          this.notificationQueueService.addNotification('Referral(s) finalized successfully', 'success');

          // save registration ID for lookup in the new component
          this.uniqueKeyService.setKey(this.registrationSummary.id);

          // go to registration summary page
          this.router.navigate([`/${this.path}/registration/summary`]);
        }, err => {
          this.submitting = false;
          this.notificationQueueService.addNotification('Failed to finalize referral(s)', 'danger');
          console.log('error creating referrals =', err);
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

  addIncidentalsReferral(anchoring = false) {
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
    this.keepAlive();
    if (anchoring) { this.triggerScrollTo('incidentals_' + (this.incidentalsReferrals.length - 1).toString()); }
  }

  addFoodReferral(anchoring = false) {
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
    this.keepAlive();
    if (anchoring) { this.triggerScrollTo('food_' + (this.foodReferrals.length - 1).toString()); }
  }

  addLodgingReferral(anchoring = false) {
    const referral: Partial<LodgingReferral> = {
      essNumber: this.regId,
      referralId: null, // is populated by BE after save
      active: true,
      type: 'LODGING',
      purchaser: this.purchaser,
      validDates: { from: this.defaultDate, to: null, days: null },
      evacuees: [],
      supplier: { id: null, active: true, province: 'BC' },
      numRooms: 1
    };
    this.lodgingReferrals.push({ value: referral, valid: false });
    this.updateFormValidity();
    this.keepAlive();
    if (anchoring) { this.triggerScrollTo('lodging_' + (this.lodgingReferrals.length - 1).toString()); }
  }

  addClothingReferral(anchoring = false) {
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
    this.keepAlive();
    if (anchoring) { this.triggerScrollTo('clothing_' + (this.clothingReferrals.length - 1).toString()); }
  }

  addTransportationReferral(anchoring = false) {
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
    this.keepAlive();
    if (anchoring) { this.triggerScrollTo('_' + (this.transportationReferrals.length - 1).toString()); }
  }

  clearIncidentalsReferrals(): void {
    this.keepAlive();
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Incidentals referrals?')) {
      while (this.incidentalsReferrals.length > 0) { this.incidentalsReferrals.pop(); }
      this.updateFormValidity();
    }
  }

  clearFoodReferrals(): void {
    this.keepAlive();
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Food referrals?')) {
      while (this.foodReferrals.length > 0) { this.foodReferrals.pop(); }
      this.updateFormValidity();
    }
  }

  clearLodgingReferrals(): void {
    this.keepAlive();
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Lodging referrals?')) {
      while (this.lodgingReferrals.length > 0) { this.lodgingReferrals.pop(); }
      this.updateFormValidity();
    }
  }

  clearClothingReferrals(): void {
    this.keepAlive();
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Clothing referrals?')) {
      while (this.clothingReferrals.length > 0) { this.clothingReferrals.pop(); }
      this.updateFormValidity();
    }
  }

  clearTransportationReferrals(): void {
    this.keepAlive();
    // TODO: replace confirm with a better popup
    if (confirm('Do you really want to clear all Transportation referrals?')) {
      while (this.transportationReferrals.length > 0) { this.transportationReferrals.pop(); }
      this.updateFormValidity();
    }
  }

  // populate evacuees
  private createEvacueeList(reg: RegistrationSummary): Evacuee[] {
    if (!reg || !reg.headOfHousehold) { return []; }
    const hoh = reg.headOfHousehold;
    const family = hoh.familyMembers || [];
    return [hoh, ...family];
  }

  toggleDefaultDatePicker() {
    if (this.showDefaultDatePicker) {
      // ui element is shown so user is hiding the date picker so we need to reset it back to the incident start time
      this.defaultDate = new Date(this.registrationSummary.incidentTask.startDate);
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

  // reload user to refresh the session and session watchdog timer
  private keepAlive() {
    this.authService.login(true).subscribe();
  }

  // --------------------HELPERS-----------------------------------------
  remove(arr: [], i: number, scrollTo?: string) {
    this.keepAlive();
    // Scroll to first then remove the array element
    if (scrollTo) {
      this.triggerScrollTo(scrollTo);
    }
    if (arr) {
      arr.splice(i, 1);
      this.updateFormValidity();
    }
  }
}
