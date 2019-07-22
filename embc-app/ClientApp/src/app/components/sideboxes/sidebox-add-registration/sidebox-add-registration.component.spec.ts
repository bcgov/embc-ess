import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SideboxAddRegistrationComponent } from './sidebox-add-registration.component';

describe('SideboxAddRegistrationComponent', () => {
  let component: SideboxAddRegistrationComponent;
  let fixture: ComponentFixture<SideboxAddRegistrationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SideboxAddRegistrationComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SideboxAddRegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
