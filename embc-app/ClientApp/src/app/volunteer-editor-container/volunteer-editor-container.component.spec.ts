import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerEditorContainerComponent } from './volunteer-editor-container.component';

describe('VolunteerEditorContainerComponent', () => {
  let component: VolunteerEditorContainerComponent;
  let fixture: ComponentFixture<VolunteerEditorContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VolunteerEditorContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerEditorContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
