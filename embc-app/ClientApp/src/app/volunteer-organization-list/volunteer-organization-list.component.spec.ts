import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerOrganizationListComponent } from './volunteer-organization-list.component';

describe('VolunteerOrganizationListComponent', () => {
  let component: VolunteerOrganizationListComponent;
  let fixture: ComponentFixture<VolunteerOrganizationListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerOrganizationListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerOrganizationListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
