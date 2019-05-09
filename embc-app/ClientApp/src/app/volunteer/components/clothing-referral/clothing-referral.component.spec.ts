import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClothingReferralComponent } from './clothing-referral.component';

describe('ClothingReferralComponent', () => {
  let component: ClothingReferralComponent;
  let fixture: ComponentFixture<ClothingReferralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ClothingReferralComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClothingReferralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
