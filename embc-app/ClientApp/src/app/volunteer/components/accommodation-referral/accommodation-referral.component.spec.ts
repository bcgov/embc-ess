import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccommodationReferralComponent } from './accommodation-referral.component';

describe('AccommodationReferralComponent', () => {
  let component: AccommodationReferralComponent;
  let fixture: ComponentFixture<AccommodationReferralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AccommodationReferralComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccommodationReferralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
