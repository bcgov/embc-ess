import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvacueeRegistrationOneComponent } from './evacuee-registration-one.component';

describe('EvacueeRegistrationOneComponent', () => {
  let component: EvacueeRegistrationOneComponent;
  let fixture: ComponentFixture<EvacueeRegistrationOneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvacueeRegistrationOneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvacueeRegistrationOneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
