import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizationMakerComponent } from './organization-maker.component';

describe('OrganizationMakerComponent', () => {
  let component: OrganizationMakerComponent;
  let fixture: ComponentFixture<OrganizationMakerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrganizationMakerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrganizationMakerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
