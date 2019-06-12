import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerMakerComponent } from './volunteer-maker.component';

describe('VolunteerMakerComponent', () => {
  let component: VolunteerMakerComponent;
  let fixture: ComponentFixture<VolunteerMakerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerMakerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerMakerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
