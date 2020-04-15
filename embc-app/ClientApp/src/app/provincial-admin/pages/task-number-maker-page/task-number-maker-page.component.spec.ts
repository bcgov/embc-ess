import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskNumberMakerPageComponent } from './task-number-maker-page.component';

describe('TaskNumberMakerPageComponent', () => {
  let component: TaskNumberMakerPageComponent;
  let fixture: ComponentFixture<TaskNumberMakerPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskNumberMakerPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskNumberMakerPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
