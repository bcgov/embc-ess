import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmbcAdminDashboardComponent } from './embc-admin-dashboard.component';

describe('EmbcAdminDashboardComponent', () => {
  let component: EmbcAdminDashboardComponent;
  let fixture: ComponentFixture<EmbcAdminDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmbcAdminDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmbcAdminDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
