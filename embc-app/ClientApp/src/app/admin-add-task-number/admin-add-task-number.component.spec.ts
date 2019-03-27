import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAddTaskNumberComponent } from './admin-add-task-number.component';

describe('AdminAddTaskNumberComponent', () => {
  let component: AdminAddTaskNumberComponent;
  let fixture: ComponentFixture<AdminAddTaskNumberComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminAddTaskNumberComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAddTaskNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
