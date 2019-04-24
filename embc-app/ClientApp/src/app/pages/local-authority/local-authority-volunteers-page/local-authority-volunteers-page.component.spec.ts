import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LocalAuthorityVolunteersPageComponent } from './local-authority-volunteers-page.component';

describe('LocalAuthorityVolunteersPageComponent', () => {
  let component: LocalAuthorityVolunteersPageComponent;
  let fixture: ComponentFixture<LocalAuthorityVolunteersPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LocalAuthorityVolunteersPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LocalAuthorityVolunteersPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
