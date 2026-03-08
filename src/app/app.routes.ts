import { Routes } from '@angular/router';
import { authGuard } from './core/guards';
import { MainLayoutComponent } from './shared/layouts/main-layout/main-layout.component';

export const routes: Routes = [
    // Landing page — shown after SSO token validation.
    // Public route; no authGuard so it loads even before the user clicks "Go to Dashboard".
    {
        path: '',
        loadComponent: () => import('./features/landing/landing.component').then(m => m.LandingComponent)
    },

    // Protected routes with layout (require authentication)
    {
        path: '',
        component: MainLayoutComponent,
        canActivate: [authGuard],
        children: [
            {
                path: 'dashboard',
                loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
            },
            {
                path: 'code-type',
                loadComponent: () => import('./features/code-type/code-type.component').then(m => m.CodeTypeComponent)
            },
            {
                path: 'all-code-types',
                loadComponent: () => import('./features/code-type-list/code-type-list.component').then(m => m.CodeTypeListComponent)
            },
            {
                path: 'all-attribute-types',
                loadComponent: () => import('./features/code-attribute-type-list/code-attribute-type-list.component').then(m => m.CodeAttributeTypeListComponent)
            },
            {
                path: 'code-type-attribute',
                loadComponent: () => import('./features/code-type-attribute/code-type-attribute.component').then(m => m.CodeTypeAttributeComponent)
            },
            {
                path: 'all-main-entries',
                loadComponent: () => import('./features/code-main-list/code-main-list.component').then(m => m.CodeMainListComponent)
            },
            {
                path: 'code-type-main',
                loadComponent: () => import('./features/code-type-main/code-type-main.component').then(m => m.CodeTypeMainComponent)
            },
            {
                path: 'code-details',
                loadComponent: () => import('./features/code-details/code-details.component').then(m => m.CodeDetailsComponent)
            },
            {
                path: 'all-code-details',
                loadComponent: () => import('./features/code-details-list/code-details-list.component').then(m => m.CodeDetailsListComponent)
            },
            {
                path: 'code-settings',
                loadComponent: () => import('./features/code-settings/code-settings.component').then(m => m.CodeSettingsComponent)
            },
            {
                path: 'code-sequence',
                loadComponent: () => import('./features/code-sequence/code-sequence.component').then(m => m.CodeSequenceComponent)
            },
            {
                path: 'all-codes',
                loadComponent: () => import('./features/codes-list/codes-list.component').then(m => m.CodesListComponent)
            },
            {
                path: 'code-generation',
                loadComponent: () => import('./features/code-generation/code-generation.component').then(m => m.CodeGenerationComponent)
            },
            {
                path: 'update-code-type',
                loadComponent: () => import('./features/update-code-type/update-code-type.component').then(m => m.UpdateCodeTypeComponent)
            },
            {
                path: 'update-code-type/:id',
                loadComponent: () => import('./features/update-code-type/update-code-type.component').then(m => m.UpdateCodeTypeComponent)
            },
            {
                path: 'update-code-attribute-type/:id',
                loadComponent: () => import('./features/update-code-attribute-type/update-code-attribute-type.component').then(m => m.UpdateCodeAttributeTypeComponent)
            },
            {
                path: 'update-code-main/:id',
                loadComponent: () => import('./features/update-code-main/update-code-main.component').then(m => m.UpdateCodeMainComponent)
            },
            {
                path: 'update-code-detail/:id',
                loadComponent: () => import('./features/update-code-detail/update-code-detail.component').then(m => m.UpdateCodeDetailComponent)
            },
            {
                path: 'settings',
                loadComponent: () => import('./features/settings/settings.component').then(m => m.SettingsComponent)
            }
        ]
    },

    // Redirect unknown routes to root (which redirects to dashboard)
    {
        path: '**',
        redirectTo: ''
    }
];
