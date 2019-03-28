import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminEvacueesComponent } from './admin-evacuees.component';

describe('AdminEvacueesComponent', () => {
  let component: AdminEvacueesComponent;
  let fixture: ComponentFixture<AdminEvacueesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminEvacueesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminEvacueesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
