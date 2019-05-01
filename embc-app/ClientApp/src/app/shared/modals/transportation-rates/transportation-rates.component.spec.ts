import { ComponentFixture, async, TestBed } from '@angular/core/testing';
import { TransportationRatesComponent } from './transportation-rates.component';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

describe('TransportationRatesComponent', () => {
  let component: TransportationRatesComponent;
  let fixture: ComponentFixture<TransportationRatesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        TransportationRatesComponent
      ],
      imports: [
        FormsModule
      ],
      providers: [
        NgbActiveModal
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransportationRatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
