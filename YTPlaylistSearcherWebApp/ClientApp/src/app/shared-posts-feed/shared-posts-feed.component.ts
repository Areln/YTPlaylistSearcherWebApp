import { Component } from '@angular/core';
import { SharedPostDTO } from '../DTOs/SharedPostDTO';
import { PlaylistService } from '../services/PlaylistService';

@Component({
  selector: 'app-shared-posts-feed',
  templateUrl: './shared-posts-feed.component.html',
  styleUrls: ['./shared-posts-feed.component.css']
})
export class SharedPostsFeedComponent {

  isLoading: boolean = false;
  errorMessage: string | null | undefined;
  sharedPosts!: SharedPostDTO[];
  displayedColumns: string[] = [ 'result'];

  // infinite scroll
  //https://stackoverflow.com/questions/57142883/how-can-i-use-infinite-scroll-in-combination-with-mat-table-and-service-in-angul
  constructor(private _playlistService: PlaylistService) {
    this.isLoading = true;
    this._playlistService.GetSharedPosts().subscribe(result => {
      this.sharedPosts = result;
      this.isLoading = false;
    }, error => {
      console.log(error);
      this.isLoading = false;
    });
  }
}
