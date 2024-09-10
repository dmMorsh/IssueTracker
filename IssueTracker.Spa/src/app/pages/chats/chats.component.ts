import { Component, OnInit } from '@angular/core';
import { IChat } from '../../models/ichats.model';
import { Router } from '@angular/router';
import { ChatService } from '../../services/chats.service';

@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styleUrl: './chats.component.scss'
})
export class ChatsComponent implements OnInit {

  userId: string = "00000000-0000-0000-0000-000000000000";
  chats: IChat[] = [];

  constructor(private cService: ChatService, private router: Router) { }

  ngOnInit(): void {
    this.userId = localStorage.getItem('userId')!;
    this.getChats();
  }

  getChats() {
    this.cService.getAll().subscribe(str => {
      var _str = JSON.stringify(str);
      var parsedJson: any[] = JSON.parse(_str);
      this.chats = this.chats.concat(parsedJson);
    });
  }

  writeMsg(chat: IChat) {
    this.router.navigate(['/personalChat', chat.id, this.userId]);
  }
}