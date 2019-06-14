import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfRegistrationStepperComponent } from './self-registration-stepper.component';

describe('SelfRegistrationStepperComponent', () => {
  let component: SelfRegistrationStepperComponent;
  let fixture: ComponentFixture<SelfRegistrationStepperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfRegistrationStepperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfRegistrationStepperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
