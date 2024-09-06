import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_URL } from '../constants/constants';
import { ITicket } from '../models/itickets.model';
import { Message } from '../models/message.model';

@Injectable({ providedIn: 'root' })

export class MessagesService {

    private apiUrl: string;

    constructor(private http: HttpClient) {
        this.apiUrl = `${API_URL}`;
     }

    getAll() {
        return this.http.get<string>(`${this.apiUrl}/Messages`);
    }

    getById(id: string) {
        return this.http.get<Array<Message>>(`${this.apiUrl}/Messages/getById?id=${id}`);
    }

    create(item: ITicket){        
        return this.http.post<string>(`${this.apiUrl}/Messages`, item);
    }

    update(item: ITicket){
        return this.http.put<string>(`${this.apiUrl}/Messages/${item.id}`, item);
    }

    delete(item: ITicket){
        return this.http.delete(`${this.apiUrl}/Messages/${item.id}`);
    }
}
