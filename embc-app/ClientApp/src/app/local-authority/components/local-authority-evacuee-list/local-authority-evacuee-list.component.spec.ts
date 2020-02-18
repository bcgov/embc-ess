import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LocalAuthorityEvacueeListComponent } from './local-authority-evacuee-list.component';

describe('LocalAuthorityEvacueeListComponent', () => {
  let component: LocalAuthorityEvacueeListComponent;
  let fixture: ComponentFixture<LocalAuthorityEvacueeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LocalAuthorityEvacueeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LocalAuthorityEvacueeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
