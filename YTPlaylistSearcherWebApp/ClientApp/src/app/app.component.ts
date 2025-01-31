import { Component } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { PlaylistService } from './services/PlaylistService';

declare const google: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';

  constructor(private jwtHelper: JwtHelperService, private playlistService: PlaylistService) { }

  ngAfterViewInit(): void {
    // Initialize the Google Identity Services
    google.accounts.id.initialize({
      client_id: '806613470372-ghnlq2pa5nuoatd711hcplvlg4saiaqm.apps.googleusercontent.com',
      callback: this.handleCredentialResponse.bind(this),
    });

    // Render the Google Sign-In button
    google.accounts.id.renderButton(
      document.getElementById('g_id_signin'),
      { theme: 'outline', size: 'large' }  // Customization options
    );
  }

  handleCredentialResponse(response: any): void {
    const token = response.credential;
    // Send the token to your backend for verification and further processing
    this.playlistService.signInWithGoogle(token).subscribe(result => {
      console.log(result);
      localStorage.setItem('jwtToken', token);
    });
  }

  isLoggedIn() {
    const token = localStorage.getItem("jwt");
    if (token) {
      return true;
    } else {
      return false;
    }
  }
}
