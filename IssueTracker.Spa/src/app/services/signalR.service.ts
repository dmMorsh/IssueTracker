import { Inject, Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { Message } from '../models/message.model';
import { API_URL } from '../constants/constants';
import { CookieService } from 'ngx-cookie-service';

@Injectable({ providedIn: 'root' })

export class SignalRService {

  private hubConnection!: HubConnection;
  private messageReceived = new Subject<Message>();
  private connectionEstablished = new BehaviorSubject<boolean>(false);

  get messageReceived$() {
    return this.messageReceived.asObservable();
  }

  get connectionEstablished$() {
    return this.connectionEstablished.asObservable();
  }

  constructor(private cookieService: CookieService) { }

  init(chatId: number) {
    var token: string = this.cookieService.get('token')!;
    if (token != null) {
      this.createConnection(token, chatId);
      this.registerOnServerEvents();
      this.startConnection();
    }
  }

  sendChatMessage(message: Message) {
    this.hubConnection.invoke('SendMessage', message);
  }

  private createConnection(token: string, ChatId: number) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(API_URL + `/chathub?chatId=${ChatId}`, { accessTokenFactory: () => token })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();
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

  private registerOnServerEvents(): void {
    this.hubConnection.on('Send', (data) => {
      this.messageReceived.next(data);
    });
    this.hubConnection.on('ReceiveMessage', (data) => {
      this.messageReceived.next(data);
    });
  }

  disconnect(): void {
    this.hubConnection.stop();
  }
}