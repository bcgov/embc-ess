import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAddTaskNumberConfirmationComponent } from './admin-add-task-number-confirmation.component';

describe('AdminAddTaskNumberConfirmationComponent', () => {
  let component: AdminAddTaskNumberConfirmationComponent;
  let fixture: ComponentFixture<AdminAddTaskNumberConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminAddTaskNumberConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAddTaskNumberConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
