import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvincialAdminRegistrationsPageComponent } from './provincial-admin-registrations-page.component';

describe('ProvincialAdminRegistrationsPageComponent', () => {
  let component: ProvincialAdminRegistrationsPageComponent;
  let fixture: ComponentFixture<ProvincialAdminRegistrationsPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProvincialAdminRegistrationsPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProvincialAdminRegistrationsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
