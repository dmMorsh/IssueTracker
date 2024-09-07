import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../services/NotificationService';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss'
})
export class NavigationComponent implements OnInit {

  dis: string = "disabled";
  frReq: string = "";
  ticEx: string = "";

  constructor(private notificationService: NotificationService) { }

  ngOnInit(): void {
    //JUST FOR FUN
    var myRole = localStorage.getItem('userRole')!;
    if (myRole === "Admin") {
      this.dis = ""
    }

    this.notificationService.notifications$.subscribe((notifications) => {
      if (notifications.length == 0) {
        return;
      } else if (notifications[0].includes("subscriber")) {
        notifications.shift();
        this.frReq = "🟣";
      } else if (notifications[0].includes("ticket")) {
        notifications.shift();
        this.ticEx = "🟣";
      }
    });
  }
}