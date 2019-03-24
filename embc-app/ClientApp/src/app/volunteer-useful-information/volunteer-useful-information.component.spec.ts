import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerUsefulInformationComponent } from './volunteer-useful-information.component';

describe('VolunteerUsefulInformationComponent', () => {
  let component: VolunteerUsefulInformationComponent;
  let fixture: ComponentFixture<VolunteerUsefulInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerUsefulInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerUsefulInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
