import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss'
})
export class NavigationComponent implements OnInit{

  dis: string = "disabled"

  ngOnInit(): void {
    //JUST FOR FUN
    var myRole = localStorage.getItem('userRole')!;
    if(myRole === "Admin"){this.dis = ""}
  }
}
