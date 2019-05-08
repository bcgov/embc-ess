import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerRegistrationsPageComponent } from './volunteer-registrations-page.component';

describe('VolunteerRegistrationsPageComponent', () => {
  let component: VolunteerRegistrationsPageComponent;
  let fixture: ComponentFixture<VolunteerRegistrationsPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerRegistrationsPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerRegistrationsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
