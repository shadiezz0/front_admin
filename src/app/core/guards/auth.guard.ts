import { inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services';


export const authGuard: CanActivateFn = (route, state) => {
    const platformId = inject(PLATFORM_ID);
    const authService = inject(AuthService);
    const router = inject(Router);

    if (!isPlatformBrowser(platformId)) {
        return true;
    }

    if (authService.isLoggedIn()) {
        return true;
    }

    router.navigate(['/']);
    return false;
};


export const guestGuard: CanActivateFn = () => {
    return true;
};
