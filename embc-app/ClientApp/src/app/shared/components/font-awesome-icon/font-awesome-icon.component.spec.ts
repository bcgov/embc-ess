/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FontAwesomeIconComponent } from './font-awesome-icon.component';

describe('FontAwesomeIconComponent', () => {
  let component: FontAwesomeIconComponent;
  let fixture: ComponentFixture<FontAwesomeIconComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FontAwesomeIconComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FontAwesomeIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
