import { ComponentFixture, async, TestBed } from '@angular/core/testing';
import { ClothingRatesComponent } from './clothing-rates.component';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

describe('ClothingRatesComponent', () => {
  let component: ClothingRatesComponent;
  let fixture: ComponentFixture<ClothingRatesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        ClothingRatesComponent
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
    fixture = TestBed.createComponent(ClothingRatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
