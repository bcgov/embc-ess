import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerEvacueeResultsPageComponent } from './volunteer-evacuee-results-page.component';

describe('VolunteerEvacueeResultsPageComponent', () => {
  let component: VolunteerEvacueeResultsPageComponent;
  let fixture: ComponentFixture<VolunteerEvacueeResultsPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerEvacueeResultsPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerEvacueeResultsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
