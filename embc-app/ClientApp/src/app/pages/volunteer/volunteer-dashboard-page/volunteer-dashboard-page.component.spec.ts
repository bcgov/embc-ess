import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerDashboardPageComponent } from './volunteer-dashboard-page.component';

describe('VolunteerDashboardPageComponent', () => {
  let component: VolunteerDashboardPageComponent;
  let fixture: ComponentFixture<VolunteerDashboardPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerDashboardPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerDashboardPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
