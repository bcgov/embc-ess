import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TransportationReferralComponent } from './transportation-referral.component';

describe('TransportationReferralComponent', () => {
  let component: TransportationReferralComponent;
  let fixture: ComponentFixture<TransportationReferralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TransportationReferralComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransportationReferralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
