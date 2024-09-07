import { Component, EventEmitter, Output, Input, OnInit, Inject, OnDestroy } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IExecutionList, ITicket, ITicketComment, IWatchList } from '../../models/itickets.model';
import { priorityOfTask, statusOfTask, typeOfIssue } from '../../enums/enums';
import { DatePipe } from '@angular/common';
import { UsersService } from '../../services/users.service';
import { CommentsService } from '../../services/comments.service';

@Component({
  selector: 'app-edit-tickets',
  templateUrl: './edit-tickets.component.html',
  styleUrl: './edit-tickets.component.scss'
})

export class EditTicketsComponent implements OnInit, OnDestroy {

  onlyRead = false;
  oldStatus = "";

  userId = "";
  userName = "";

  editedId = -1;
  editedExId = "";
  editedWcId = "";
  commentInput = "";
  selectedDate = '2000-01-01';  //YYYY-MM-DD
  selectedTime = '10:00';       //HH:mm
  //to switch card-body
  isEditComment = false;
  isEditWatchers = false;
  isEditExecutors = false;

  isNew = false;
  editedTicket!: ITicket;

  searchQuery: string = "";
  selectedUser: string = "00000000-0000-0000-0000-000000000000";
  selectedExecuter: string = "";
  selectedWatcher: string = "";
  users: any[] = [];//{id, username}
  filteredUsers: any[] = [];

  issueTypes = Object.keys(typeOfIssue)
    .filter(key => isNaN(Number(key)));

  priorityTask = Object.keys(priorityOfTask)
    .filter(key => isNaN(Number(key)));

  statusTask = Object.keys(statusOfTask)
    .filter(key => isNaN(Number(key)));

  constructor(
    private dialogRef: MatDialogRef<EditTicketsComponent>,
    private datePipe: DatePipe,
    private uService: UsersService,
    private commService: CommentsService
    , @Inject(MAT_DIALOG_DATA) public data: { ticket: ITicket, onlyRead: boolean }) { }

  ngOnDestroy(): void {

    if(this.onlyRead && this.editedTicket.status == this.oldStatus){
      this.dialogRef.close();
      return;
    }

    //normalize enums
    this.editedTicket.issueType = this.issueTypes.indexOf(this.editedTicket.issueType.toString());
    this.editedTicket.status = this.statusTask.indexOf(this.editedTicket.status.toString());
    this.editedTicket.priority = this.priorityTask.indexOf(this.editedTicket.priority.toString());

    //to refresh name on page
    const objectToFind = { id: this.selectedUser };
    const index = this.users.findIndex(obj => obj.id === objectToFind.id);
    if (index !== -1) {
      this.editedTicket.executor = this.users[index].username;

      if (this.editedTicket.executionList.length == 0) {
        this.editedTicket.executionList.push({
          ticketId: this.editedTicket.id, userId: this.selectedUser,
          userName: this.users.find(i => i.id == this.selectedUser).username
        })
      } else {
        var i = this.editedTicket.executionList.findIndex(obj =>
          obj.userId === this.selectedUser &&
          obj.ticketId === this.editedTicket.id
        );
        if (i === -1) {
          this.editedTicket.executionList.push({ userId: this.selectedUser, ticketId: this.editedTicket.id });
        }
      }
    }
    if (this.editedTicket.watchList.length == 0) {
      this.editedTicket.watchList.push({
        ticketId: this.editedTicket.id, userId: this.userId,
        userName: this.userName
      })
    }
    this.editedTicket.executorId = this.selectedUser;

    //normalize date
    var strDate: any = "" + this.selectedDate + "T" + this.selectedTime + ":00.000Z";
    this.editedTicket.dueDate = strDate;
    var newDate: any = this.datePipe.transform(new Date, 'yyyy-MM-ddTHH:mm:ss.SSS') + "Z";
    this.editedTicket.updatedDate = newDate;
    if (this.isNew) {
      newDate = this.datePipe.transform(this.editedTicket.createDate, 'yyyy-MM-ddTHH:mm:ss.SSS') + "Z";
      this.editedTicket.createDate = newDate;
    }

    this.dialogRef.close(this.editedTicket);
  }

  ngOnInit(): void {

    this.editedTicket = JSON.parse(JSON.stringify(this.data?.ticket ?? null));

    this.onlyRead = this.data?.onlyRead ?? false;
    if(this.editedTicket != null && this.onlyRead){
      var myInt: any = (this.editedTicket.status);
      this.oldStatus = Object.values(statusOfTask)[myInt] as string;
    }

    this.userId = localStorage.getItem('userId')!;
    if (!this.userId) this.userId = "";
    this.userName = localStorage.getItem('username')!;
    if (!this.userName) this.userName = "unknown";

    if (!this.editedTicket) {
      this.makeNewTicket();
    } else {
      this.fillTicket();
    }

    this.uService.getAll().subscribe(str => {

      var _str = JSON.stringify(str);
      var parsedJson: any[] = JSON.parse(_str);
      const objectToDelete = { id: this.editedTicket.executorId, username: this.editedTicket.executor };
      const index = parsedJson.findIndex(obj => obj.id === objectToDelete.id && obj.username === objectToDelete.username);
      if (index !== -1) {
        parsedJson.splice(index, 1);
      }
      if (this.editedTicket.executorId != this.userId) {
        parsedJson.push({ id: this.userId, username: this.userName });
      }
      this.users = this.users.concat(parsedJson);
    });
  }

  makeNewTicket() {

    this.isNew = true;
    this.editedTicket = {
      id: 0,
      title: '',

      createDate: new Date(),
      updatedDate: new Date(),
      dueDate: new Date(),

      creatorId: this.userId,
      executorId: "",
      creator: "",
      executor: "",

      issueType: "task",
      status: "todo",
      priority: "high",

      description: '',
      executionList: [],
      watchList: [{ userId: this.userId, ticketId: 0, userName: this.userName }],
      comments: []
    }
  }

  fillTicket() {

    if (this.editedTicket.executor != "") {
      this.users.push({ id: this.editedTicket.executorId, username: this.editedTicket.executor });
      this.selectedUser = this.editedTicket.executorId;
    }
    this.selectedDate = this.editedTicket.dueDate.toString().slice(0, 10)
    this.selectedTime = this.editedTicket.dueDate.toString().slice(11, 16)
    var myInt: any = (this.editedTicket.issueType);
    this.editedTicket.issueType = Object.values(typeOfIssue)[myInt];
    var myInt: any = (this.editedTicket.status);
    this.editedTicket.status = Object.values(statusOfTask)[myInt];
    var myInt: any = (this.editedTicket.priority);
    this.editedTicket.priority = Object.values(priorityOfTask)[myInt];

    if (this.editedTicket.executionList === undefined) {
      this.editedTicket.executionList = [];
    }
    if (this.editedTicket.watchList === undefined) {
      this.editedTicket.watchList = [];
    }
    if (this.editedTicket.comments === undefined) {
      this.editedTicket.comments = [];
    }
  }

  updateUserList() {
    this.filteredUsers = this.users.filter(user => user.name.toLowerCase().includes(this.searchQuery.toLowerCase()));
  }

  editTicket(): void {
    this.dialogRef.close();
  }

  pushComment(desc: string) {

    if (desc != "" && this.editedId == -1) {
      var newComm: ITicketComment = {
        id: 0,
        ticketId: this.editedTicket.id,
        creator: this.userName,
        createDate: new Date,
        description: desc,
        edited: false
      }
      this.commService.create(newComm).subscribe(str => {
        var _str = JSON.stringify(str);
        var parsedJson = JSON.parse(_str);
        this.editedTicket.comments.unshift(parsedJson);
        this.commentInput = "";
      });
    } else if (desc != "" && this.editedId != -1) {
      var oldComm: ITicketComment = this.editedTicket.comments
      [this.editedTicket.comments.findIndex(obj => obj.id === this.editedId)]

      var editComm: ITicketComment = Object.assign({}, oldComm);
      this.commService.update(editComm).subscribe(str => {
        oldComm.edited = true;
        oldComm.description = this.commentInput;
        this.commentInput = "";
        this.editedId = -1;
      })
    }
  }

  deleteComment(comm: ITicketComment) {

    this.commService.delete(comm).subscribe(str => {
      var num: number = this.editedTicket.comments.findIndex(obj =>
        obj.id === comm.id
        && obj.createDate === comm.createDate
        && obj.creator === comm.creator);
      this.editedTicket.comments.splice(num, 1);
    });
  }

  editComment(comm: ITicketComment | null = null) {

    if (comm) {
      this.commentInput = comm.description;
      this.editedId = comm.id;
    } else {
      this.commentInput = "";
      this.editedId = -1;
    }
  }

  deleteExecutor(ex: IExecutionList) {

    var num: number = this.editedTicket.executionList.findIndex(obj =>
      obj.ticketId === ex.ticketId
      && obj.userId === ex.userId
      && obj.userName === ex.userName
    );
    this.editedTicket.executionList.splice(num, 1);
  }

  editExecutor(ex: IExecutionList) {

    if (ex) {
      this.editedExId = ex.userId;
      this.selectedExecuter = ex.userId;
    } else {
      this.editedExId = "";
    };
  }

  clearExecutor() {
    this.selectedExecuter = "";
    this.editedExId = "";
  }

  pushExecuter(arg0: string) {

    var found = this.editedTicket.executionList.find(i => i.userId == this.selectedExecuter);
    if (found) {
      this.selectedExecuter = "";
      this.editedExId = "";
      return;
    }

    if (this.selectedExecuter !== "" && this.selectedExecuter !== null) {
      if (this.editedExId === "") {
        var name = this.users.find(i => i.id == this.selectedExecuter).username;
        this.editedTicket.executionList.push({ ticketId: this.editedTicket.id, userId: this.selectedExecuter, userName: name })
        this.selectedExecuter = "";
      } else {//if need replase
        var oldEx: IExecutionList = this.editedTicket.executionList
        [this.editedTicket.executionList.findIndex(obj => obj.userId === this.editedExId)]
        oldEx.userId = this.selectedExecuter;
        oldEx.userName = this.users.find(i => i.id == this.selectedExecuter).username;
        this.selectedExecuter = "";
        this.editedExId = "";
      }
    } else {
      this.editedExId = "";
    };
  }

  deleteWatcher(ex: IWatchList) {

    var num: number = this.editedTicket.watchList.findIndex(obj =>
      obj.ticketId === ex.ticketId
      && obj.userId === ex.userId
      && obj.userName === ex.userName
    );
    this.editedTicket.watchList.splice(num, 1);
  }

  editWatcher(wc: IWatchList) {

    if (wc) {
      this.editedWcId = wc.userId;
      this.selectedWatcher = wc.userId;
    } else {
      this.editedWcId = "";
    }
  }

  clearWatcher() {
    this.selectedWatcher = "";
    this.editedWcId = "";
  }

  pushWatcher(arg0: string) {

    var found = this.editedTicket.watchList.find(i => i.userId == this.selectedWatcher);
    if (found) {
      this.selectedWatcher = "";
      this.editedWcId = "";
      return;
    }

    if (this.selectedWatcher !== "" && this.selectedWatcher !== null) {
      if (this.editedWcId === "") {
        var name = this.users.find(i => i.id == this.selectedWatcher).username;
        this.editedTicket.watchList.push({ ticketId: this.editedTicket.id, userId: this.selectedWatcher, userName: name })
        this.selectedWatcher = "";
      } else {//if need replase
        var oldWc: IWatchList = this.editedTicket.watchList
        [this.editedTicket.watchList.findIndex(obj => obj.userId === this.editedWcId)]
        oldWc.userId = this.selectedWatcher;
        oldWc.userName = this.users.find(i => i.id == this.selectedWatcher).username;
        this.selectedWatcher = "";
        this.editedWcId = "";
      }
    } else {
      this.editedWcId = "";
    };
  }
}