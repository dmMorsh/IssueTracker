import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_URL } from '../constants/constants';
import { ITicket } from '../models/itickets.model';

@Injectable({ providedIn: 'root' })

export class TicketsService {

    private apiUrl: string;

    constructor(private http: HttpClient) {
        this.apiUrl = `${API_URL}`;
    }

    getAll() {
        return this.http.get<string>(`${this.apiUrl}/Tickets`);
    }

    getPage(page: number, pageSize: number = 10) {
        return this.http.get<{ totalPages: number, tickets: ITicket[] }>(`${this.apiUrl}/Tickets/page?page=${page}&pageSize=${pageSize}`);
    }

    getWatchingPage(page: number, pageSize: number = 10) {
        return this.http.get<{ totalPages: number, tickets: ITicket[] }>(`${this.apiUrl}/Tickets/watching/page?page=${page}&pageSize=${pageSize}`);
    }

    getExecutingPage(page: number, pageSize: number = 10) {
        return this.http.get<{ totalPages: number, tickets: ITicket[] }>(`${this.apiUrl}/Tickets/executing/page?page=${page}&pageSize=${pageSize}`);
    }

    create(item: ITicket) {
        return this.http.post<string>(`${this.apiUrl}/Tickets`, item);
    }

    update(item: ITicket) {
        return this.http.put<string>(`${this.apiUrl}/Tickets/${item.id}`, item);
    }

    delete(item: ITicket) {
        return this.http.delete(`${this.apiUrl}/Tickets/${item.id}`);
    }
}
