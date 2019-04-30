import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidFromToComponent } from './valid-from-to.component';

describe('ValidFromToComponent', () => {
  let component: ValidFromToComponent;
  let fixture: ComponentFixture<ValidFromToComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ValidFromToComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidFromToComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
