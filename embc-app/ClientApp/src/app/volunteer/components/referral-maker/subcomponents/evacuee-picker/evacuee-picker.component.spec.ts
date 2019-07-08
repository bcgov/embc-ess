import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvacueePickerComponent } from './evacuee-picker.component';

describe('EvacueeListComponent', () => {
  let component: EvacueePickerComponent;
  let fixture: ComponentFixture<EvacueePickerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EvacueePickerComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvacueePickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
