import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskNumberMakerComponent } from './task-number-maker.component';

describe('TaskNumberMakerComponent', () => {
  let component: TaskNumberMakerComponent;
  let fixture: ComponentFixture<TaskNumberMakerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskNumberMakerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskNumberMakerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
