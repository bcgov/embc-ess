import { ComponentFixture, async, TestBed } from '@angular/core/testing';
import { IncidentalsRatesComponent } from './incidentals-rates.component';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

describe('IncidentalsRatesComponent', () => {
  let component: IncidentalsRatesComponent;
  let fixture: ComponentFixture<IncidentalsRatesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        IncidentalsRatesComponent
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
    fixture = TestBed.createComponent(IncidentalsRatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
