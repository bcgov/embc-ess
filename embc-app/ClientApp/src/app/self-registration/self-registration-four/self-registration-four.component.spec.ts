import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfRegistrationFourComponent } from './self-registration-four.component';

describe('SelfRegistrationFourComponent', () => {
  let component: SelfRegistrationFourComponent;
  let fixture: ComponentFixture<SelfRegistrationFourComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfRegistrationFourComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfRegistrationFourComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
