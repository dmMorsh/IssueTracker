import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})

export class HeaderComponent {

  isAuth = false;
  currentNickName = "";

  constructor(private authService: AuthService){
   
    this.authService.isAuth$.subscribe(newisAuth => {
      this.isAuth = newisAuth;})

    this.authService.currentNickName$.subscribe(newNickName => {
        this.currentNickName = newNickName;})
  
    this.authService.checkAuth();
  }

  logOut(){ 
    this.authService.setCondition(false);
    this.authService.logOut();
  }
}
