import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SideboxCollectionNoticeComponent } from './sidebox-collection-notice.component';

describe('SideboxCollectionNoticeComponent', () => {
  let component: SideboxCollectionNoticeComponent;
  let fixture: ComponentFixture<SideboxCollectionNoticeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SideboxCollectionNoticeComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SideboxCollectionNoticeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
