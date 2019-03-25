import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvacueeSummaryComponent } from './evacuee-summary.component';

describe('EvacueeSummaryComponent', () => {
  let component: EvacueeSummaryComponent;
  let fixture: ComponentFixture<EvacueeSummaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvacueeSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvacueeSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
