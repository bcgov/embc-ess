import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferralTableComponent } from './referral-table.component';

describe('ReferralTableComponent', () => {
  let component: ReferralTableComponent;
  let fixture: ComponentFixture<ReferralTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ReferralTableComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReferralTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
