import { Component, Inject, OnDestroy, OnInit, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalRService } from '../../services/signalR.service';
import { Message } from '../../models/message.model';
import { MessagesService } from '../../services/messages.service';
import { IUser } from '../../types/user.interface';
import { ChatsService } from '../../services/chats.service';

@Component({
  selector: 'app-personal-chat',
  templateUrl: './personal-chat.component.html',
  styleUrl: './personal-chat.component.scss'
})

export class PersonalChatComponent implements OnInit, OnDestroy {

  chatId = -1;
  userId: string = "";
  users: Array<IUser> = [];
  userName = "";

  signalrConnectionEstablished$ = this.signalRService.connectionEstablished$;
  connectionEstablished = true;
  chatmessages = new Array<Message>;

  private readonly formbuilder = inject(FormBuilder);
  form = this.formbuilder.group({
    chatmessage: ['', Validators.required],
  });

  constructor(private route: ActivatedRoute,
    private readonly signalRService: SignalRService,
    private msgService: MessagesService,
    private chtService: ChatsService,
    private router: Router) { }

  ngOnInit() {

    this.route.params.subscribe(params => {

      this.chatId = parseInt(params['chatId']);
      this.userId = params['userId'];

      if (!this.chatId) {
        this.router.navigate((['/contacts']))
        return
      }

      this.chtService.getGuysById(this.chatId.toString()).subscribe(str => {
        this.users = str;
        //give my name
        const user = this.users.find(user => user.id === this.userId);
        this.userName = user ? user.username : "Unknown"

        this.msgService.getById(this.chatId.toString()).subscribe(str => {
          if (str)
            this.chatmessages = str;

          ////give names
          this.chatmessages = this.chatmessages.map(message => {
            const user = this.users.find(user => user.id === message.sender);
            return {
              ...message,
              sender: user ? user.username : "Unknown" // add sender name to msg
            };
          });
        })
      });

      //connect to signalR
      this.connect(this.chatId);
    });
  }

  ngOnDestroy(): void {
    this.signalRService.disconnect();
  }

  connect(int: number) {
    this.signalRService.init(this.chatId);
    this.signalRService.messageReceived$.subscribe((message) => {
      const user = this.users.find(user => user.id === message.sender);
      message.sender = user ? user.username : "Unknown"
      this.chatmessages.unshift(message);
    });
  }

  sendChat() {
    var message: Message = {
      id: 1,
      chatId: this.chatId,
      dateSent: new Date,
      message: this.form.controls.chatmessage.value!,
      sender: this.userId
    }
    this.form.controls.chatmessage.setValue('')
    this.signalRService.sendChatMessage(message);
  }
}