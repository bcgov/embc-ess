import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CommunitiesSelectComponent } from './communities-select.component';

describe('CommunitiesSelectComponent', () => {
  let component: CommunitiesSelectComponent;
  let fixture: ComponentFixture<CommunitiesSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommunitiesSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunitiesSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
