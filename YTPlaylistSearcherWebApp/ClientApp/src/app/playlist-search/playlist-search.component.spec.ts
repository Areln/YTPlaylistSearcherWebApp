import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PlaylistSearchComponent } from './playlist-search.component';


describe('PlaylistSearchComponent', () => {
  let component: PlaylistSearchComponent;
  let fixture: ComponentFixture<PlaylistSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlaylistSearchComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlaylistSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
