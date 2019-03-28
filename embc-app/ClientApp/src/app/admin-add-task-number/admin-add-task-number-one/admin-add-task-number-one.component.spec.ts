import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAddTaskNumberOneComponent } from './admin-add-task-number-one.component';

describe('AdminAddTaskNumberOneComponent', () => {
  let component: AdminAddTaskNumberOneComponent;
  let fixture: ComponentFixture<AdminAddTaskNumberOneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminAddTaskNumberOneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAddTaskNumberOneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
