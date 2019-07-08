import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FoodReferralComponent } from './food-referral.component';

describe('FoodReferralComponent', () => {
  let component: FoodReferralComponent;
  let fixture: ComponentFixture<FoodReferralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FoodReferralComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FoodReferralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
