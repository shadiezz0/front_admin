import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services';
import { ROUTES } from '../../core/constants';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterLink],
    templateUrl: './login.component.html',
    styleUrl: './login.component.css'
})
export class LoginComponent {
    loginForm: FormGroup;
    isLoading = signal(false);
    errorMessage = signal<string | null>(null);

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private router: Router
    ) {
        this.loginForm = this.fb.group({
            userCode: ['', [Validators.required, Validators.minLength(2)]],
            password: ['', [Validators.required, Validators.minLength(3)]]
        });
    }

    onSubmit(): void {
        if (this.loginForm.valid && !this.isLoading()) {
            this.isLoading.set(true);
            this.errorMessage.set(null);

            this.authService.login(this.loginForm.value).subscribe({
                next: (response) => {
                    if (response.success) {
                        this.router.navigate([ROUTES.DASHBOARD]);
                    } else {
                        this.errorMessage.set(response.message || 'Login failed');
                        this.isLoading.set(false);
                    }
                },
                error: (error) => {
                    this.isLoading.set(false);
                    this.errorMessage.set(error.error?.message || 'Login failed. Please try again.');
                }
            });
        }
    }

    get userCode() { return this.loginForm.get('userCode'); }
    get password() { return this.loginForm.get('password'); }
}
