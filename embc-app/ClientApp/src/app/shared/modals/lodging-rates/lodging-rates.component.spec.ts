import { ComponentFixture, async, TestBed } from '@angular/core/testing';
import { LodgingRatesComponent } from './lodging-rates.component';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

describe('LodgingRatesComponent', () => {
  let component: LodgingRatesComponent;
  let fixture: ComponentFixture<LodgingRatesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        LodgingRatesComponent
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
    fixture = TestBed.createComponent(LodgingRatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
