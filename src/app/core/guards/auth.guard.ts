import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services';
import { ROUTES } from '../constants';

/**
 * Guard to protect routes that require authentication
 */
export const authGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (authService.isLoggedIn()) {
        return true;
    }

    // Redirect to login page with return url
    router.navigate([ROUTES.AUTH.LOGIN], {
        queryParams: { returnUrl: state.url }
    });
    return false;
};

/**
 * Guard to prevent authenticated users from accessing auth pages
 */
export const guestGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (!authService.isLoggedIn()) {
        return true;
    }

    // Redirect to dashboard if already authenticated
    router.navigate([ROUTES.DASHBOARD]);
    return false;
};
