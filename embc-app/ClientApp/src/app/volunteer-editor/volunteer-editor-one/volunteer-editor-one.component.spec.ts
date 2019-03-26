import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VolunteerEditorOneComponent } from './volunteer-editor-one.component';

describe('VolunteerEditorOneComponent', () => {
  let component: VolunteerEditorOneComponent;
  let fixture: ComponentFixture<VolunteerEditorOneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [VolunteerEditorOneComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VolunteerEditorOneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
