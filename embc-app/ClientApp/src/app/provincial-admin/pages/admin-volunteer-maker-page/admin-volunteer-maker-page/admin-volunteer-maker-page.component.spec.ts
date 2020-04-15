import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminVolunteerMakerPageComponent } from './admin-volunteer-maker-page.component';

describe('AdminVolunteerMakerPageComponent', () => {
  let component: AdminVolunteerMakerPageComponent;
  let fixture: ComponentFixture<AdminVolunteerMakerPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminVolunteerMakerPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminVolunteerMakerPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
