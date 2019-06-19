import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FullRegistrationSummaryBlockComponent } from './full-registration-summary-block.component';

describe('FullRegistrationSummaryBlockComponent', () => {
  let component: FullRegistrationSummaryBlockComponent;
  let fixture: ComponentFixture<FullRegistrationSummaryBlockComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FullRegistrationSummaryBlockComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FullRegistrationSummaryBlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
