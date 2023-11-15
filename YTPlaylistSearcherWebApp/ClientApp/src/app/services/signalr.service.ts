import { EventEmitter, Inject, Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { SharedPostDTO } from '../DTOs/SharedPostDTO';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  connection!: signalR.HubConnection;

  public newMessageEvent = new EventEmitter<string>();
  public newPost = new EventEmitter<SharedPostDTO>();
  public deletePost = new EventEmitter<number>();

  constructor(@Inject('BASE_URL') private baseUrl: string) {

    this.connection = new HubConnectionBuilder()
      .withUrl(this.baseUrl + "posts")
      .configureLogging(LogLevel.Debug)
      .build();

    this.connection.start().catch((reason) => {
      console.log(reason);
    });

    this.connection.on("ReceiveMessage", (data: string) => {
      this.newMessageEvent.next(data);
    });

    this.connection.on("NewPost", (post: string) => {
      var temp = JSON.parse(post);
      this.newPost.next(temp);
    });

    this.connection.on("DeletePost", (id: number) => {
      this.deletePost.next(id);
    });

  }
}
