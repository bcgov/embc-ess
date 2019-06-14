import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TopTabBarComponent } from './top-tab-bar.component';

describe('TopTabBarComponent', () => {
  let component: TopTabBarComponent;
  let fixture: ComponentFixture<TopTabBarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TopTabBarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopTabBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
