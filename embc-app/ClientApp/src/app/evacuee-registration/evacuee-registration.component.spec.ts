import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvacueeRegistrationComponent } from './evacuee-registration.component';

describe('EvacueeRegistrationComponent', () => {
  let component: EvacueeRegistrationComponent;
  let fixture: ComponentFixture<EvacueeRegistrationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvacueeRegistrationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvacueeRegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
