// API endpoints
export const API_ENDPOINTS = {
    AUTH: {
        LOGIN: '/auth/login',
        REGISTER: '/auth/register',
        LOGOUT: '/auth/logout',
        REFRESH: '/auth/refresh',
        PROFILE: '/auth/profile'
    },
    USERS: {
        LIST: '/users',
        DETAIL: (id: string) => `/users/${id}`,
        UPDATE: (id: string) => `/users/${id}`,
        DELETE: (id: string) => `/users/${id}`
    },
    CODE_GENERATOR: {
        GENERATE: '/code-generator/generate',
        TEMPLATES: '/code-generator/templates',
        HISTORY: '/code-generator/history'
    }
} as const;

// Storage keys
export const STORAGE_KEYS = {
    TOKEN: 'auth_token',
    REFRESH_TOKEN: 'refresh_token',
    USER: 'user_data',
    LANGUAGE: 'app_language',
    THEME: 'app_theme'
} as const;

// Route paths
export const ROUTES = {
    HOME: '/',
    AUTH: {
        LOGIN: '/auth/login',
        REGISTER: '/auth/register'
    },
    DASHBOARD: '/dashboard',
    SETTINGS: '/settings',
    LANDING: '/landing'
} as const;

// App configuration constants
export const APP_CONFIG = {
    TOKEN_EXPIRY_TIME: 3600000, // 1 hour in milliseconds
    REQUEST_TIMEOUT: 30000, // 30 seconds
    MAX_FILE_SIZE: 5242880, // 5MB in bytes
    DEBOUNCE_TIME: 300 // milliseconds
} as const;
