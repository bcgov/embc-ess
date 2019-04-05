import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskNumberListComponent } from './task-number-list.component';

describe('TaskNumbersListComponent', () => {
  let component: TaskNumberListComponent;
  let fixture: ComponentFixture<TaskNumberListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TaskNumberListComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskNumberListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
