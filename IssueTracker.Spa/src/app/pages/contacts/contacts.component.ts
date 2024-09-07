import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { IUser } from '../../types/user.interface';
import { Router } from '@angular/router';
import { ChatsService } from '../../services/chats.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrl: './contacts.component.scss'
})

export class ContactsComponent implements OnInit {

  userId: string = "00000000-0000-0000-0000-000000000000";
  users: IUser[] = [];//{id, username}
  chatId = 0;

  constructor(private uService: UsersService, private cService: ChatsService, private router: Router) { }

  ngOnInit() {
    this.userId = localStorage.getItem('userId')!;
    this.getUsers();
  }

  writeMsg(user: IUser) {
    this.cService.getChatId(user.id).subscribe(str => {
      if (str) {
        this.router.navigate(['/personalChat', str, this.userId]);
      }
    })
  }

  watchInfo(user: IUser) {
    throw new Error('Method not implemented.');
  }

  getUsers() {
    this.uService.getAll().subscribe(str => {
      var _str = JSON.stringify(str);
      var parsedJson: any[] = JSON.parse(_str);
      this.users = this.users.concat(parsedJson);
    });
  }
}