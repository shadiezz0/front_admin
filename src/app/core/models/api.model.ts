// API Response wrapper
export interface ApiResponse<T> {
    success: boolean;
    data?: T;
    message?: string;
    errors?: Record<string, string[]>;
}

// Pagination interface
export interface Pagination {
    page: number;
    pageSize: number;
    totalItems: number;
    totalPages: number;
}

// Paginated response
export interface PaginatedResponse<T> {
    items: T[];
    pagination: Pagination;
}

// Generic error response
export interface ErrorResponse {
    statusCode: number;
    message: string;
    timestamp: string;
    path?: string;
}
