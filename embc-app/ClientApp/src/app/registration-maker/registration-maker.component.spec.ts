import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationMakerComponent } from './registration-maker.component';

describe('RegistrationMakerComponent', () => {
  let component: RegistrationMakerComponent;
  let fixture: ComponentFixture<RegistrationMakerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RegistrationMakerComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrationMakerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
