import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_URL } from '../constants/constants';

@Injectable({ providedIn: 'root' })

export class WeatherService {

    private apiUrl: string;

    constructor(private http: HttpClient) {
        this.apiUrl = `${API_URL}`;
    }

    getAll() {
        return this.http.get<string>(`${this.apiUrl}/WeatherForecast`);
    }
}