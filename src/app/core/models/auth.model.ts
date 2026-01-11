// Login credentials
export interface LoginCredentials {
    userCode: string;
    password: string;
}

// Login API response data
export interface LoginData {
    userCode: string;
    token: string;
    roles: string[];
    claims: Record<string, any>;
    expiresOn: string;
    employeeName: string;
    permissions: string[];
    genderId: number;
}

// API response wrapper
export interface LoginResponse {
    success: boolean;
    message: string;
    data: LoginData;
}

// User model (stored locally)
export interface User {
    userCode: string;
    employeeName: string;
    roles: string[];
    permissions: string[];
    genderId: number;
    token: string;
    expiresOn: string;
}

// User role enum (based on your API roles)
export enum UserRole {
    ADMIN = 'Admin',
    USER = 'User',
    GUEST = 'Guest'
}

// Register data
export interface RegisterData {
    username: string;
    email: string;
    password: string;
}

// Legacy - kept for compatibility
export interface AuthResponse {
    token: string;
    refreshToken?: string;
    user: User;
}
