import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SideboxAddOrganizationComponent } from './sidebox-add-organization.component';

describe('SideboxAddOrganizationComponent', () => {
  let component: SideboxAddOrganizationComponent;
  let fixture: ComponentFixture<SideboxAddOrganizationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SideboxAddOrganizationComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SideboxAddOrganizationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
