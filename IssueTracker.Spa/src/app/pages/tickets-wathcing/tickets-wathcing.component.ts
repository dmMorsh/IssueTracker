import { Component, OnInit } from '@angular/core';
import { ITicket } from '../../models/itickets.model';
import { TicketsService } from '../../services/tickets.service';
import { TicketToolsService } from '../../services/ticket-tools.service';
import { MatDialog } from '@angular/material/dialog';
import { EditTicketsComponent } from '../../components/edit-tickets/edit-tickets.component';

@Component({
  selector: 'app-tickets-wathcing',
  templateUrl: './tickets-wathcing.component.html',
  styleUrl: './tickets-wathcing.component.scss'
})
export class TicketsWathcingComponent {

  mTicketView!: ITicket[];
  currenPage = 1;
  countPage = 0;

  constructor(private ticketService: TicketsService, public tts: TicketToolsService, public dialog: MatDialog) { }

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

  openAddDialog(): void {
    const dialogRef = this.dialog.open(EditTicketsComponent);
    dialogRef.afterClosed().subscribe((result: ITicket) => {
      if (result) {
        this.createItem(result);
      }
    });
  }

  createItem(item: ITicket) {

    this.ticketService.create(item).subscribe(str => {
      var _str = JSON.stringify(str);
      var parsedJson = JSON.parse(_str);
      if (this.mTicketView.length >= 10) {
        this.mTicketView = [parsedJson]
        this.countPage++;
        this.currenPage = this.countPage;
      } else {
        this.mTicketView.push(parsedJson);
      }
    });
  }
}