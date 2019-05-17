import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationListComponent } from './registration-list.component';

describe('RegistrationListComponent', () => {
  let component: RegistrationListComponent;
  let fixture: ComponentFixture<RegistrationListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RegistrationListComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrationListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
