import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvincialAdminOrganizationsPageComponent } from './provincial-admin-organizations-page.component';

describe('ProvincialAdminOrganizationsPageComponent', () => {
  let component: ProvincialAdminOrganizationsPageComponent;
  let fixture: ComponentFixture<ProvincialAdminOrganizationsPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProvincialAdminOrganizationsPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProvincialAdminOrganizationsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
