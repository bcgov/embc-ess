import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EssEditorComponent } from './ess-editor.component';

describe('EssEditorComponent', () => {
  let component: EssEditorComponent;
  let fixture: ComponentFixture<EssEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EssEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EssEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
