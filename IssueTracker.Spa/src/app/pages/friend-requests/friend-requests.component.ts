import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { IUser } from '../../types/user.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-friend-requests',
  templateUrl: './friend-requests.component.html',
  styleUrl: './friend-requests.component.scss'
})

export class FriendRequestsComponent implements OnInit {

  users: IUser[] = [];

  constructor(private uService: UsersService, private router: Router) { }

  ngOnInit(): void {
    this.getFriendRequests();
  }

  getFriendRequests() {
    this.uService.getFriendRequests().subscribe(str => {
      var _str = JSON.stringify(str);
      var parsedJson: IUser[] = JSON.parse(_str);
      this.users = this.users.concat(parsedJson);
    })
  }

  addFriend(friend: IUser) {

    this.uService.getFriend(friend).subscribe(str => {
      if (str == "OK") {
        var i = this.users.indexOf(friend);
        if (i !== -1) {
          this.users.splice(i, 1);
        }
      }
    })
  }

  watchInfo(_t10: IUser) {
    throw new Error('Method not implemented.');
  }
}