import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizationMakerPageComponent } from './organization-maker-page.component';

describe('OrganizationMakerPageComponent', () => {
  let component: OrganizationMakerPageComponent;
  let fixture: ComponentFixture<OrganizationMakerPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrganizationMakerPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrganizationMakerPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
