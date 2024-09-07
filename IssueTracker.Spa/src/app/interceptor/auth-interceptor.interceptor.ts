import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  //refresh token if expired
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.handle401Error(request, next);
        }
        return throwError(error);
      })
    );
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    var at = request.headers.get("Authorization")! || ""
    return this.authService.refreshToken(at).pipe(
      switchMap((obj) => {
        request = request.clone({
          setHeaders: {
            Accept: "application/json",
            Authorization: `Bearer ${obj.token}`
          }
        })
        return next.handle(request);
      }),
      catchError((error) => {
        return throwError(error);
      })
    );
  }
}