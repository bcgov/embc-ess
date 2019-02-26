import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfRegistrationTwoComponent } from './self-registration-two.component';

describe('SelfRegistrationTwoComponent', () => {
  let component: SelfRegistrationTwoComponent;
  let fixture: ComponentFixture<SelfRegistrationTwoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfRegistrationTwoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfRegistrationTwoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
