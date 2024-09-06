import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { setTimeout } from 'timers';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})

export class AppComponent implements OnInit {
  
  title = 'My-app';
  isAuth = false;
  
  constructor(public authService: AuthService){}

  ngOnInit(): void {
    this.authService.isAuth$.subscribe(newisAuth => {
      this.isAuth = newisAuth;
    });
    this.authService.checkAuth();
  }
}
