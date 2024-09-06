import { Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { API_URL } from '../constants/constants';

@Injectable({ providedIn: 'root' })

export class NotificationService {

  private hubConnection!: HubConnection;
  private connectionEstablished = new BehaviorSubject<boolean>(false);
  
  get connectionEstablished$() {
    return this.connectionEstablished.asObservable();
  }
  
  init(UserId: string, token: string){    
    this.createConnection(UserId, token);
    this.addNotificationListener();
    this.startConnection();    
  }

  private createConnection(UserId: string, token: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(API_URL + `/chathub?userId=${UserId}`, { accessTokenFactory: () => token})
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();
  }

  public addNotificationListener() {
    this.hubConnection.on('ReceiveNotification', (message: string) => {
      console.log('Notification received: ' + message);
    });
  }

  private startConnection() {
    if (this.hubConnection.state === HubConnectionState.Connected) {
      return;
    }
    this.hubConnection.start().then(
      () => {
        this.connectionEstablished.next(true);
      },
      (error) => console.error(error)
    );
  }

  disconnect(): void {
    this.hubConnection.stop().then(() => {
    })
    .catch(err => 
      console.log('Error while stopping connection: ' + err)
    );
  }
}