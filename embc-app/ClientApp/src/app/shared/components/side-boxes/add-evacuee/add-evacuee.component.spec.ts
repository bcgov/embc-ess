import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEvacueeComponent } from './add-evacuee.component';

describe('AddEvacueeComponent', () => {
  let component: AddEvacueeComponent;
  let fixture: ComponentFixture<AddEvacueeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEvacueeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEvacueeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
