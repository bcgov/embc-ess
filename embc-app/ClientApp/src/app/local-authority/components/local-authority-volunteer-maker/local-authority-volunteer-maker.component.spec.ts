import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LocalAuthorityVolunteerMakerComponent } from './local-authority-volunteer-maker.component';

describe('LocalAuthorityVolunteerMakerComponent', () => {
  let component: LocalAuthorityVolunteerMakerComponent;
  let fixture: ComponentFixture<LocalAuthorityVolunteerMakerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LocalAuthorityVolunteerMakerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LocalAuthorityVolunteerMakerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
