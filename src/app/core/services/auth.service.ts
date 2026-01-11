import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject, tap, catchError, throwError, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User, LoginResponse, LoginCredentials, RegisterData } from '../models';
import { STORAGE_KEYS, API_ENDPOINTS, ROUTES } from '../constants';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private readonly apiUrl = environment.apiUrl;
    private currentUserSubject = new BehaviorSubject<User | null>(this.getUserFromStorage());

    // Signals for reactive state management
    currentUser = signal<User | null>(this.getUserFromStorage());
    isAuthenticated = computed(() => this.currentUser() !== null);

    // Observable for legacy support
    currentUser$ = this.currentUserSubject.asObservable();

    constructor(
        private http: HttpClient,
        private router: Router
    ) {
        // Sync signal with BehaviorSubject
        this.currentUser$.subscribe(user => this.currentUser.set(user));
    }

    /**
     * Login user with credentials
     */
    login(credentials: LoginCredentials): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(
            `${this.apiUrl}${API_ENDPOINTS.AUTH.LOGIN}`,
            credentials
        ).pipe(
            tap(response => {
                if (response.success && response.data) {
                    this.handleLoginSuccess(response.data);
                }
            }),
            catchError(error => this.handleAuthError(error))
        );
    }

    /**
     * Register new user (if needed in the future)
     */
    register(data: RegisterData): Observable<any> {
        return this.http.post<any>(
            `${this.apiUrl}${API_ENDPOINTS.AUTH.REGISTER}`,
            data
        ).pipe(
            catchError(error => this.handleAuthError(error))
        );
    }

    /**
     * Logout current user
     */
    logout(): void {
        this.clearAuthData();
        this.currentUserSubject.next(null);
        this.router.navigate([ROUTES.AUTH.LOGIN]);
    }

    /**
     * Get current auth token
     */
    getToken(): string | null {
        return localStorage.getItem(STORAGE_KEYS.TOKEN);
    }

    /**
     * Check if user is authenticated
     */
    isLoggedIn(): boolean {
        const token = this.getToken();
        const user = this.currentUser();

        if (!token || !user) {
            return false;
        }

        // Check if token is expired
        if (user.expiresOn) {
            const expiryDate = new Date(user.expiresOn);
            const now = new Date();
            if (now >= expiryDate) {
                this.logout();
                return false;
            }
        }

        return true;
    }

    /**
     * Check if user has a specific permission
     */
    hasPermission(permission: string): boolean {
        const user = this.currentUser();
        return user?.permissions?.includes(permission) || false;
    }

    /**
     * Check if user has a specific role
     */
    hasRole(role: string): boolean {
        const user = this.currentUser();
        return user?.roles?.includes(role) || false;
    }

    /**
     * Get user permissions
     */
    getPermissions(): string[] {
        return this.currentUser()?.permissions || [];
    }

    /**
     * Get user roles
     */
    getRoles(): string[] {
        return this.currentUser()?.roles || [];
    }

    /**
     * Handle successful login
     */
    private handleLoginSuccess(data: any): void {
        // Store token
        localStorage.setItem(STORAGE_KEYS.TOKEN, data.token);

        // Create user object
        const user: User = {
            userCode: data.userCode,
            employeeName: data.employeeName,
            roles: data.roles,
            permissions: data.permissions,
            genderId: data.genderId,
            token: data.token,
            expiresOn: data.expiresOn
        };

        // Store user data
        localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(user));

        // Update state
        this.currentUserSubject.next(user);

        console.log('Login successful:', {
            user: data.employeeName,
            roles: data.roles,
            permissionsCount: data.permissions.length
        });
    }

    /**
     * Handle authentication error
     */
    private handleAuthError(error: any): Observable<never> {
        console.error('Authentication error:', error);

        let errorMessage = 'An error occurred during authentication';

        if (error.error?.message) {
            errorMessage = error.error.message;
        } else if (error.message) {
            errorMessage = error.message;
        } else if (error.status === 0) {
            errorMessage = 'Unable to connect to the server. Please check your connection.';
        } else if (error.status === 401) {
            errorMessage = 'Invalid credentials. Please try again.';
        } else if (error.status === 500) {
            errorMessage = 'Server error. Please try again later.';
        }

        return throwError(() => ({ error: { message: errorMessage } }));
    }

    /**
     * Get user from local storage
     */
    private getUserFromStorage(): User | null {
        try {
            const userJson = localStorage.getItem(STORAGE_KEYS.USER);
            if (!userJson) return null;

            const user = JSON.parse(userJson);

            // Validate token expiry
            if (user.expiresOn) {
                const expiryDate = new Date(user.expiresOn);
                const now = new Date();
                if (now >= expiryDate) {
                    this.clearAuthData();
                    return null;
                }
            }

            return user;
        } catch (error) {
            console.error('Error reading user from storage:', error);
            return null;
        }
    }

    /**
     * Clear all authentication data
     */
    private clearAuthData(): void {
        localStorage.removeItem(STORAGE_KEYS.TOKEN);
        localStorage.removeItem(STORAGE_KEYS.REFRESH_TOKEN);
        localStorage.removeItem(STORAGE_KEYS.USER);
    }
}
