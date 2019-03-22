import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TesterPageComponent } from './tester-page.component';

describe('TesterPageComponent', () => {
  let component: TesterPageComponent;
  let fixture: ComponentFixture<TesterPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TesterPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TesterPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
