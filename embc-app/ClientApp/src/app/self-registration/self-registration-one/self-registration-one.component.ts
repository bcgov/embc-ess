import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormControl, FormArray } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { first } from "rxjs/operators";

import { AppState } from 'src/app/store/app-state';
import { UpdateRegistration } from "src/app/store/actions/registration.action";
import { Registration } from 'src/app/core/models';

@Component({
  selector: 'app-self-registration-one',
  templateUrl: './self-registration-one.component.html',
  styleUrls: ['./self-registration-one.component.scss']
})
export class SelfRegistrationOneComponent implements OnInit {
  form: FormGroup;
  registration: Registration;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  // Shortcuts for this.form.get(...)
  get isRegisteringFamilyMembers() { return this.form.get('isRegisteringFamilyMembers'); }
  get familyMembers() { return this.form.get('familyMembers') as FormArray; }
  get primaryResidence() { return this.form.get('primaryResidence') as FormGroup; }
  get mailingAddress() { return this.form.get('mailingAddress') as FormGroup; }

  // Form UI logic; i.e. show additional form fields when a checkbox is checked
  get ui() {
    return {
      showMailingAddress: () => { return this.form.get('hasMailingAddress').value === true; },
      showFamilyMembers: () => { return this.familyMembers.length > 0; },
    };
  }

  ngOnInit() {
    this.getInitialState()
      .pipe(first())
      .subscribe(registration => {
        this.initForm(registration);
        this.handleFormChanges();
      });
  }

  getInitialState() {
    return this.store.select(state => state.registration);
  }

  initForm(state: Registration) {
    this.registration = state;

    this.form = this.fb.group({
      isRestricted: this.fb.control(state.isRestricted),

      familyRepresentative: this.fb.group({
        firstName: [state.familyRepresentative.firstName, Validators.required],
        lastName: [state.familyRepresentative.lastName, Validators.required],
        nickname: [state.familyRepresentative.nickname],
        initials: [state.familyRepresentative.initials],
        gender: [state.familyRepresentative.gender],
        dob: [state.familyRepresentative.dob],
      }),

      isRegisteringFamilyMembers: this.fb.control(state.isRegisteringFamilyMembers),
      familyMembers: this.fb.array(state.familyMembers),

      phoneNumber: [state.familyRepresentative.profile.phoneNumber],
      phoneNumberAlt: [state.familyRepresentative.profile.phoneNumberAlt],
      email: [state.familyRepresentative.profile.email],

      primaryResidence: this.fb.group({
        addressLine1: [state.familyRepresentative.profile.primaryResidence.addressLine1],
        community: [state.familyRepresentative.profile.primaryResidence.community],
        postalCode: [state.familyRepresentative.profile.primaryResidence.postalCode],
        province: [state.familyRepresentative.profile.primaryResidence.province],
        country: [state.familyRepresentative.profile.primaryResidence.country],
      }),

      hasMailingAddress: this.fb.control(null),
      mailingAddress: this.fb.group({
        addressLine1: [state.familyRepresentative.profile.mailingAddress.addressLine1],
        community: [state.familyRepresentative.profile.mailingAddress.community],
        postalCode: [state.familyRepresentative.profile.mailingAddress.postalCode],
        province: [state.familyRepresentative.profile.mailingAddress.province],
        country: [state.familyRepresentative.profile.mailingAddress.country],
      }),
    });
  }

  handleFormChanges() {
    // TODO: Register any value change listeners here...
    this.isRegisteringFamilyMembers.valueChanges.subscribe((value: number) => {
      if (value === 1) {
        this.addFamilyMember();
      } else {
        this.clearFamilyMembers();
      }
    });
  }

  onSave() {
    const form = this.form.value;
    const state = this.registration;

    const newState: Registration = {
      ...state,
      ...{
        isRestricted: form.isRestricted,
        isRegisteringFamilyMembers: form.isRegisteringFamilyMembers,
        familyMembers: [...form.familyMembers],
        familyRepresentative: {
          firstName: form.familyRepresentative.firstName,
          lastName: form.familyRepresentative.lastName,
          nickname: form.familyRepresentative.nickname,
          initials: form.familyRepresentative.initials,
          gender: form.familyRepresentative.gender,
          dob: form.familyRepresentative.dob,
          profile: {
            phoneNumber: form.phoneNumber,
            phoneNumberAlt: form.phoneNumberAlt,
            email: form.email,
            primaryResidence: { ...form.primaryResidence },
            mailingAddress: form.hasMailingAddress ? { ...form.mailingAddress } : undefined,
          },
          isEvacuee: true,
          isVolunteer: false,
          isFamilyMember: false,
        },
      }
    };

    this.store.dispatch(new UpdateRegistration(newState));
  }

  addFamilyMember() {
    this.familyMembers.push(this.fb.group({
      relationshipToEvacuee: [''],
      sameLastNameAsEvacuee: [true],
      firstName: [''],
      lastName: [''],
      initials: [''],
      gender: [undefined],
      dob: [undefined],
    }));
  }

  clearFamilyMembers() {
    this.clear(this.familyMembers);
  }

  // TODO: Refactor into utils method
  private clear(formArray: FormArray) {
    while (formArray.length !== 0) {
      formArray.removeAt(0);
    }
  }

  next() {
    this.onSave();
    this.router.navigate(['../step-2'], { relativeTo: this.route });
  }
}
