import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminOrganizationsComponent } from './admin-organizations.component';

describe('AdminOrganizationsComponent', () => {
  let component: AdminOrganizationsComponent;
  let fixture: ComponentFixture<AdminOrganizationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminOrganizationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminOrganizationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
