import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LodgingReferralComponent } from './lodging-referral.component';

describe('LodgingReferralComponent', () => {
  let component: LodgingReferralComponent;
  let fixture: ComponentFixture<LodgingReferralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LodgingReferralComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LodgingReferralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
