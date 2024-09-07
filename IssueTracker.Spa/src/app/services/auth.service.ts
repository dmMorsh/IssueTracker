import { HttpClient } from "@angular/common/http";
import { Injectable, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { IAuthUser, IUser } from "../types/user.interface";
import { API_URL } from "../constants/constants";
import { Observable, Subject, catchError, tap } from "rxjs";
import { CookieService } from 'ngx-cookie-service';

@Injectable({
    providedIn: 'root',
})

export class AuthService {

    userValue = "";
    private isAuthSubject = new Subject<boolean>();
    isAuth$ = this.isAuthSubject.asObservable();

    private nameSubject = new Subject<string>();
    currentNickName$ = this.nameSubject.asObservable();

    constructor(private readonly http: HttpClient,
        private readonly router: Router,
        private cookieService: CookieService) { }

    setCondition(isAuth: boolean) {
        this.isAuthSubject.next(isAuth);
    }

    setNickName(nickName: string) {
        this.nameSubject.next(nickName);
    }

    checkAuth() {
        if (typeof localStorage !== 'undefined') {
            const token = this.cookieService.get('token');
            this.setCondition(!!token);
            var nickName: string = localStorage.getItem('username')!;
            if (nickName == null) { nickName = "Unknown" }
            this.setNickName(nickName);
        }
    }

    login(userData: IAuthUser) {

        return this.http.post<IUser>(API_URL + "/Authorisation", userData)
            .pipe(

                tap((res: IUser) => {
                    this.cookieService.set('token', res.token);
                    this.cookieService.set('refreshToken', res.refreshToken);
                    localStorage.setItem('userId', res.id)
                    localStorage.setItem('username', res.username)
                    localStorage.setItem('userRole', res.role!)
                    this.setCondition(true);
                    this.setNickName(res.username);
                    this.router.navigate(["/"])
                }),

                catchError(err => {
                    this.setCondition(false);
                    const errorMessage = err.error?.message || "An unexpected error occurred";
                    throw new Error(errorMessage);
                })
            )
    }

    register(userData: IAuthUser) {

        return this.http.post<IUser>(API_URL + "/Authorisation/register", userData)
            .pipe(

                tap((res: IUser) => {
                    localStorage.setItem('id', res.id.toString())
                    this.cookieService.set('token', res.token);
                    this.cookieService.set('refreshToken', res.refreshToken);
                    localStorage.setItem('username', res.username)
                    this.setCondition(true);
                    this.setNickName(res.username);
                    this.router.navigate(["/"])
                }),

                catchError(err => {
                    this.setCondition(false);
                    const errorMessage = err.error?.message || "An unexpected error occurred";
                    throw new Error(errorMessage);
                })
            )
    }

    logOut() {
        this.cookieService.delete('token');
    }

    refreshToken(at: string): Observable<any> {

        const rt = this.cookieService.get('refreshToken') || "";

        return this.http.post<{ token: string }>(API_URL + "/Authorisation/refresh-token"
            , { accessToken: at, refreshToken: rt })
            .pipe(
                tap((res: { token: string }) => {
                    if (res.token) {
                        this.cookieService.set('token', res.token);
                    }
                })
            )
    }
}