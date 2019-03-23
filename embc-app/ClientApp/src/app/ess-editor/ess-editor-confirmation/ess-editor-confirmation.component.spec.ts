import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EssEditorConfirmationComponent } from './ess-editor-confirmation.component';

describe('EssEditorConfirmationComponent', () => {
  let component: EssEditorConfirmationComponent;
  let fixture: ComponentFixture<EssEditorConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EssEditorConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EssEditorConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
