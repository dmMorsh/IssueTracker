import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { NotificationService } from './services/NotificationService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})

export class AppComponent implements OnInit {

  title = 'My-app';
  isAuth = false;

  constructor(public authService: AuthService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.authService.isAuth$.subscribe(newisAuth => {
      this.isAuth = newisAuth;
      //add notification
      if (newisAuth) {
        this.notificationService.init();
      }
      else {
        this.notificationService.disconnect()
      }
    });
    this.authService.checkAuth();
  }
}