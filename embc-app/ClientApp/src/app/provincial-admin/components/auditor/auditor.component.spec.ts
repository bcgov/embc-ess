import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditorComponent } from './auditor.component';

describe('AuditorComponent', () => {
  let component: AuditorComponent;
  let fixture: ComponentFixture<AuditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
