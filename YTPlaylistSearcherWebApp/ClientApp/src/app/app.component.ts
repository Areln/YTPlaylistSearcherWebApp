import { Component } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';

  constructor(private jwtHelper: JwtHelperService) { }

  isLoggedIn() {
    const token = localStorage.getItem("jwt");
    if (token) {
      return true;
    } else {
      return false;
    }
  }
}
