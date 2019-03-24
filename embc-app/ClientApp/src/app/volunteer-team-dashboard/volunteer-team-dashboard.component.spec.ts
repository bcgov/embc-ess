import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerTeamDashboardComponent } from './volunteer-team-dashboard.component';

describe('VolunteerTeamDashboardComponent', () => {
  let component: VolunteerTeamDashboardComponent;
  let fixture: ComponentFixture<VolunteerTeamDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerTeamDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerTeamDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
