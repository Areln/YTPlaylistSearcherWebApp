import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedPostsFeedComponent } from './shared-posts-feed.component';

describe('SharedPostsFeedComponent', () => {
  let component: SharedPostsFeedComponent;
  let fixture: ComponentFixture<SharedPostsFeedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SharedPostsFeedComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SharedPostsFeedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
