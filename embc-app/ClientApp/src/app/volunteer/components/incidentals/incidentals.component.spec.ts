import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentalsComponent } from './incidentals.component';

describe('IncidentalsComponent', () => {
  let component: IncidentalsComponent;
  let fixture: ComponentFixture<IncidentalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [IncidentalsComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
