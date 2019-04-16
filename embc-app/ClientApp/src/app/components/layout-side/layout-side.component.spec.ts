import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutSideComponent } from './layout-side.component';

describe('LayoutSideComponent', () => {
  let component: LayoutSideComponent;
  let fixture: ComponentFixture<LayoutSideComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutSideComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutSideComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
