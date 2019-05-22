import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvacueeListComponent } from './evacuee-list.component';

describe('EvacueeListComponent', () => {
  let component: EvacueeListComponent;
  let fixture: ComponentFixture<EvacueeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EvacueeListComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvacueeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
