import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerEditorConfirmationComponent } from './volunteer-editor-confirmation.component';

describe('VolunteerEditorConfirmationComponent', () => {
  let component: VolunteerEditorConfirmationComponent;
  let fixture: ComponentFixture<VolunteerEditorConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [VolunteerEditorConfirmationComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerEditorConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
