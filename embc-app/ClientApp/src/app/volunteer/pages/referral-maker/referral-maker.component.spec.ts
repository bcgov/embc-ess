import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferralMakerComponent } from './referral-maker.component';

describe('ReferralMakerComponent', () => {
  let component: ReferralMakerComponent;
  let fixture: ComponentFixture<ReferralMakerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ReferralMakerComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReferralMakerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
