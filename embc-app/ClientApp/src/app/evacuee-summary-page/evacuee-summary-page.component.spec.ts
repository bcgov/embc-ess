import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvacueeSummaryPageComponent } from './evacuee-summary-page.component';

describe('EvacueeSummaryPageComponent', () => {
  let component: EvacueeSummaryPageComponent;
  let fixture: ComponentFixture<EvacueeSummaryPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvacueeSummaryPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvacueeSummaryPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
