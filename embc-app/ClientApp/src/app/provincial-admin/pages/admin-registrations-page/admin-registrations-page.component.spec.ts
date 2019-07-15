import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRegistrationsPageComponent } from './admin-registrations-page.component';

describe('RegistrationsPageComponent', () => {
  let component: AdminRegistrationsPageComponent;
  let fixture: ComponentFixture<AdminRegistrationsPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AdminRegistrationsPageComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminRegistrationsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
