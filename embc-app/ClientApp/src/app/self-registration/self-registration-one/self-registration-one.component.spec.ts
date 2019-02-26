import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfRegistrationOneComponent } from './self-registration-one.component';

describe('SelfRegistrationOneComponent', () => {
  let component: SelfRegistrationOneComponent;
  let fixture: ComponentFixture<SelfRegistrationOneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfRegistrationOneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfRegistrationOneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
