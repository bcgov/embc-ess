import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogInEnvironmentComponent } from './log-in-environment.component';

describe('LogInEnvironmentComponent', () => {
  let component: LogInEnvironmentComponent;
  let fixture: ComponentFixture<LogInEnvironmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LogInEnvironmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogInEnvironmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
