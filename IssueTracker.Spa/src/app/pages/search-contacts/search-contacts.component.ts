import { Component } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { IUser } from '../../types/user.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search-contacts',
  templateUrl: './search-contacts.component.html',
  styleUrl: './search-contacts.component.scss'
})

export class SearchContactsComponent {

  users: IUser[] = [];
  isExpanded = false;

  constructor(private uService: UsersService, private router: Router) { }

  watchInfo(_t17: IUser) {
    throw new Error('Method not implemented.');
  }

  expand() {
    this.isExpanded = true;
  }

  search(user: string) {
    if (user == "") return;
    this.isExpanded = false;
    this.users = [];
    this.uService.getByName(user).subscribe(str => {
      var _str = JSON.stringify(str);
      var parsedJson: IUser[] = JSON.parse(_str);
      this.users = this.users.concat(parsedJson);
    })
  }

  subscribe(friend: IUser) {
    this.uService.subscribe(friend).subscribe(str => {
    })
  }
}