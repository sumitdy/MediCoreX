import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';

import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

@Component({
  selector: 'app-register',

  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],

  templateUrl: './register.component.html',

  styleUrl: './register.component.scss'
})
export class RegisterComponent {

  registerForm = new FormGroup({

    fullName: new FormControl('', [
      Validators.required,
      Validators.minLength(3)
    ]),

    email: new FormControl('', [
      Validators.required,
      Validators.email
    ]),

    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6)
    ])

  });

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(): void {

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.authService.register(this.registerForm.value).subscribe({

      next: (response) => {

        console.log(
          'Registration successful:',
          response
        );

        alert(
          'Registration successful! Please login.'
        );

        this.router.navigate(['/login']);
      },

      error: (error) => {

        console.error(
          'Registration failed:',
          error
        );

        alert(
          error.error?.Message ||
          'Registration failed. Please try again.'
        );
      }

    });
  }
}