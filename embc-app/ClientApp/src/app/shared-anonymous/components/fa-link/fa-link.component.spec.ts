/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FontAwesomeLinkComponent } from './fa-link.component';

describe('FontAwesomeLinkComponent', () => {
  let component: FontAwesomeLinkComponent;
  let fixture: ComponentFixture<FontAwesomeLinkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FontAwesomeLinkComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FontAwesomeLinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
