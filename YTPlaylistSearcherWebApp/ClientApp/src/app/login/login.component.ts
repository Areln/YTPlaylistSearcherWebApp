import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PlaylistService } from '../services/PlaylistService';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  invalidLogin: boolean = true;
  panelOpenState = true;
  isLoading: boolean = false;
  errorMessage: string | null | undefined;
  loginForm!: FormGroup;
  registerForm!: FormGroup;


  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private _playlistService: PlaylistService) {


    this.loginForm = formBuilder.group({
      email: [''],
      password: [''],
    });

    this.registerForm = formBuilder.group({
      email: [''],
      password: [''],
      confirmPassword: [''],
    });

  }

  public submitLogin() {
    this.isLoading = true;
    if (this.loginForm.valid) {
      this._playlistService.SubmitLogin({ UserName: this.loginForm.controls.email.value, Password: this.loginForm.controls.password.value })
        .subscribe({
          next: (response: AuthenticatedResponse) => {
            console.log(response);
            const token = response.token;
            const refreshToken = response.refreshToken;
            localStorage.setItem("jwt", token);
            localStorage.setItem("refreshToken", refreshToken);
            this.invalidLogin = false;
            this.router.navigate(["/search"]);
          },
          error: (err: HttpErrorResponse) => {
            this.invalidLogin = true
            this.isLoading = false;
            console.log(err);
          }
        })
    }
  }
  public submitRegister() {
    console.log(this.loginForm);
    this.isLoading = true;
    if (this.registerForm.valid) {
      this._playlistService.SubmitRegistration({ UserName: this.registerForm.controls.email.value, Password: this.registerForm.controls.password.value })
        .subscribe({
          next: (response: AuthenticatedResponse) => {
            console.log(response);
            const token = response.token;
            const refreshToken = response.refreshToken;
            localStorage.setItem("jwt", token);
            localStorage.setItem("refreshToken", refreshToken);
            this.invalidLogin = false;
            this.router.navigate(["/search"]);
          },
          error: (err: HttpErrorResponse) => {
            this.invalidLogin = true
            this.isLoading = false;
            console.log(err);
          }
        })
    }
  }
}

export interface LoginModel {
  username: string;
  password: string;
}
export interface AuthenticatedResponse {
  token: string;
  refreshToken: string;
}
