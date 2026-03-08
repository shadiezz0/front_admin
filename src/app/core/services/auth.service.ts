import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject, tap, catchError, throwError, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User, LoginResponse, LoginCredentials, RegisterData } from '../models';
import { STORAGE_KEYS, API_ENDPOINTS, ROUTES } from '../constants';

interface ValidateTokenResponse {
    success: boolean;
    message: string;
    data: {
        token: string;
        userCode: string;
        employeeName: string;
        empFullNameEn?: string;
        roles: string[];
        permissions: string[];
        expiresOn: string;
        genderId?: number;
    };
}

interface CheckTokenResponse {
    success: boolean;
    isTokenValid: boolean;
    expiresOn: string;
    message: string;
}

/**
 * Utility function to safely access localStorage
 */
function safeLocalStorageGetItem(key: string): string | null {
    if (typeof localStorage !== 'undefined' && localStorage.getItem) {
        try {
            return localStorage.getItem(key);
        } catch (error) {
            console.error('Error accessing localStorage:', error);
            return null;
        }
    } else {
        console.warn('localStorage is not available in this environment.');
        return null;
    }
}

function safeLocalStorageSetItem(key: string, value: string): void {
    if (typeof localStorage !== 'undefined' && localStorage.setItem) {
        try {
            localStorage.setItem(key, value);
        } catch (error) {
            console.error('Error accessing localStorage:', error);
        }
    } else {
        console.warn('localStorage is not available in this environment.');
    }
}

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
     * Simple login that just stores the token.
     * Use loginWithToken for a full session initialization with whoami.
     */
    login(token: string): void {
        safeLocalStorageSetItem(STORAGE_KEYS.TOKEN, token);
    }
    loginWithCredentials(credentials: LoginCredentials): Observable<LoginResponse> {
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
     * Validate a raw JWT token against the backend and create a session.
     * Used for SSO: the administrative project passes its token via URL.
     */
    loginWithToken(token: string): Observable<boolean> {
        // Store the token immediately so interceptor can use it for subsequent calls
        safeLocalStorageSetItem(STORAGE_KEYS.TOKEN, token);

        return this.getWhoAmI().pipe(
            map(response => {
                if (response.success && response.data) {
                    const user: User = {
                        userCode: response.data.userCode,
                        employeeName: response.data.empFullNameEn || response.data.employeeName || '',
                        roles: response.data.roles,
                        permissions: response.data.permissions,
                        genderId: 0, // Not provided by whoami usually
                        token: token,
                        expiresOn: '' // We might need to decode JWT to get this if not provided
                    };

                    safeLocalStorageSetItem(STORAGE_KEYS.USER, JSON.stringify(user));
                    this.currentUserSubject.next(user);
                    return true;
                }
                this.clearAuthData();
                return false;
            }),
            catchError(error => {
                console.error('[AuthService] loginWithToken failed:', error);
                this.clearAuthData();
                return throwError(() => error);
            })
        );
    }

    /**
     * Get information about the currently logged-in user.
     * GET /Auth/whoami
     */
    getWhoAmI(): Observable<any> {
        return this.http.get<any>(`${this.apiUrl}${API_ENDPOINTS.AUTH.WHOAMI}`);
    }

    /**
     * Check if the currently stored token is still valid on the server.
     * GET /Auth/check-token — the auth interceptor sends the Bearer header automatically.
     * Use this for server-side session validation (more reliable than client-side expiry check).
     */
    checkToken(): Observable<CheckTokenResponse> {
        return this.http.get<CheckTokenResponse>(`${this.apiUrl}/Auth/check-token`);
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
        window.location.href = '/';
    }

    /**
     * Get current auth token
     */
    getToken(): string | null {
        return safeLocalStorageGetItem(STORAGE_KEYS.TOKEN);
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


    getDisplayName(): string {
        const user = this.currentUser();
        if (!user) return '';
        return user.employeeName || user.userCode || '';
    }

    
    private handleLoginSuccess(data: any): void {
        // Store token
        safeLocalStorageSetItem(STORAGE_KEYS.TOKEN, data.token);

        // Create user object
        const user: User = {
            userCode: data.userCode,
            employeeName: data.empFullNameEn || data.employeeName || '',
            roles: data.roles,
            permissions: data.permissions,
            genderId: data.genderId,
            token: data.token,
            expiresOn: data.expiresOn
        };

        // Store user data
        safeLocalStorageSetItem(STORAGE_KEYS.USER, JSON.stringify(user));

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
            const userJson = safeLocalStorageGetItem(STORAGE_KEYS.USER);
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
