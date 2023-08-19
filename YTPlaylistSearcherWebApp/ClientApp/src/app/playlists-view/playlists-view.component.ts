import { Component } from '@angular/core';
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

  constructor(private _playlistService: PlaylistService) {
    this.isLoading = true;

    _playlistService.GetPlaylists().subscribe(result => {
      this.isLoading = false;
      this.playlists = result;
    }, error => {
      this.isLoading = false;
      this.errorMessage = error;
    });
  }


}
