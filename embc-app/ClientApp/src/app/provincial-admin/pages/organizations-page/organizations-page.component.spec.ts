import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizationsPageComponent } from './organizations-page.component';

describe('OrganizationsPageComponent', () => {
  let component: OrganizationsPageComponent;
  let fixture: ComponentFixture<OrganizationsPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [OrganizationsPageComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrganizationsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
