import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UsefulInformationPageComponent } from './useful-information-page.component';

describe('UsefulInformationPageComponent', () => {
  let component: UsefulInformationPageComponent;
  let fixture: ComponentFixture<UsefulInformationPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UsefulInformationPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsefulInformationPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
