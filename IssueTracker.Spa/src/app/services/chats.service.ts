import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_URL } from '../constants/constants';
import { IChat } from '../models/ichats.model';
import { IUser } from '../types/user.interface';

@Injectable({ providedIn: 'root' })

export class ChatsService {
    private apiUrl: string;
    constructor(private http: HttpClient) {
        this.apiUrl = `${API_URL}`;
     }

    getAll() {
        return this.http.get<string>(`${this.apiUrl}/Chats`);
    }

    getChatId(id: string){
        return this.http.get(`${this.apiUrl}/Chats/getChatId?userId=${id}`);
    }

    getGuysById(id: string) {
        return this.http.get<Array<IUser>>(`${this.apiUrl}/Chats/getGuysById?id=${id}`);
    }

    create(item: IChat){        
        return this.http.post<string>(`${this.apiUrl}/Chats`, item);
    }

    update(item: IChat){
        return this.http.put<string>(`${this.apiUrl}/Chats/${item.id}`, item);
    }

    delete(item: IChat){
        return this.http.delete(`${this.apiUrl}/Chats/${item.id}`);
    }    

}
