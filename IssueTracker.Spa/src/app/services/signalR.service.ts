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
  
  init(chatId: number){    
    this.createConnection(chatId);
    this.registerOnServerEvents();
    this.startConnection();    
  }

  sendChatMessage(message: Message) {
    this.hubConnection.invoke('SendMessage', message);
  }

  private createConnection(ChatId: number) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(API_URL + `/chathub?chatId=${ChatId}`)
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