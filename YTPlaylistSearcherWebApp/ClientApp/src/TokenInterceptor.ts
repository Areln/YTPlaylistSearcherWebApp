import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, from } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthGuard } from './app/AuthGuard';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private authGuard: AuthGuard) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authGuard.getToken();  // Get token from AuthGuard

    // Attach token if available
    if (token) {
      req = this.addToken(req, token);
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        // If 401 (Unauthorized), attempt to refresh the token
        if (error.status === 401 && token) {
          return this.handle401Error(req, next);
        }
        return throwError(error);
      })
    );
  }

  // Adds the JWT token to the authorization header
  private addToken(req: HttpRequest<any>, token: string): HttpRequest<any> {
    return req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
  }

  // Handles 401 errors by attempting to refresh the token
  private handle401Error(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return from(this.authGuard.tryRefreshingTokens(this.authGuard.getToken())).pipe(
      switchMap(isRefreshSuccess => {
        if (isRefreshSuccess) {
          const newToken = this.authGuard.getToken();  // Get the new token after refreshing
          if (newToken) {
            return next.handle(this.addToken(req, newToken));
          }
        }
        return throwError('Token refresh failed');
      })
    );
  }
}
