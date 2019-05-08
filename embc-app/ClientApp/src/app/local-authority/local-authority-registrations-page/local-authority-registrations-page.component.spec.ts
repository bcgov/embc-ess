import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LocalAuthorityRegistrationsPageComponent } from './local-authority-registrations-page.component';

describe('LocalAuthorityRegistrationsPageComponent', () => {
  let component: LocalAuthorityRegistrationsPageComponent;
  let fixture: ComponentFixture<LocalAuthorityRegistrationsPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LocalAuthorityRegistrationsPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LocalAuthorityRegistrationsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
