import { Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { API_URL } from '../constants/constants';
import { CookieService } from 'ngx-cookie-service';

@Injectable({ providedIn: 'root' })

export class NotificationService {

  private hubConnection!: HubConnection;
  private connectionEstablished = new BehaviorSubject<boolean>(false);
  private notificationsSubject = new BehaviorSubject<string[]>([]);
  public notifications$ = this.notificationsSubject.asObservable();

  constructor(private cookieService: CookieService) { }

  get connectionEstablished$() {
    return this.connectionEstablished.asObservable();
  }

  init() {
    var token: string = this.cookieService.get('token')!;
    if (token != null) {
      this.createConnection(token);
      this.addNotificationListener();
      this.startConnection();
    }
  }

  private createConnection(token: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(API_URL + `/chathub`, { accessTokenFactory: () => token })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();
  }

  public addNotificationListener() {
    this.hubConnection.on('ReceiveNotification', (message: string) => {
      console.log('Notification received: ' + message);
      this.updateNotifications(message);
    });
  }

  private updateNotifications(notification: string) {
    const currentNotifications = this.notificationsSubject.value;
    this.notificationsSubject.next([...currentNotifications, notification]);
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