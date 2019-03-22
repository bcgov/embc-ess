import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvacueeRegistrationConfirmationComponent } from './evacuee-registration-confirmation.component';

describe('EvacueeRegistrationConfirmationComponent', () => {
  let component: EvacueeRegistrationConfirmationComponent;
  let fixture: ComponentFixture<EvacueeRegistrationConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvacueeRegistrationConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvacueeRegistrationConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
