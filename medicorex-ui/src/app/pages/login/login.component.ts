import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule
} from '@angular/forms';

import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  loginForm = new FormGroup({
    email: new FormControl(''),
    password: new FormControl('')
  });

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(): void {

    this.authService.login(this.loginForm.value).subscribe({

      next: (response) => {

        console.log('Login successful:', response);

        this.authService.saveToken(response.accessToken);

        this.authService.saveRefreshToken(response.refreshToken);

        this.router.navigate(['/dashboard']);

      },

      error: (error) => {

        console.error('Login failed:', error);

      }

    });

  }

}