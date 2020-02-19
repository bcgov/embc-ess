import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ElectronicAccessAgreementComponent } from './electronic-access-agreement.component';

describe('ElectronicAccessAgreementComponent', () => {
  let component: ElectronicAccessAgreementComponent;
  let fixture: ComponentFixture<ElectronicAccessAgreementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ElectronicAccessAgreementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ElectronicAccessAgreementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
