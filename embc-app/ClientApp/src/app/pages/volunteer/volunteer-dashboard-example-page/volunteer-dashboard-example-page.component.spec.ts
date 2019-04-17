import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerDashboardExamplePageComponent } from './volunteer-dashboard-example-page.component';

describe('VolunteerDashboardExamplePageComponent', () => {
  let component: VolunteerDashboardExamplePageComponent;
  let fixture: ComponentFixture<VolunteerDashboardExamplePageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [VolunteerDashboardExamplePageComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerDashboardExamplePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
