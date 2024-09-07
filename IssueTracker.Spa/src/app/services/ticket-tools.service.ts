import { Injectable } from '@angular/core';
import { priorityOfTask, statusOfTask, typeOfIssue } from '../enums/enums';
import { EditTicketsComponent } from '../components/edit-tickets/edit-tickets.component';
import { ITicket } from '../models/itickets.model';
import { MatDialog } from '@angular/material/dialog';
import { TicketsService } from './tickets.service';

@Injectable({
  providedIn: 'root'
})

export class TicketToolsService {

  constructor(private ticketService: TicketsService, public dialog: MatDialog) { }

  getstatusOfTaskName(item: string): string {
    const values = Object.values(statusOfTask);
    return values[parseInt(item)].toString();
  }

  gettypeOfIssueName(item: string): string {
    const values = Object.values(typeOfIssue);
    return values[parseInt(item)].toString();
  }

  gettpriorityOfTaskName(item: string): string {
    const values = Object.values(priorityOfTask);
    return values[parseInt(item)].toString();
  }

  openEditDialog(mTicketView: ITicket[], item: ITicket, onlyRead: boolean = false): void {

    const dialogRef = this.dialog.open(EditTicketsComponent,
      { width: '600px', data: { ticket: item, onlyRead: onlyRead } });

    dialogRef.afterClosed().subscribe((result: ITicket | undefined) => {
      if (result) {
        this.editItem(result, item as ITicket, mTicketView);
      }
    });
  }

  editItem(result: ITicket, item: ITicket, mTicketView: ITicket[]) {
    this.ticketService.update(result).subscribe(str => {
      var i = mTicketView.indexOf(item as unknown as ITicket);
      if (i !== -1) {
        mTicketView[i] = result as unknown as ITicket;
      }
    });
  }
}
