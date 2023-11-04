import { Component, ViewChild } from '@angular/core';
import { MatTable } from '@angular/material/table';
import * as signalR from '@microsoft/signalr';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
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
  displayedColumns: string[] = ['result'];
  connection!: signalR.HubConnection;

  @ViewChild(MatTable) table!: MatTable<any>;

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

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7298/posts")
      .configureLogging(LogLevel.Debug)
      .build();

    this.connection.start().catch((reason) => {
      console.log(reason);
    });

    this.connection.on("ReceiveMessage", (data: string) => {
      console.log(data);
    });

    this.connection.on("NewPost", (post: string) => {
      var temp = JSON.parse(post);
      this.sharedPosts.unshift(temp);
      this.table.renderRows();
    });
  }
}
