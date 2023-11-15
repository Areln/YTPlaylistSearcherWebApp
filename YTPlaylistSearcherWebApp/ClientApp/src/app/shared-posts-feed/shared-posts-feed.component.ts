import { Component, ViewChild } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { MatTable } from '@angular/material/table';
import * as signalR from '@microsoft/signalr';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { SharedPostDTO } from '../DTOs/SharedPostDTO';
import { PlaylistService } from '../services/PlaylistService';
import { SignalrService } from '../services/signalr.service';

@Component({
  selector: 'app-shared-posts-feed',
  templateUrl: './shared-posts-feed.component.html',
  styleUrls: ['./shared-posts-feed.component.css']
})
export class SharedPostsFeedComponent {

  isLoading: boolean = false;
  errorMessage: string | null | undefined;
  sharedPosts!: SharedPostDTO[];
  displayedColumns: string[] = ['result'];
  connection!: signalR.HubConnection;

  @ViewChild(MatTable) table!: MatTable<any>;

  // infinite scroll
  //https://stackoverflow.com/questions/57142883/how-can-i-use-infinite-scroll-in-combination-with-mat-table-and-service-in-angul
  constructor(private _playlistService: PlaylistService, private _postHub: SignalrService) {
    this.isLoading = true;
    this._playlistService.GetSharedPosts().subscribe(result => {
      this.sharedPosts = result;
      this.isLoading = false;
    }, error => {
      console.log(error);
      this.isLoading = false;
    });

    _postHub.newMessageEvent.subscribe(data => {
      console.log('New Message Event:' + data);
    });

    _postHub.newPost.subscribe(post => {
      this.sharedPosts.unshift(post);
      this.table.renderRows();
    });

    _postHub.deletePost.subscribe(id => {
      this.sharedPosts = this.sharedPosts.filter(x => x.postID != id)
      this.table.renderRows();
    });

  }

  DeletePost(post: SharedPostDTO) {
    this._playlistService.DeletePost(post.postID).subscribe(result => {
      if (result == true) {
        this.sharedPosts = this.sharedPosts.filter(x => x.postID != post.postID)
        this.table.renderRows();
      }
    });
  }
}
