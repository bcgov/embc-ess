import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminVolunteerMakerComponent } from './admin-volunteer-maker.component';

describe('AdminVolunteerMakerComponent', () => {
  let component: AdminVolunteerMakerComponent;
  let fixture: ComponentFixture<AdminVolunteerMakerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminVolunteerMakerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminVolunteerMakerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
