import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTaskNumberComponent } from './add-task-number.component';

describe('AddTaskNumberComponent', () => {
  let component: AddTaskNumberComponent;
  let fixture: ComponentFixture<AddTaskNumberComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddTaskNumberComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddTaskNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
