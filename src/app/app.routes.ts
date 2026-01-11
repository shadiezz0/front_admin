import { Routes } from '@angular/router';
import { authGuard, guestGuard } from './core/guards';

export const routes: Routes = [
    // Landing page (public)
    {
        path: '',
        loadComponent: () => import('./features/landing/landing.component').then(m => m.LandingComponent)
    },

    // Auth routes (only for guests)
    {
        path: 'auth/login',
        loadComponent: () => import('./features/auth/login.component').then(m => m.LoginComponent),
        canActivate: [guestGuard]
    },

    // Protected routes (require authentication)
    {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
        canActivate: [authGuard]
    },

    // Code Generator Flow (protected)
    {
        path: 'code-type',
        loadComponent: () => import('./features/code-type/code-type.component').then(m => m.CodeTypeComponent),
        canActivate: [authGuard]
    },
    {
        path: 'code-type-attribute',
        loadComponent: () => import('./features/code-type-attribute/code-type-attribute.component').then(m => m.CodeTypeAttributeComponent),
        canActivate: [authGuard]
    },
    {
        path: 'code-type-main',
        loadComponent: () => import('./features/code-type-main/code-type-main.component').then(m => m.CodeTypeMainComponent),
        canActivate: [authGuard]
    },
    {
        path: 'code-details',
        loadComponent: () => import('./features/code-details/code-details.component').then(m => m.CodeDetailsComponent),
        canActivate: [authGuard]
    },
    {
        path: 'code-settings',
        loadComponent: () => import('./features/code-settings/code-settings.component').then(m => m.CodeSettingsComponent),
        canActivate: [authGuard]
    },
    {
        path: 'code-sequence',
        loadComponent: () => import('./features/code-sequence/code-sequence.component').then(m => m.CodeSequenceComponent),
        canActivate: [authGuard]
    },
    {
        path: 'code-generation',
        loadComponent: () => import('./features/code-generation/code-generation.component').then(m => m.CodeGenerationComponent),
        canActivate: [authGuard]
    },

    // Other protected routes
    {
        path: 'settings',
        loadComponent: () => import('./features/settings/settings.component').then(m => m.SettingsComponent),
        canActivate: [authGuard]
    },

    // Redirect unknown routes to landing
    {
        path: '**',
        redirectTo: ''
    }
];
