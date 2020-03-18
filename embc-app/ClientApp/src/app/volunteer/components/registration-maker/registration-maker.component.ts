import { Component, OnInit, AfterViewInit, Input, ElementRef, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, mergeMap, filter } from 'rxjs/operators';
import * as moment from 'moment';

import { AppState } from 'src/app/store';
import { RegistrationService } from 'src/app/core/services/registration.service';
import {
  Registration, FamilyMember, isBcAddress, Community, Country, Volunteer, IncidentTask, Address, User
} from 'src/app/core/models';
import { IncidentTaskService } from 'src/app/core/services/incident-task.service';
import { ValidationHelper } from 'src/app/shared/validation/validation.helper';
import { hasErrors, invalidField, clearFormArray, compareById } from 'src/app/shared/utils';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';
import { GENDER_OPTIONS, INSURANCE_OPTIONS, EVERYONE, VOLUNTEER, LOCAL_AUTHORITY, PROVINCIAL_ADMIN } from 'src/app/constants';
import { AuthService } from 'src/app/core/services/auth.service';
import { NotificationQueueService } from 'src/app/core/services/notification-queue.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { of } from 'rxjs';

@Component({
  selector: 'app-registration-maker',
  templateUrl: './registration-maker.component.html',
  styleUrls: ['./registration-maker.component.scss']
})
export class RegistrationMakerComponent implements OnInit, AfterViewInit {

  @Output()
  onSummary: EventEmitter<boolean> = new EventEmitter<boolean>();

  // state needed by this FORM
  countries$ = this.store.select(s => s.lookups.countries.countries);
  regions$ = this.store.select(s => s.lookups.regions);
  relationshipTypes$ = this.store.select(s => s.lookups.relationshipTypes.relationshipTypes);
  incidentTasks$ = this.incidentTaskService.getOpenIncidentTasks().pipe(map(x => x.data));

  CANADA: Country; // the object representation of the default country

  pageTitle = 'New Registration';
  activeForm: boolean; // not set by default

  // The model for the form data collected
  form: FormGroup;
  componentActive = true;
  submitted = false;

  // Flags for the different modes this form supports
  createMode = true;
  finalizeMode = false; // finalize mode is on when the registration is incomplete
  editMode = false; // edit mode is the mode where the form is fed data from the api. (Changes text and etc.)
  summaryMode = false; // just show the summary
  submitting = false; // this is what disables buttons on submit
  // DECLARATION AND CONSENT MUST BE CHECKED BEFORE SUBMIT
  declarationAndConsent: FormControl = new FormControl(null);

  // this is the "final copy" or version of the content in the form that will be updated or refreshed.
  registration: Registration;
  // who's completing/editing this evacuee registration
  currentUser: User;
  submission: any;

  // convenience getters so we can use helper functions in Angular templates
  hasErrors = hasErrors;
  compareById = compareById;

  // `validationErrors` represents an object with field-level validation errors to display in the form
  validationErrors: { [key: string]: any } = {};

  // error summary to display; i.e. 'Some required fields have not been completed.'
  errorSummary = '';

  path: string = null; // the base path for routing

  readonly dateMask = [/\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/]; // yyyy-mm-dd
  readonly phoneMask = [/\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/]; // 999-999-9999

  // generic validation helper
  private constraints: { [key: string]: { [key: string]: string | { [key: string]: string } } };
  private validationHelper: ValidationHelper;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store<AppState>, // ngrx app state
    private registrationService: RegistrationService,
    private incidentTaskService: IncidentTaskService,
    private router: Router,
    private notificationQueueService: NotificationQueueService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private el: ElementRef
  ) {
    // Defines all of the validation messages for the form.
    // These could instead be retrieved from a file or database.
    this.constraints = {
      incidentTask: {
        required: 'Please select a task number from the list.'
      },
      facility: {
        required: 'Please state the Facility Name / Registration Location.'
      },
      hostCommunity: {
        required: 'Please select the host community from the list.'
      },
      headOfHousehold: {
        firstName: {
          required: 'Please enter a first name.',
        },
        lastName: {
          required: 'Please enter a last name.',
        },
        dob: {
          required: 'Please enter date of birth.',
          date: 'Please enter a valid date.',
          maxDate: 'Date of birth must be today or in the past.',
        },
      },
      registeringFamilyMembers: {
        required: 'Please register any immediate family members who live within the same household as the evacuee.',
      },
      primaryResidenceInBC: {
        required: 'Please make a selection regarding your primary residence.',
      },
      phoneNumber: {
        phone: 'Please enter a valid telephone number.',
      },
      phoneNumberAlt: {
        phone: 'Please enter a valid telephone number.',
      },
      email: {
        email: 'Please enter a valid email address.',
      },
      mailingAddressSameAsPrimary: {
        required: 'Please select whether your mailing address is the same as your primary residence.',
      },
      mailingAddressInBC: {
        required: 'Please make a selection regarding your mailing address.',
      },
      insuranceCode: {
        required: 'Please make a selection regarding insurance coverage.',
      },
      dietaryNeeds: {
        required: 'Please make a selection regarding dietary requirements.',
      },
      medicationNeeds: {
        required: 'Please make a selection regarding medication.',
      },
      hasThreeDayMedicationSupply: {
        required: 'Please make a selection regarding medication supply.',
      },
      hasPets: {
        required: 'Please make a selection regarding pets.',
      },
      requiresAccommodation: {
        required: 'Please make a selection regarding lodging while evacuated.',
      },
      requiresClothing: {
        required: 'Please make a selection regarding clothing while evacuated.',
      },
      requiresFood: {
        required: 'Please make a selection regarding food while evacuated.',
      },
      requiresIncidentals: {
        required: 'Please make a selection regarding incidentals while evacuated.',
      },
      requiresTransportation: {
        required: 'Please make a selection regarding transportation while evacuated.',
      },
    };

    // TODO: Wow. it sure would be nice if we could just instatiate a class instead of using interfaces
    this.registration = this.blankRegistration();

    // Define an instance of the validator for use with this form,
    // passing in this form's set of validation messages.
    this.validationHelper = new ValidationHelper(this.constraints);
  }

  // convenience getter for easy access to form fields within the HTML template
  get f(): any { return this.form.controls; }

  // convenience getter for easy access to validation errors within the HTML template
  get e(): any { return this.validationErrors; }

  // Shortcuts for this.form.get(...)
  // this is a way to grab the familymembers in a typed way
  get familyMembers() { return this.f.familyMembers as FormArray; }

  setRestricted(state: boolean) {
    // if not restricted then show the form
    this.activeForm = !state;
    // set the value of the restricted form element
    this.form.patchValue({ restrictedAccess: state });
  }

  ngOnInit() {
    // which path should this user use to route to and from this component
    this.authService.path.subscribe((path: string) => this.path = path);

    // fetch the default country
    this.countries$.subscribe((countries: Country[]) => {
      // the only (first) element that is named Canada
      countries.forEach((country: Country) => {
        // if the canada is not set and we found one in the list
        if (country.name === 'Canada') {
          this.CANADA = country;
        }
      });
    });

    // Create form controls
    this.initForm();

    // Watch for value changes
    this.onFormChange();

    // Know the current user
    this.authService.getCurrentUser().subscribe((user: User) => this.currentUser = user);
    this.authService.role.subscribe((role: string) => {
      // If the user Everyone or Volunteer (ERA User) then they cannot choose the task number.
      if (role !== PROVINCIAL_ADMIN && role !== LOCAL_AUTHORITY){
        this.f.incidentTask.disable();
      }
      console.log(this.f.incidentTask.disabled);
    });
    // // if there are route params we should grab them
    // const id = this.route.snapshot.paramMap.get('id');

    // get unique key if one exists
    const regId = this.uniqueKeyService.getKey();
    if (regId) {
      // this is a form with data flowing in
      this.registrationService.getRegistrationById(regId, 'editing')
        .subscribe((registration: Registration) => {
          // explicit fallback in case value from db is null/undefined
          registration.restrictedAccess = registration.restrictedAccess ? true : false;

          // set registration mode to edit and save the previous content in an object
          // NOTE: these flags are reversed! ie, requiresAccomodation means "claims to have accomodation on self reg"
          registration.requiresAccommodation = registration.requiresAccommodation;
          registration.requiresClothing = registration.requiresClothing;
          registration.requiresFood = registration.requiresFood;
          registration.requiresIncidentals = registration.requiresIncidentals;
          registration.requiresTransportation = registration.requiresTransportation;

          // hide/show form accordingly
          this.activeForm = !registration.restrictedAccess;

          this.registration = registration;
          this.editMode = true;
          this.displayRegistration(registration);
        }, err => {
          this.notificationQueueService.addNotification('Failed to load evacuee', 'danger');
          console.log('error getting registration =', err);

          // go back to list of evacuees
          this.router.navigate([`/${this.path}/registrations`]);
        });
    } else {
      // this is a fresh form
      this.displayRegistration();
    }
  }

  ngAfterViewInit() {
    // focus the first input
    const elements = document.getElementsByClassName('form-control') as HTMLCollectionOf<HTMLElement>;
    if (elements.length > 0) {
      elements[0].focus();
    } else {
      // wait for elements to display and try again
      setTimeout(() => this.ngAfterViewInit(), 100);
    }
  }

  invalid(field: string, parent: FormGroup = this.form): boolean {
    return invalidField(field, parent, this.submitted);
  }

  addFamilyMember(fmbr?: FamilyMember): void {
    this.familyMembers.push(this.createFamilyMember(fmbr));
  }

  removeFamilyMember(i: number): void {
    this.familyMembers.removeAt(i);
  }

  // reset the list of family members
  clearFamilyMembers(): void {
    clearFormArray(this.familyMembers);
  }

  // family member formgroup
  createFamilyMember(fmbr?: FamilyMember): FormGroup {
    if (fmbr) {
      return this.formBuilder.group({
        bcServicesNumber: fmbr.bcServicesNumber || null,
        id: fmbr.id || null,
        active: fmbr.active || null,
        sameLastNameAsEvacuee: fmbr.sameLastNameAsEvacuee,
        firstName: [fmbr.firstName, Validators.required],
        lastName: [this.asStringAndUpperCase(fmbr.lastName), Validators.required],
        nickname: fmbr.nickname,
        initials: fmbr.initials,
        gender: fmbr.gender,
        dob: [fmbr.dob, [Validators.required, CustomValidators.date('YYYY-MM-DD'), CustomValidators.maxDate(moment())]],
      });
    } else {
      // make a new family member blank and return it.
      return this.formBuilder.group({
        sameLastNameAsEvacuee: true,
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        initials: '',
        gender: null,
        dob: [null, [Validators.required, CustomValidators.date('YYYY-MM-DD'), CustomValidators.maxDate(moment())]],
      });
    }
  }

  initForm(): void {
    this.form = this.formBuilder.group({
      restrictedAccess: null,
      essFileNumber: null,
      dietaryNeeds: [null, Validators.required],
      dietaryNeedsDetails: [null],
      petCarePlan: [null],
      disasterAffectDetails: [null],
      externalReferralsDetails: '',
      facility: [null, Validators.required],
      familyRecoveryPlan: [null],
      internalCaseNotes: [null],
      insuranceCode: [null, Validators.required],  // one of ['yes', 'yes-unsure', 'no', 'unsure']
      medicationNeeds: [null, Validators.required],
      selfRegisteredDate: null,
      registrationCompletionDate: null,
      registeringFamilyMembers: [null, Validators.required],
      hasThreeDayMedicationSupply: [null, CustomValidators.requiredWhenTrue('medicationNeeds')],
      hasInquiryReferral: null,
      hasHealthServicesReferral: null,
      hasFirstAidReferral: null,
      hasChildCareReferral: null,
      hasPersonalServicesReferral: null,
      hasPetCareReferral: null,
      hasPets: [null, Validators.required],
      requiresAccommodation: [null, Validators.required],
      requiresClothing: [null, Validators.required],
      requiresFood: [null, Validators.required],
      requiresIncidentals: [null, Validators.required],
      requiresTransportation: [null, Validators.required],

      // HOH fields that we decided to put at the parent form level to simplify things
      phoneNumber: '', // only BC phones will be validated so keep validators out of here...
      phoneNumberAlt: '',

      email: ['', Validators.email],

      primaryResidence: this.formBuilder.group({
        addressSubtype: null, // address fields are validated by sub-components (bc-address, other-address)
        addressLine1: '',
        postalCode: '',
        community: '',
        city: '',
        province: '',
        country: '',
      }),

      mailingAddress: this.formBuilder.group({
        addressSubtype: null, // address fields are validated by sub-components (bc-address, other-address)
        addressLine1: '',
        postalCode: '',
        community: '',
        city: '',
        province: '',
        country: '',
      }),

      headOfHousehold: this.formBuilder.group({
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        nickname: '',
        initials: '',
        gender: null,
        dob: [null, [Validators.required, CustomValidators.date('YYYY-MM-DD'), CustomValidators.maxDate(moment())]],
      }),

      familyMembers: this.formBuilder.array([]), // array of formGroups

      incidentTask: [{value: null}, [Validators.required]], // which task is this from
      hostCommunity: [null, Validators.required], // which community is hosting

      // UI booleans
      primaryResidenceInBC: [null, Validators.required],
      // this will be validated when 'mailingAddressSameAsPrimary == false'
      mailingAddressInBC: [null, CustomValidators.requiredWhenFalse('mailingAddressSameAsPrimary')],
      mailingAddressSameAsPrimary: [null, Validators.required],
    });

    // set task number
    this.store.select(state => state.volunterTask.taskNumber)
      .pipe(filter(n => !!n))
      .pipe(map(taskNumber => {
        this.incidentTasks$.subscribe(tasks => {
          const task = tasks.find(t => t.taskNumber === taskNumber);
          this.form.get('incidentTask').patchValue(task);
        });
      })).subscribe();
  }

  onFormChange(): void {
    // validate the whole form as we capture data
    this.form.valueChanges.subscribe(() => this.validateForm());

    // show/hide family members section based on the "family info" radio button
    this.f.registeringFamilyMembers.valueChanges
      .subscribe((value: string) => {
        if (value === 'yes' && this.familyMembers.length === 0) {
          this.addFamilyMember();
        }
        if (value === 'no') {
          // TODO: should prompt before clearing any entered family member data
          this.clearFamilyMembers();
        }
      });

    // set "family info" radio to "no family" when all members have been removed from the form
    this.familyMembers.valueChanges
      .subscribe((family: any[]) => {
        const radio = this.f.registeringFamilyMembers;
        if (radio.value === 'yes' && family.length === 0) {
          radio.setValue('no');
        }
      });

    // validate phone numbers, for BC residents ONLY!
    // NOTE - international numbers are not validated due to variance in formats, etc.
    this.f.primaryResidenceInBC.valueChanges
      .subscribe((checked: boolean) => {
        if (checked) {
          this.f.phoneNumber.setValidators([CustomValidators.phone]);
          this.f.phoneNumberAlt.setValidators([CustomValidators.phone]);
        } else {
          this.f.phoneNumber.setValidators(null);
          this.f.phoneNumberAlt.setValidators(null);
        }
        this.f.phoneNumber.updateValueAndValidity();
        this.f.phoneNumberAlt.updateValueAndValidity();
      });
  }

  validateForm(): void {
    this.validationErrors = this.validationHelper.processMessages(this.form);
  }

  displayRegistration(r?: Registration | null): void {
    // Display the appropriate page title and form state
    if (r == null) {
      // null registration means this is a new registration
      this.pageTitle = 'New Registration';
      this.createMode = true;
      this.finalizeMode = false; // turn off these
    } else if (!r.isFinalized) {
      this.pageTitle = `Finalize Evacuee Registration ${r.essFileNumber}`;
      this.finalizeMode = true;
      this.createMode = false; // turn off these
    } else {
      this.pageTitle = 'Edit Evacuee Registration';
      this.createMode = this.finalizeMode = false; // turn off these

    }

    if (r && this.form) {
      // Reset the form back to pristine
      this.form.reset();

      const familyMembers: FamilyMember[] = r.headOfHousehold.familyMembers;
      const primaryResidence: Address = r.headOfHousehold.primaryResidence;
      const mailingAddress: Address = r.headOfHousehold.mailingAddress;
      const incidentTask: IncidentTask = r.incidentTask;
      const hostCommunity: Community = r.hostCommunity;

      // If the evacuee is here now then the defer to later of the registration of family members is now currently yes.
      if (r.registeringFamilyMembers === 'yes-later') {
        r.registeringFamilyMembers = 'yes';
      }

      // iterate over the array and collect each family member as a formgroup and put them into a form array
      // we need to do this before we update the main form so it populates the FormArray properly
      if (familyMembers != null) {
        familyMembers.forEach((m: FamilyMember) => {
          this.addFamilyMember(m);
        });
      }

      // some form fields for showing or hiding UI elements
      const primaryResidenceInBC: boolean = isBcAddress(primaryResidence);
      const mailingAddressInBC: boolean = isBcAddress(mailingAddress);
      const mailingAddressSameAsPrimary: boolean = (mailingAddress == null);

      // Update the data on the form from the data included from the API
      this.form.patchValue({
        id: r.id as string,
        restrictedAccess: r.restrictedAccess as boolean,
        essFileNumber: r.essFileNumber as number,
        dietaryNeeds: r.dietaryNeeds as boolean,
        dietaryNeedsDetails: r.dietaryNeedsDetails as string,
        petCarePlan: r.petCarePlan as string,
        disasterAffectDetails: r.disasterAffectDetails as string,
        externalReferralsDetails: r.externalReferralsDetails as string,
        facility: r.facility as string,
        familyRecoveryPlan: r.familyRecoveryPlan as string,
        internalCaseNotes: r.internalCaseNotes as string,
        insuranceCode: r.insuranceCode as string,
        medicationNeeds: r.medicationNeeds as boolean,
        selfRegisteredDate: r.selfRegisteredDate as string,
        registrationCompletionDate: r.registrationCompletionDate as string,
        registeringFamilyMembers: r.registeringFamilyMembers as string,

        hasThreeDayMedicationSupply: r.hasThreeDayMedicationSupply as boolean,

        hasInquiryReferral: r.hasInquiryReferral as boolean,
        hasHealthServicesReferral: r.hasHealthServicesReferral as boolean,
        hasFirstAidReferral: r.hasFirstAidReferral as boolean,
        hasChildCareReferral: r.hasChildCareReferral as boolean,
        hasPersonalServicesReferral: r.hasPersonalServicesReferral as boolean,
        hasPetCareReferral: r.hasPetCareReferral as boolean,

        hasPets: r.hasPets as boolean,

        requiresAccommodation: r.requiresAccommodation as boolean,
        requiresClothing: r.requiresClothing as boolean,
        requiresFood: r.requiresFood as boolean,
        requiresIncidentals: r.requiresIncidentals as boolean,
        requiresTransportation: r.requiresTransportation as boolean,

        headOfHousehold: {
          // id: r.headOfHousehold.id as string,
          // active: r.headOfHousehold.active as boolean,
          firstName: r.headOfHousehold.firstName as string,
          lastName: this.asStringAndUpperCase(r.headOfHousehold.lastName),
          nickname: r.headOfHousehold.nickname as string,
          initials: r.headOfHousehold.initials as string,
          gender: r.headOfHousehold.gender as string,
          dob: r.headOfHousehold.dob as string,
          // bcServicesNumber: r.headOfHousehold.bcServicesNumber as string,
          // personType: r.headOfHousehold.personType,
        },

        // these belong to the HOH but we placed them here to simplify the HTML markup...
        phoneNumber: r.headOfHousehold.phoneNumber as string,
        phoneNumberAlt: r.headOfHousehold.phoneNumberAlt as string,

        email: r.headOfHousehold.email as string,

        // primaryResidence: r.headOfHousehold.primaryResidence as Address,
        // mailingAddress: r.headOfHousehold.mailingAddress as Address,
        hostCommunity: hostCommunity as Community,
        incidentTask: incidentTask as IncidentTask,

        // UI booleans
        primaryResidenceInBC: primaryResidenceInBC as boolean,
        mailingAddressInBC: mailingAddressInBC as boolean,
        mailingAddressSameAsPrimary: mailingAddressSameAsPrimary as boolean,
      });

      // add the primary residence back into the form
      if (primaryResidence != null) {
        this.form.patchValue({
          primaryResidence: {
            addressSubtype: primaryResidence.addressSubtype as string,
            addressLine1: primaryResidence.addressLine1 as string,
            postalCode: primaryResidence.postalCode as string,
            community: primaryResidence.community as Community,
            city: primaryResidence.city as string,
            province: primaryResidence.province as string,
            country: primaryResidence.country as Country,
          },
        });
      }
      // add the mailing address back into the form
      if (mailingAddress != null) {
        this.form.patchValue({
          mailingAddress: {
            addressSubtype: mailingAddress.addressSubtype as string,
            addressLine1: mailingAddress.addressLine1 as string,
            postalCode: mailingAddress.postalCode as string,
            community: mailingAddress.community as Community,
            city: mailingAddress.city as string,
            province: mailingAddress.province as string,
            country: mailingAddress.country as Country,
          },
        });
      }
    }
  }

  next() {
    this.submitting = true; // disables buttons while we process the form
    this.submitted = true; // used for invalid feedback // TODO: possibly get rid of this
    this.errorSummary = null;
    this.validateForm();

    // stop here if form is invalid
    if (this.form.invalid) {
      //this.errorSummary = 'Some required fields have not been completed.';
      this.errorSummary = this.getValidationErrorSummary();
      this.submitting = false; // reenable so they can try again
      //this.form.markAsTouched();
      //this.scrollToFirstInvalidControl();
    } else {
      // success!
      this.errorSummary = null;
      // save the registration by copying the properties into it.
      this.registration = this.collectRegistrationFromForm();

      // navigate to the next page. AKA show the summary part of the form.
      this.summaryMode = true;
      this.onSummary.emit(true);
      this.submitting = false; // reenable when we parse data
      window.scrollTo(0, 0); // scroll to top
    }
  }

  private getValidationErrorSummary(): string {
    let result: string = "";
    const errors = this.validationErrors;
    // loop through each validation error and add it to the string
    for (let key in errors) {
      // Most errors are just strings
      if (typeof errors[key] === "string") {
        let err = errors[key];
        // Do not add empty line breaks
        err = err !== "" && err != null
          ? `${err}\n`
          : '';
        result += err;
      }
      // a few errors are nested objects
      else {
        let nestedErrors = errors[key];
        for (let nestedKey in nestedErrors) {
          let err = nestedErrors[nestedKey];
          err = err !== "" && err != null
            ? `${err}\n`
            : '';
          result += err;
        }
      }
    }
    return result;
  }


  private scrollToFirstInvalidControl() {
    // Reference to the first invalid control
    const firstInvalidControl: HTMLElement = this.el.nativeElement.querySelector("form .ng-invalid");
    window.scroll({
      top: this.getTopOffset(firstInvalidControl),
      left: 0,
      behavior: "smooth"
    });
  }

  private getTopOffset(controlEl: HTMLElement): number {
    // Could calculate this with another dom query - we want the height of the closest label element (or <p> for radio groups) + ~5px more for comfort
    // 25 seems pretty comfortable testing in a desktop browser and it's mobile emulators.
    const labelOffset = 25;
    return controlEl.getBoundingClientRect().top + window.scrollY - labelOffset;
  }


  submit(addReferrals: boolean = false) {
    this.submitted = true; // send data to the server
    this.submitting = true; // in transmission

    // these are represented opposite in the db. So these are flipped on page load then flipped on submit
    const r = this.registration;
    r.requiresAccommodation = !r.requiresAccommodation;
    r.requiresClothing = !r.requiresClothing;
    r.requiresFood = !r.requiresFood;
    r.requiresIncidentals = !r.requiresIncidentals;
    r.requiresTransportation = !r.requiresTransportation;

    // create or update registration
    // TODO: should this be editmode instead?
    if (r.id == null) {
      this.registrationService.createRegistration(r)
        .subscribe((registration: Registration) => {
          this.submitting = false;

          // add a notification to the queue
          this.notificationQueueService.addNotification('Evacuee added successfully', 'success');
          if (addReferrals) {
            // save registration ID for lookup in the new component
            this.uniqueKeyService.setKey(registration.id);

            // go to summary page
            this.router.navigate([`/${this.path}/registration/summary`]);
          } else {
            // done adding the entry - clear the reference key
            this.uniqueKeyService.clearKey();

            // go to home page
            this.router.navigate([`/${this.path}`]);
          }
        }, err => {
          this.notificationQueueService.addNotification('Failed to add registration', 'danger');
          console.log('error creating registration =', err);
        });
    } else {
      // update existing registration
      this.registrationService.updateRegistration(r)
        .subscribe(() => {
          this.submitting = false;
          // add a notification to the queue
          this.notificationQueueService.addNotification('Evacuee updated successfully', 'success');
          if (addReferrals) {
            // save registration ID for lookup in the new component
            this.uniqueKeyService.setKey(r.id);

            // go to summary page
            this.router.navigate([`/${this.path}/registration/summary`]);
          } else {
            // done editing the entry - clear the reference key
            this.uniqueKeyService.clearKey();

            // go to home page
            this.router.navigate([`/${this.path}`]);
          }
        }, err => {
          this.notificationQueueService.addNotification('Failed to update evacuee', 'danger');
          console.log('error updating registration =', err);
        });
    }
  }

  back() {
    // return to the edit mode so you can change the form data
    this.summaryMode = false;
    this.onSummary.emit(false);
    window.scrollTo(0, 0); // scroll to top
  }

  collectRegistrationFromForm(): Registration {
    const values = this.form.value;
    // ensure proper sub-types are assigned to people entities
    const personType: 'FMBR' = 'FMBR';
    const familyMembers: FamilyMember[] = (values.familyMembers as FamilyMember[]).map(fmr => ({ ...fmr, personType }));

    // Use form values to create evacuee registration
    const r: Registration = {
      id: null,
      active: null,
      declarationAndConsent: null,
      essFileNumber: null,

      headOfHousehold: {
        id: null,
        firstName: values.headOfHousehold.firstName || null,
        lastName: values.headOfHousehold.lastName || null,
        initials: values.headOfHousehold.initials || null,
        nickname: values.headOfHousehold.nickname || null,
        gender: values.headOfHousehold.gender || null,
        dob: values.headOfHousehold.dob || null,
        personType: 'HOH' || null,
        phoneNumber: values.phoneNumber || null,
        phoneNumberAlt: values.phoneNumberAlt || null,
        email: values.email || null,
        familyMembers, // copy in the already parsed values for familymembers
        primaryResidence: { ...values.primaryResidence },
        mailingAddress: values.mailingAddressSameAsPrimary ? null : { ...values.mailingAddress },
      },

      // Registration Record
      restrictedAccess: values.restrictedAccess,
      dietaryNeeds: values.dietaryNeeds as boolean,
      dietaryNeedsDetails: values.dietaryNeedsDetails as string,
      petCarePlan: values.petCarePlan as string,
      disasterAffectDetails: values.disasterAffectDetails as string,
      externalReferralsDetails: this.asStringAndTrim(values.externalReferralsDetails),
      facility: values.facility as string,
      familyRecoveryPlan: this.asStringAndTrim(values.familyRecoveryPlan),
      internalCaseNotes: this.asStringAndTrim(values.internalCaseNotes),
      insuranceCode: values.insuranceCode as string,
      medicationNeeds: values.medicationNeeds as boolean,
      registeringFamilyMembers: values.registeringFamilyMembers as string, // 'yes' or 'no'

      // Family state flags
      hasThreeDayMedicationSupply: values.hasThreeDayMedicationSupply as boolean,
      hasInquiryReferral: values.hasInquiryReferral as boolean,
      hasHealthServicesReferral: values.hasHealthServicesReferral as boolean,
      hasFirstAidReferral: values.hasFirstAidReferral as boolean,
      hasChildCareReferral: values.hasChildCareReferral as boolean,
      hasPersonalServicesReferral: values.hasPersonalServicesReferral as boolean,
      hasPetCareReferral: values.hasPetCareReferral as boolean,
      hasPets: values.hasPets as boolean,

      // requirements
      requiresAccommodation: values.requiresAccommodation as boolean,
      requiresClothing: values.requiresClothing as boolean,
      requiresFood: values.requiresFood as boolean,
      requiresIncidentals: values.requiresIncidentals as boolean,
      requiresTransportation: values.requiresTransportation as boolean,

      // dates we care about
      selfRegisteredDate: values.selfRegisteredDate as string,
      registrationCompletionDate: new Date().toJSON() as string, // this stamps whenever the registration was completed

      // related entities
      incidentTask: values.incidentTask,
      hostCommunity: values.hostCommunity,
    };

    const registration = this.registration;
    if (this.editMode) {
      // if we are editing the form we assign the values collected when the form initialized and collected the registration from the api.
      r.id = registration.id;
      r.active = registration.active || null;
      r.declarationAndConsent = registration.declarationAndConsent || null;
      r.essFileNumber = registration.essFileNumber || null;
      r.headOfHousehold.id = registration.headOfHousehold.id || null;
      r.registrationCompletionDate = registration.registrationCompletionDate || null; // todo need to check if this date is being handled correctly
      r.headOfHousehold.primaryResidence.id = registration.headOfHousehold.primaryResidence.id || null;
    }

    // timestamp the completion date on
    r.registrationCompletionDate = r.registrationCompletionDate || new Date().toJSON();

    // The user now consents.
    r.declarationAndConsent = true;

    // the user included a primary address but the form did not set the country
    if (!r.headOfHousehold.primaryResidence.country) {
      r.headOfHousehold.primaryResidence.country = this.CANADA;
    }

    // the user included a mailing address but the form did not set the country
    if (r.headOfHousehold.mailingAddress && !r.headOfHousehold.mailingAddress.country) {
      r.headOfHousehold.mailingAddress.country = this.CANADA;
    }

    // if they set Dietary Needs to false then delete Dietary Needs Details
    if (!r.dietaryNeeds) {
      r.dietaryNeedsDetails = '';
    }

    // if they set pets to false then delete pet care plan details.
    if (!r.petCarePlan) {
      r.petCarePlan = '';
    }

    // if they set Medication Needs to false then set Has Three Day Medication Supply to false
    if (!r.medicationNeeds) {
      r.hasThreeDayMedicationSupply = false;
    }
    // return the registration
    return r;
  }

  private asStringAndTrim(value: any): string {
    const s = value as string;
    return s ? s.trim() : null;
  }

  private asStringAndUpperCase(value: any): string {
    const s = value as string;
    return s ? s.toUpperCase() : null;
  }

  cancel() {
    // clear the loaded record if available
    this.uniqueKeyService.clearKey();
    // navigate back home
    this.router.navigate([`/${this.path}/registrations`]);
  }

  // --------------------HELPERS-----------------------------------------
  isBcAddress(address: Address): boolean {
    return isBcAddress(address);
  }

  genderOption(key: string) {
    const option = GENDER_OPTIONS.find(item => item.key === key);
    return option ? option.value : null;
  }

  insuranceOption(key: string) {
    const option = INSURANCE_OPTIONS.find(item => item.key === key);
    return option ? option.value : null;
  }

  blankRegistration(): Registration {
    // This is a workaround for not having an instantiable class that initializes the interface
    return {
      id: null,
      active: null,
      restrictedAccess: null,
      declarationAndConsent: null,
      essFileNumber: null,
      dietaryNeeds: null,
      dietaryNeedsDetails: null,
      petCarePlan: null,
      disasterAffectDetails: null,
      externalReferralsDetails: null,
      facility: null,
      familyRecoveryPlan: null,
      internalCaseNotes: null,
      insuranceCode: null,
      medicationNeeds: null,
      registrationCompletionDate: null,
      registeringFamilyMembers: null,
      selfRegisteredDate: null,
      hasThreeDayMedicationSupply: null,
      hasInquiryReferral: null,
      hasHealthServicesReferral: null,
      hasFirstAidReferral: null,
      hasChildCareReferral: null,
      hasPersonalServicesReferral: null,
      hasPetCareReferral: null,
      hasPets: null,
      requiresAccommodation: null,
      requiresClothing: null,
      requiresFood: null,
      requiresIncidentals: null,
      requiresTransportation: null,
      headOfHousehold: null,
      incidentTask: null,
      hostCommunity: null,
      isFinalized: null
    };
  }

  hasPetsChanged() {
    // If the value is false, clear out any pet care plans
    if (!Boolean(this.form.get('hasPets').value)) {
      this.form.get('petCarePlan').setValue(null);
    }
  }


}
