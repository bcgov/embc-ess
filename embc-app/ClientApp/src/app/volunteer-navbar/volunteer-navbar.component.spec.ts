import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerNavbarComponent } from './volunteer-navbar.component';

describe('VolunteerNavbarComponent', () => {
  let component: VolunteerNavbarComponent;
  let fixture: ComponentFixture<VolunteerNavbarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerNavbarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
