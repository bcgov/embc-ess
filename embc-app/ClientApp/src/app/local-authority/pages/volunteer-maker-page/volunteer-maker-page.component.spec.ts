import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerMakerPageComponent } from './volunteer-maker-page.component';

describe('VolunteerMakerPageComponent', () => {
  let component: VolunteerMakerPageComponent;
  let fixture: ComponentFixture<VolunteerMakerPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerMakerPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerMakerPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
