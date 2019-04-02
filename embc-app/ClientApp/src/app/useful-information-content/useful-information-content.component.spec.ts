import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UsefulInformationContentComponent } from './useful-information-content.component';

describe('UsefulInformationContentComponent', () => {
  let component: UsefulInformationContentComponent;
  let fixture: ComponentFixture<UsefulInformationContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UsefulInformationContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsefulInformationContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
