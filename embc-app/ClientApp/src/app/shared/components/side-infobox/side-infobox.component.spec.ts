import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SideInfoboxComponent } from './side-infobox.component';

describe('SideInfoboxComponent', () => {
  let component: SideInfoboxComponent;
  let fixture: ComponentFixture<SideInfoboxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SideInfoboxComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SideInfoboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
