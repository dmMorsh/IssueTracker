import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_URL } from '../constants/constants';
import { ITicketComment } from '../models/itickets.model';

@Injectable({ providedIn: 'root' })

export class CommentsService {

    private apiUrl: string;

    constructor(private http: HttpClient) {
        this.apiUrl = `${API_URL}`;
     }

    create(item: ITicketComment){
        return this.http.post<string>(`${this.apiUrl}/Comments`, item);
    }

    update(item: ITicketComment){
        return this.http.put<string>(`${this.apiUrl}/Comments/${item.id}`, item);
    }

    delete(item: ITicketComment){
        return this.http.delete(`${this.apiUrl}/Comments/${item.id}`);
    }
}
