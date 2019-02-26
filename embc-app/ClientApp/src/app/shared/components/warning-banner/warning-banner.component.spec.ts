import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WarningBannerComponent } from './warning-banner.component';

describe('WarningBannerComponent', () => {
  let component: WarningBannerComponent;
  let fixture: ComponentFixture<WarningBannerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WarningBannerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WarningBannerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
