import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../services/NotificationService';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss'
})
export class NavigationComponent implements OnInit {

  dis: string = "disabled"
  notifications: string[] = [];

  constructor(private notificationService: NotificationService) { }

  ngOnInit(): void {
    //JUST FOR FUN
    var myRole = localStorage.getItem('userRole')!;
    if (myRole === "Admin") { this.dis = "" }

    this.notificationService.notifications$.subscribe((notifications) => {
      this.notifications = notifications;
    });
  }
}