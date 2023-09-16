import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CounterStrikeLineUpsSearchComponent } from './counter-strike-line-ups-search.component';

describe('CounterStrikeLineUpsSearchComponent', () => {
  let component: CounterStrikeLineUpsSearchComponent;
  let fixture: ComponentFixture<CounterStrikeLineUpsSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CounterStrikeLineUpsSearchComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CounterStrikeLineUpsSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
