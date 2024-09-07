import { Component, OnInit } from '@angular/core';
import { ITicket } from '../../models/itickets.model';
import { MatDialog } from '@angular/material/dialog';
import { TicketsService } from '../../services/tickets.service';
import { catchError, throwError } from 'rxjs';
import { EditTicketsComponent } from '../../components/edit-tickets/edit-tickets.component';
import { TicketToolsService } from '../../services/ticket-tools.service';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrl: './tickets.component.scss'
})

export class TicketsComponent implements OnInit {

  mTicketView!: ITicket[];
  currenPage = 1;
  countPage = 0;

  constructor(private ticketService: TicketsService, public dialog: MatDialog, public tts: TicketToolsService) { }

  ngOnInit(): void {
    this.getTickets();
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(EditTicketsComponent);
    dialogRef.afterClosed().subscribe((result: ITicket) => {
      if (result) {
        this.createItem(result);
      }
    });
  }

  getTickets(page: number = 1) {
    this.ticketService.getPage(page).subscribe(str => {
      this.mTicketView = (str.tickets);
      this.countPage = (str.totalPages);
      this.currenPage = page;
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

  deleteItem(item: ITicket): void {

    this.ticketService.delete(item)
      .pipe(
        catchError(error => {
          return throwError(error);
        }))
      .subscribe(str => {
        var i = this.mTicketView.indexOf(item);
        if (i !== -1) {
          this.mTicketView.splice(i, 1);
        }
        //if it was last then go bage back
        if (this.mTicketView.length == 0 && this.currenPage > 1) {
          this.getTickets(this.currenPage - 1)
        }
      });
  }
}