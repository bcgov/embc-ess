import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvincialAdminTaskNumbersPageComponent } from './provincial-admin-task-numbers-page.component';

describe('ProvincialAdminTaskNumbersPageComponent', () => {
  let component: ProvincialAdminTaskNumbersPageComponent;
  let fixture: ComponentFixture<ProvincialAdminTaskNumbersPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProvincialAdminTaskNumbersPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProvincialAdminTaskNumbersPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
