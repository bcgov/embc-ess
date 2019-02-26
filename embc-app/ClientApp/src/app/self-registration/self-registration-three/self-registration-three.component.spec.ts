import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfRegistrationThreeComponent } from './self-registration-three.component';

describe('SelfRegistrationThreeComponent', () => {
  let component: SelfRegistrationThreeComponent;
  let fixture: ComponentFixture<SelfRegistrationThreeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfRegistrationThreeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfRegistrationThreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
