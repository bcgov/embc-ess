import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SideboxAddTaskNumberComponent } from './sidebox-add-task-number.component';

describe('AddTaskNumberComponent', () => {
  let component: SideboxAddTaskNumberComponent;
  let fixture: ComponentFixture<SideboxAddTaskNumberComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SideboxAddTaskNumberComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SideboxAddTaskNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
