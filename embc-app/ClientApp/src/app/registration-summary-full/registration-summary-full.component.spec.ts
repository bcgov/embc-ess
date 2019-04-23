import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationSummaryFullComponent } from './registration-summary-full.component';

describe('RegistrationSummaryFullComponent', () => {
  let component: RegistrationSummaryFullComponent;
  let fixture: ComponentFixture<RegistrationSummaryFullComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RegistrationSummaryFullComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrationSummaryFullComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
