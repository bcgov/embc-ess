import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerLoginComponent } from './volunteer-login.component';

describe('VolunteerLoginComponent', () => {
  let component: VolunteerLoginComponent;
  let fixture: ComponentFixture<VolunteerLoginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerLoginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
