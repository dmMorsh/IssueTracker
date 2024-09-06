import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { NotificationService } from './services/NotificationService';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})

export class AppComponent implements OnInit {
  
  title = 'My-app';
  isAuth = false;
  
  constructor(public authService: AuthService
    , private notificationService: NotificationService
    , private cookieService: CookieService
  ){}

  ngOnInit(): void {
    this.authService.isAuth$.subscribe(newisAuth => {
      this.isAuth = newisAuth;
      //add notification
      if(newisAuth){
        this.ConnectSR()
      }
      else{
        this.notificationService.disconnect()
      }
    });
    this.authService.checkAuth();
  }

  ConnectSR(){
    var userId: string | null = localStorage.getItem('userId');
    var token: string = this.cookieService.get('token')!;
    if(userId != null && token != null)
    this.notificationService.init(userId, token);
  }
}