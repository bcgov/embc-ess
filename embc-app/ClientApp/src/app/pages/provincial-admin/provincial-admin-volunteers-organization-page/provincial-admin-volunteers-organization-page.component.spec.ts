import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvincialAdminVolunteersOrganizationPageComponent } from './provincial-admin-volunteers-organization-page.component';

describe('ProvincialAdminVolunteersOrganizationPageComponent', () => {
  let component: ProvincialAdminVolunteersOrganizationPageComponent;
  let fixture: ComponentFixture<ProvincialAdminVolunteersOrganizationPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProvincialAdminVolunteersOrganizationPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProvincialAdminVolunteersOrganizationPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
