import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvacueeSummaryContainerComponent } from './evacuee-summary-container.component';

describe('EvacueeSummaryContainerComponent', () => {
  let component: EvacueeSummaryContainerComponent;
  let fixture: ComponentFixture<EvacueeSummaryContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvacueeSummaryContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvacueeSummaryContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
