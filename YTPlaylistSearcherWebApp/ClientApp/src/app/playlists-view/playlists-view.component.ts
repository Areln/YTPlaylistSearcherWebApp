import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { PlaylistDTO } from '../DTOs/PlaylistDTO';
import { PlaylistService } from '../services/PlaylistService';

@Component({
  selector: 'app-playlists-view',
  templateUrl: './playlists-view.component.html',
  styleUrls: ['./playlists-view.component.css']
})
export class PlaylistsViewComponent {

  isLoading: boolean = false;
  errorMessage: string | null | undefined;
  playlists!: PlaylistDTO[];

  constructor(private _playlistService: PlaylistService, private router: Router, @Inject('BASE_URL') private baseUrl: string) {
    this.isLoading = true;

    _playlistService.GetPlaylists().subscribe(result => {
      this.isLoading = false;
      this.playlists = result;
    }, error => {
      this.isLoading = false;
      this.errorMessage = error;
    });
  }

  playlistClick(playlistID: string) {
    this.router.navigate(['/search/' + playlistID]);
  }

}
