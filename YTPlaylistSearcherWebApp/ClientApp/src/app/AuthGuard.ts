import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthenticatedResponse } from './login/login.component';
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private jwtHelper: JwtHelperService, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    const isRefreshSuccess = await this.tryRefreshingTokens(token);
    if (!isRefreshSuccess) {
      this.router.navigate(["/loggedout"]);
    }

    return isRefreshSuccess;
  }

  private async tryRefreshingTokens(token: any): Promise<boolean> {
    const refreshToken: any = localStorage.getItem("refreshToken");
    if (!token || !refreshToken) {
      return false;
    }

    const credentials = JSON.stringify({ accessToken: token, refreshToken: refreshToken });
    let isRefreshSuccess: boolean;

    try {
      const refreshRes = await new Promise<AuthenticatedResponse | null>((resolve, reject) => {
        this.http.post<AuthenticatedResponse>(this.baseUrl + "token/refresh", { accessToken: token, refreshToken: refreshToken }, {
          headers: new HttpHeaders({
            "Content-Type": "application/json"
          })
        }).subscribe({
          next: (res: AuthenticatedResponse) => {

            resolve(res);
          },
          error: (_) => {
            isRefreshSuccess = false;
            console.log(_);
            resolve(null);
          }
        });
      });

      if (refreshRes != null) {
        localStorage.setItem("jwt", refreshRes.token);
        localStorage.setItem("refreshToken", refreshRes.refreshToken);
        isRefreshSuccess = true;
      } else {
        isRefreshSuccess = false;
      }
    } catch (e) {
      isRefreshSuccess = false;
    }

    return isRefreshSuccess;
  }
}
