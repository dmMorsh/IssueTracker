import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { IUser } from '../../types/user.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-im-following',
  templateUrl: './im-following.component.html',
  styleUrl: './im-following.component.scss'
})

export class ImFollowingComponent implements OnInit{
  
  users: IUser[] = [];

  constructor(private uService: UsersService, private router: Router) {}

  ngOnInit(): void {
    this.getSubscriptions();
  }

  getSubscriptions(){
    this.uService.getSubscriptions().subscribe(str => {
      var _str = JSON.stringify(str);
      var parsedJson: IUser[] = JSON.parse(_str);
      this.users = this.users.concat(parsedJson);
    })
  }

  watchInfo(_t9: any) {
    throw new Error('Method not implemented.');
  }

  delFriend(friend: IUser) {
    
    this.uService.unsubscribe(friend).subscribe(str => {
      if(str == "OK"){
        var i = this.users.indexOf(friend);
        if(i !== -1){
          this.users.splice(i,1);
        }
      }
    });
  }

}
