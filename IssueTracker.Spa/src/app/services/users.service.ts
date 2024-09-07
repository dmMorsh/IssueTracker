import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_URL } from '../constants/constants';
import { IUser } from '../types/user.interface';

@Injectable({ providedIn: 'root' })

export class UsersService {

    private apiUrl: string;

    constructor(private http: HttpClient) {
        this.apiUrl = `${API_URL}`;
    }

    getAll() {
        return this.http.get<string>(`${this.apiUrl}/Friends`);
    }

    getFriendRequests() {
        return this.http.get<string>(`${this.apiUrl}/Friends/friendRequests`);
    }

    getSubscriptions() {
        return this.http.get<string>(`${this.apiUrl}/Friends/subscriptions`);
    }

    getFriend(item: IUser) {
        return this.http.post<string>(`${this.apiUrl}/Friends`, { id: item.id });
    }

    subscribe(item: IUser) {
        return this.http.post<string>(`${this.apiUrl}/Friends/subscribe`, { id: item.id });
    }

    unsubscribe(item: IUser) {
        return this.http.post<string>(`${this.apiUrl}/Friends/unsubscribe`, { id: item.id });
    }

    update(item: IUser) {
        return this.http.put<string>(`${this.apiUrl}/Users/${item.id}`, item);
    }

    delete(item: IUser) {
        return this.http.delete(`${this.apiUrl}/Users/${item.id}`);
    }

    getByName(name: string) {
        return this.http.get(`${this.apiUrl}/Users/getByName?userName=${name}`);
    }
}
