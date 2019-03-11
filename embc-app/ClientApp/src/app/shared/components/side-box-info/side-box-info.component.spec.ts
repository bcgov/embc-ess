import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBoxInfoComponent } from './side-box-info.component';

describe('SideBoxInfoComponent', () => {
  let component: SideBoxInfoComponent;
  let fixture: ComponentFixture<SideBoxInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SideBoxInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SideBoxInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
