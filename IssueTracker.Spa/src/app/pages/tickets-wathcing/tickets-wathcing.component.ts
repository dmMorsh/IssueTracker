import { Component, OnInit } from '@angular/core';
import { ITicket } from '../../models/itickets.model';
import { TicketsService } from '../../services/tickets.service';
import { TicketToolsService } from '../../services/ticket-tools.service';

@Component({
  selector: 'app-tickets-wathcing',
  templateUrl: './tickets-wathcing.component.html',
  styleUrl: './tickets-wathcing.component.scss'
})
export class TicketsWathcingComponent {

  mTicketView!: ITicket[]; 
  currenPage = 1;
  countPage = 0;

  constructor(private ticketService: TicketsService, public tts: TicketToolsService){}

  ngOnInit(): void {
    this.getTickets();
  }

  getTickets(page: number = 1) {
    this.ticketService.getWatchingPage(page).subscribe(str => {      
      this.mTicketView = (str.tickets); 
      this.countPage = (str.totalPages); 
      this.currenPage = page;
    });
  }
}