import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PlaylistService } from '../services/PlaylistService';
import { AuthGuard } from '../AuthGuard';

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
    private _playlistService: PlaylistService,
    private authguard: AuthGuard) {


    this.loginForm = formBuilder.group({
      email: [''],
      password: [''],
    });

    this.registerForm = formBuilder.group({
      username: [''],
      email: [''],
      password: [''],
      confirmPassword: [''],
    }, { validators: PasswordInputsMatch });

  }

  public submitLogin() {
    this.isLoading = true;
    if (this.loginForm.valid) {
      this._playlistService.SubmitLogin({ UserName: this.loginForm.controls.email.value, Password: this.loginForm.controls.password.value })
        .subscribe({
          next: (response: AuthenticatedResponse) => {
            this.authguard.onLogin(response);
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
      this._playlistService.SubmitRegistration({
        UserName: this.registerForm.controls.username.value,
        Email: this.registerForm.controls.email.value,
        Password: this.registerForm.controls.password.value
      })
        .subscribe({
          next: (response: AuthenticatedResponse) => {
            this.authguard.onLogin(response);
            this.router.navigate(["/search"]);
            this.invalidLogin = false;
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

/** make sure password fields are the same */
export const PasswordInputsMatch: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const password = control.get('password');
  const confirmPassword = control.get('confirmPassword');
  return password && confirmPassword && password.value !== confirmPassword.value ? { passwordMismatch: true } : null;
};

export interface LoginModel {
  username: string;
  password: string;
}
export interface AuthenticatedResponse {
  token: string;
  refreshToken: string;
}
