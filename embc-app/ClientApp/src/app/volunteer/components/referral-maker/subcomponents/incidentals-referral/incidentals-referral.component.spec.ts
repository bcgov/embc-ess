import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentalsReferralComponent } from './incidentals-referral.component';

describe('IncidentalsReferralComponent', () => {
  let component: IncidentalsReferralComponent;
  let fixture: ComponentFixture<IncidentalsReferralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [IncidentalsReferralComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentalsReferralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
