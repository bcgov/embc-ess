import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerEditorComponent } from './volunteer-editor.component';

describe('VolunteerEditorComponent', () => {
  let component: VolunteerEditorComponent;
  let fixture: ComponentFixture<VolunteerEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [VolunteerEditorComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
