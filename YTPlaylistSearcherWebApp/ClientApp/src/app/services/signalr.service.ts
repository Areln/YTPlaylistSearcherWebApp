import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  //private hubConnection: signalR.HubConnection
  //public startConnection = () => {
  //  this.hubConnection = new signalR.HubConnectionBuilder()
  //    .withUrl('https://localhost:5001/chart')
  //    .build();
  //  this.hubConnection
  //    .start()
  //    .then(() => console.log('Connection started'))
  //    .catch(err => console.log('Error while starting connection: ' + err))
  //}

  //public addTransferChartDataListener = () => {
  //  this.hubConnection.on('transferchartdata', (data) => {
  //    this.data = data;
  //    console.log(data);
  //  });
  //}
}
