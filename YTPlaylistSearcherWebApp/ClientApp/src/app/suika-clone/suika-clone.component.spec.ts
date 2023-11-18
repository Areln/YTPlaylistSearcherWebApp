import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuikaCloneComponent } from './suika-clone.component';

describe('SuikaCloneComponent', () => {
  let component: SuikaCloneComponent;
  let fixture: ComponentFixture<SuikaCloneComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SuikaCloneComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuikaCloneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
