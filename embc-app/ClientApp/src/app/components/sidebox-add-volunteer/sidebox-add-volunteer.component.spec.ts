import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SideboxAddVolunteerComponent } from './sidebox-add-volunteer.component';

describe('SideboxAddVolunteerComponent', () => {
  let component: SideboxAddVolunteerComponent;
  let fixture: ComponentFixture<SideboxAddVolunteerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SideboxAddVolunteerComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SideboxAddVolunteerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
