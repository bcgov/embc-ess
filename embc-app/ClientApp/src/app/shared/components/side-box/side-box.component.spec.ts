import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBoxComponent } from './side-box.component';

describe('SideBoxComponent', () => {
  let component: SideBoxComponent;
  let fixture: ComponentFixture<SideBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SideBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SideBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
