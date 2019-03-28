import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTaskNumbersComponent } from './admin-task-numbers.component';

describe('AdminTaskNumbersComponent', () => {
  let component: AdminTaskNumbersComponent;
  let fixture: ComponentFixture<AdminTaskNumbersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminTaskNumbersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTaskNumbersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
