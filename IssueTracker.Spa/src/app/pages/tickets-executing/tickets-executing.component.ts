import { Component, OnInit } from '@angular/core';
import { ITicket } from '../../models/itickets.model';
import { TicketsService } from '../../services/tickets.service';
import { TicketToolsService } from '../../services/ticket-tools.service';

@Component({
  selector: 'app-tickets-executing',
  templateUrl: './tickets-executing.component.html',
  styleUrl: './tickets-executing.component.scss'
})
export class TicketsExecutingComponent implements OnInit{

  mTicketView!: ITicket[]; 
  currenPage = 1;
  countPage = 0;

  constructor(private ticketService: TicketsService, public tts: TicketToolsService){}

  ngOnInit(): void {
    this.getTickets();
  }

  getTickets(page: number = 1) {
    this.ticketService.getExecutingPage(page).subscribe(str => {      
      this.mTicketView = (str.tickets); 
      this.countPage = (str.totalPages); 
      this.currenPage = page;
    });
  }
}
