import { priorityOfTask, statusOfTask, typeOfIssue } from "../enums/enums";

export interface ITicket {

  id: number,
  title: string,

  createDate: Date,
  updatedDate: Date,
  dueDate: Date,

  creatorId: string,
  executorId: string,
  creator: string,
  executor: string,

  issueType: typeOfIssue | string,
  status: statusOfTask | string,
  priority: priorityOfTask | string,

  description: string,
  executionList: Array<IExecutionList>,
  watchList: Array<IWatchList>,
  comments: Array<ITicketComment>
  
}

export interface ITicketComment{

  id: number,
  ticketId: number,
  creator: string,
  createDate: Date,
  description: string,
  edited: boolean

}

export interface IExecutionList{

  userId: string,
  ticketId: number, 
  userName?: string

}
export interface IWatchList{

  userId: string,
  ticketId: number, 
  userName?: string

}