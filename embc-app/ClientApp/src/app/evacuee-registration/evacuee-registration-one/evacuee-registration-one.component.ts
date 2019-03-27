import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { map } from 'rxjs/operators';

import { AppState } from '../../store';
import { RegistrationService } from '../../core/services/registration.service';
import {
  Registration, FamilyMember, isBcAddress, Community, Country,
  RelationshipType, HeadOfHousehold, Address, Volunteer, IncidentTask
} from 'src/app/core/models';
import { IncidentTaskService } from '../../core/services/incident-task.service';
import { UpdateRegistration } from 'src/app/store/registration/registration.actions';
import { ValidationHelper } from 'src/app/shared/validation/validation.helper';
import { hasErrors, invalidField, clearFormArray } from 'src/app/shared/utils';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';


@Component({
  selector: 'app-evacuee-registration-one',
  templateUrl: './evacuee-registration-one.component.html',
  styleUrls: ['./evacuee-registration-one.component.scss']
})
export class EvacueeRegistrationOneComponent implements OnInit {
  // state needed by this FORM
  countries$ = this.store.select(s => s.lookups.countries.countries);
  regionalDistrics$ = this.store.select(s => s.lookups.regionalDistricts);
  regions$ = this.store.select(s => s.lookups.regions);
  relationshipTypes$ = this.store.select(s => s.lookups.relationshipTypes.relationshipTypes);
  communities$ = this.store.select(s => s.lookups.communities.communities);
  incidentTasks$ = this.incidentTaskService.getAllIncidentTasks().pipe(map(x => x.data));

  pageTitle = 'Add an Evacuee';

  // The model for the form data collected
  form: FormGroup;
  submitted = false;
  componentActive = true;

  // Flags for the different modes this form supports
  createMode = true;
  finalizeMode = false;
  editMode = false;

  registration: Registration;
  submission: any;
  // the ess file number on its own is useful for looking up information from the DB
  // essFileNumber: string;

  // convenience getters so we can use helper functions in Angular templates
  hasErrors = hasErrors;
  invalidField = invalidField;

  // `validationErrors` represents an object with field-level validation errors to display in the form
  validationErrors: { [key: string]: any } = {};

  // error summary to display; i.e. 'Some required fields have not been completed.'
  errorSummary = '';

  // generic validation helper
  private constraints: { [key: string]: { [key: string]: string | { [key: string]: string } } };
  private validationHelper: ValidationHelper;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store<AppState>, // ngrx app state
    private route: ActivatedRoute,
    private registrationService: RegistrationService,
    private incidentTaskService: IncidentTaskService,
    private router: Router
  ) {
    // Defines all of the validation messages for the form.
    // These could instead be retrieved from a file or database.
    this.constraints = {
      incidentTask: {
        required: 'Please select a task number from the dropdown list.'
      },
      facility: {
        required: 'Please enter a facility name.'
      },
      hostCommunity: {
        required: 'Please select a host community from the dropdown list.'
      },
      headOfHousehold: {
        firstName: {
          required: 'Please enter your first name.',
        },
        lastName: {
          required: 'Please enter your last name.',
        },
        dob: {
          required: 'Please enter your date of birth.',
          dateInThePast: 'Date of birth must be today or in the past.',
        },
      },
      registeringFamilyMembers: {
        required: 'Please register any immediate family members who live within the same household as you.',
      },
      primaryResidenceInBC: {
        required: 'Please make a selection regarding your primary residence.',
      },
      mailingAddressSameAsPrimary: {
        required: 'Please select whether your mailing address is the same as your primary residence.',
      },
      mailingAddressInBC: {
        required: 'Please make a selection regarding your mailing address.',
      },
      phoneNumber: {
        phone: 'Must be 10 digits, no spaces allowed.',
      },
      phoneNumberAlt: {
        phone: 'Must be 10 digits, no spaces allowed.',
      },
      email: {
        email: 'Please enter a valid email address.',
      }
    };

    // Define an instance of the validator for use with this form,
    // passing in this form's set of validation messages.
    this.validationHelper = new ValidationHelper(this.constraints);
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.form.controls;
  }

  // Shortcuts for this.form.get(...)
  get familyMembers() {
    // this is a way to grab the familymembers in a typed way
    return this.f.familyMembers as FormArray;
  }

  // isBcAddress(a: Address): boolean {
  //   if (a.province === 'BC') {
  //     return true;
  //   } else {
  //     return false;
  //   }
  // }

  // TODO: Review these...
  // setHohPrimaryResidenceProvince() {
  //   // if the unset flag is true or a value it clears the values
  //   const patch = { hohPrimaryResidence: { province: 'BC', country: { name: 'Canada' } } };
  //   this.form.patchValue(patch);
  // }
  // setHohMailingAddressProvince() {
  //   const patch = { hohMailingAddress: { province: 'BC', country: { name: 'Canada' } } };
  //   this.form.patchValue(patch);
  // }

  ngOnInit() {
    // Create form controls
    this.initForm();

    // Watch for value changes
    this.onFormChange();

    // if there are route params we should grab them
    const id = this.route.snapshot.params.id;

    if (id) {
      // this is a form with data flowing in.
      // TODO: Redirect to error page if we fail to fetch the registration
      this.registrationService.getRegistrationById(id).subscribe(r => this.displayRegistration(r));
    } else {
      // this is a fresh form
      this.displayRegistration();
    }
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
        sameLastNameAsEvacuee: fmbr.sameLastNameAsEvacuee,
        firstName: [fmbr.firstName, Validators.required],
        lastName: [fmbr.lastName, Validators.required],
        initials: fmbr.initials,
        gender: fmbr.gender,
        dob: [new Date(fmbr.dob).toString(), [Validators.required, CustomValidators.dateInThePast()]], // TODO: check this!!
        relationshipToEvacuee: [fmbr.relationshipToEvacuee, Validators.required],
      });
    } else {
      // make a new family member blank and return it.
      return this.formBuilder.group({
        sameLastNameAsEvacuee: true,
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        initials: '',
        gender: null,
        dob: [null, [Validators.required, CustomValidators.dateInThePast()]], // TODO: Split into [DD] [MM] [YYYY]
        relationshipToEvacuee: [null, Validators.required],
      });
    }
  }

  initForm(): void {
    this.form = this.formBuilder.group({
      restrictedAccess: false,
      essFileNumber: null,
      dietaryNeeds: [null, Validators.required],
      dietaryNeedsDetails: '',
      disasterAffectDetails: null,
      externalReferralsDetails: '',
      facility: [null, Validators.required],
      familyRecoveryPlan: '',
      followUpDetails: '',
      insuranceCode: [null, Validators.required],  // one of ['yes', 'yes-unsure', 'no', 'unsure']
      medicationNeeds: [null, Validators.required],
      selfRegisteredDate: null,
      registrationCompletionDate: null,
      registeringFamilyMembers: [null, Validators.required],
      hasThreeDayMedicationSupply: null,
      hasInquiryReferral: null,
      hasHealthServicesReferral: null,
      hasFirstAidReferral: null,
      hasChildCareReferral: null,
      hasPersonalServicesReferral: null,
      hasPetCareReferral: null,
      hasPets: [null, Validators.required],
      requiresAccommodation: null,
      requiresClothing: null,
      requiresFood: null,
      requiresIncidentals: null,
      requiresTransportation: null,
      requiresSupport: [null, Validators.required],

      // HOH fields that we decided to put at the parent form level to simplify things
      phoneNumber: '', // only BC phones will be validates so keep validators out of here...
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
        dob: [null, [Validators.required, CustomValidators.dateInThePast()]], // TODO: Split into [DD] [MM] [YYYY]
      }),

      familyMembers: this.formBuilder.array([]), // array of formGroups

      incidentTask: [null, Validators.required], // which task is this from
      hostCommunity: [null, Validators.required], // which community is hosting
      completedBy: null, // TODO: the volunteer completing this form (we need AUTH in place to do know who you are)

      // UI booleans
      primaryResidenceInBc: [null, Validators.required],
      mailingAddressInBc: null, // this will be validated when 'mailingAddressSameAsPrimary == false'
      mailingAddressSameAsPrimary: [null, Validators.required],
    });
  }

  onFormChange(): void {
    // validate the whole form as we capture data
    this.form.valueChanges.subscribe(() => this.validateForm());
  }

  validateForm(): void {
    this.validationErrors = this.validationHelper.processMessages(this.form);
  }

  displayRegistration(r?: Registration | null): void {
    // Set the local registration property
    this.registration = r;

    // Display the appropriate page title and form state
    if (r == null) {
      this.pageTitle = 'Add an Evacuee';
      this.createMode = true;
      this.finalizeMode = this.editMode = false; // turn off these
    } else {
      if (r.incidentTask == null) {
        this.pageTitle = 'Finalize Evacuee Registration';
        this.finalizeMode = true;
        this.createMode = this.editMode = false; // turn off these
      } else {
        this.pageTitle = 'Edit Evacuee Registration';
        this.editMode = true;
        this.createMode = this.finalizeMode = false; // turn off these
      }
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

      // some form fields for showing or hiding UI elements
      const primaryResidenceInBc: boolean = isBcAddress(primaryResidence);
      const mailingAddressInBc: boolean = isBcAddress(mailingAddress);
      const mailingAddressSameAsPrimary: boolean = (mailingAddress == null);

      // Update the data on the form from the data included from the API
      this.form.patchValue({
        // id: r.id as string,
        restrictedAccess: r.restrictedAccess as boolean,
        essFileNumber: r.essFileNumber as number,
        dietaryNeeds: r.dietaryNeeds as boolean,
        dietaryNeedsDetails: r.dietaryNeedsDetails as string,
        disasterAffectDetails: r.disasterAffectDetails as string,
        externalReferralsDetails: r.externalReferralsDetails as string,
        facility: r.facility as string,
        familyRecoveryPlan: r.familyRecoveryPlan as string,
        followUpDetails: r.followUpDetails as string,
        insuranceCode: r.insuranceCode as string,
        medicationNeeds: r.medicationNeeds as boolean,
        selfRegisteredDate: r.selfRegisteredDate as Date,
        registrationCompletionDate: r.registrationCompletionDate as Date,
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
        requiresSupport: r.requiresSupport as boolean,

        headOfHousehold: {
          // id: r.headOfHousehold.id as string,
          // active: r.headOfHousehold.active as boolean,
          firstName: r.headOfHousehold.firstName as string,
          lastName: r.headOfHousehold.lastName as string,
          nickname: r.headOfHousehold.nickname as string,
          initials: r.headOfHousehold.initials as string,
          gender: r.headOfHousehold.gender as string,
          dob: r.headOfHousehold.dob as Date,
          // bcServicesNumber: r.headOfHousehold.bcServicesNumber as string,
          // personType: r.headOfHousehold.personType,
        },

        // these belong to the HOH but we placed them here to simplify the HTML markup...
        phoneNumber: r.headOfHousehold.phoneNumber as string,
        phoneNumberAlt: r.headOfHousehold.phoneNumberAlt as string,
        email: r.headOfHousehold.email as string,

        // primaryResidence: r.headOfHousehold.primaryResidence as Address,
        // mailingAddress: r.headOfHousehold.mailingAddress as Address,
        completedBy: r.completedBy as Volunteer,
        hostCommunity: hostCommunity as Community,
        incidentTask: incidentTask as IncidentTask,

        // UI booleans
        primaryResidenceInBc: primaryResidenceInBc as boolean,
        mailingAddressInBc: mailingAddressInBc as boolean,
        mailingAddressSameAsPrimary: mailingAddressSameAsPrimary as boolean,
      });

      // iterate over the array and collect each family member as a formgroup and put them into a form array
      if (familyMembers != null) {
        familyMembers.forEach((m: FamilyMember) => {
          this.addFamilyMember(m);
        });
      }

      // TODO: Review the following:

      // // These are switches that will be handy maybe.
      // // incident task
      // if (incidentTask != null) {
      //   alert('There is an incident.');
      // }
      // // host community
      // if (hostCommunity != null) {
      //   alert('host community set');
      // }

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

  formCleanup() {
    // TODO: make sure this is sent back to the api in a well formed way.
    // freeze the form values into a constant
    const r = this.form.value;
    const reg: any = {
      id: r.id as string,
      restrictedAccess: r.restrictedAccess as boolean,
      essFileNumber: r.essFileNumber as number,

      dietaryNeeds: r.dietaryNeeds as boolean,
      dietaryNeedsDetails: r.dietaryNeedsDetails as string,
      disasterAffectDetails: r.disasterAffectDetails as string,
      externalReferralsDetails: r.externalReferralsDetails as string,
      facility: r.facility as string,
      familyRecoveryPlan: r.familyRecoveryPlan as string,
      followUpDetails: r.followUpDetails as string,
      insuranceCode: r.insuranceCode as string,
      medicationNeeds: r.medicationNeeds as boolean,
      selfRegisteredDate: r.selfRegisteredDate as Date,
      registrationCompletionDate: new Date() as Date, // this stamps whenever the data is cleaned up
      registeringFamilyMembers: r.registeringFamilyMembers as string, // 'yes' or 'no'
      hasThreeDayMedicationSupply: r.hasThreeDayMedicationSupply as boolean,
      hasInquiryReferral: r.hasInquiryReferral as boolean,
      hasHealthServicesReferral: r.hasHealthServicesReferral as boolean,
      hasFirstAidReferral: r.hasFirstAidReferral as boolean,
      hasChildCareReferral: r.hasChildCareReferral as boolean,
      hasPersonalServicesReferral: r.hasPersonalServicesReferral as boolean,
      hasPetCareReferral: r.hasPetCareReferral as boolean,
      hasPets: r.hasPets as boolean,
      requiresAccomodation: r.requiresAccomodation as boolean,
      requiresClothing: r.requiresClothing as boolean,
      requiresFood: r.requiresFood as boolean,
      requiresIncidentals: r.requiresIncidentals as boolean,
      requiresTransportation: r.requiresTransportation as boolean,
      requiresSupport: r.requiresSupport as boolean,

      headOfHousehold: {
        id: r.headOfHousehold.id as string,
        active: r.headOfHousehold.active as boolean,
        phoneNumber: r.headOfHousehold.phoneNumber as string,
        phoneNumberAlt: r.headOfHousehold.phoneNumberAlt as string,
        email: r.headOfHousehold.email as string,
        firstName: r.headOfHousehold.firstName as string,
        lastName: r.headOfHousehold.lastName as string,
        nickname: r.headOfHousehold.nickname as string,
        initials: r.headOfHousehold.initials as string,
        gender: r.headOfHousehold.gender as string,
        dob: r.headOfHousehold.dob as Date,
        bcServicesNumber: r.headOfHousehold.bcServicesNumber as string,
        primaryResidence: r.hohPrimaryResidence as Address,
        mailingAddress: r.hohMailingAddress as Address,
        familyMembers: r.familyMembers as FamilyMember[],
        personType: r.headOfHousehold.personType as string,
      },
      incidentTask: r.incidentTask as IncidentTask,
      hostCommunity: r.hostCommunity as Community,
    };


    return reg as Registration;
  }

  onSubmit() {
    this.submitted = true;
    this.validateForm();

    // stop here if form is invalid
    if (this.form.invalid) {
      this.errorSummary = 'Some required fields have not been completed.';
      return;
    }

    // success!
    this.errorSummary = null;
    this.next();

    // TODO: Review code below...

    // // assume that the registration data is dirty or unformatted
    // const reg = this.formCleanup();
    // // Submit the registration
    // if (this.registration) {
    //   // update
    //   this.submission = reg;
    //   this.registrationService.updateRegistration(reg)
    //     .subscribe(() => { });
    // } else {
    //   // post new
    //   this.submission = reg;
    //   this.registrationService.createRegistration(reg)
    //     .subscribe(r => {
    //       alert(JSON.stringify(r));
    //     });
    // }
  }

  next() {
    // const registration: Registration = {
    //   ...this.registration,
    //   ...this.form.value
    // };

    // update client-side state
    this.saveState();

    // navigate to the next page.
    // TODO flow to the next element
    this.router.navigate(['../confirmation'], { relativeTo: this.route });

    // this.registrationService.createRegistration(registration).subscribe(
    //   data => {
    //     console.log('NEW REGISTRATION ==>');
    //     console.log(data);
    //     this.router.navigate(['register-evacuee/confirmation']);
    //   },
    //   err => {
    //     // this.router.navigate(['../error'], { relativeTo: this.route });
    //   }
    // );

  }

  saveState() {
    const values = this.form.value;

    // ensure proper sub-types are assigned to people entities
    const personType: 'FMBR' = 'FMBR';
    const familyMembers: FamilyMember[] = (values.familyMembers as FamilyMember[]).map(fmr => ({ ...fmr, personType }));

    // Use form values to create evacuee registration
    const registration: Registration = {
      ...this.registration,

      headOfHousehold: {
        ...this.registration.headOfHousehold,
        ...values.headOfHousehold,
        personType: 'HOH',
        phoneNumber: values.phoneNumber,
        phoneNumberAlt: values.phoneNumberAlt,
        email: values.email,
        familyMembers,
        primaryResidence: { ...values.primaryResidence },
        mailingAddress: values.mailingAddressSameAsPrimary ? null : { ...values.mailingAddress },
      },

      // Registration Record
      restrictedAccess: values.restrictedAccess,
      dietaryNeeds: values.dietaryNeeds as boolean,
      dietaryNeedsDetails: values.dietaryNeedsDetails as string,
      disasterAffectDetails: values.disasterAffectDetails as string,
      externalReferralsDetails: values.externalReferralsDetails as string,
      facility: values.facility as string,
      familyRecoveryPlan: values.familyRecoveryPlan as string,
      followUpDetails: values.followUpDetails as string,
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
      requiresSupport: values.requiresSupport as boolean,

      // dates we care about
      selfRegisteredDate: values.selfRegisteredDate as Date,
      registrationCompletionDate: new Date() as Date, // this stamps whenever the registration was completed

      // related entities
      incidentTask: values.incidentTask,
      hostCommunity: values.hostCommunity,
      completedBy: values.completedBy,
    };

    // save the registration to the application state
    this.store.dispatch(new UpdateRegistration({ registration }));
  }
}
