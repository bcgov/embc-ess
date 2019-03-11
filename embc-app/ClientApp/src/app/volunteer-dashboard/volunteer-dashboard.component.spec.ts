import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerDashboardComponent } from './volunteer-dashboard.component';

describe('VolunteerDashboardComponent', () => {
  let component: VolunteerDashboardComponent;
  let fixture: ComponentFixture<VolunteerDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
