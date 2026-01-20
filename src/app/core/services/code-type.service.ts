import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface CodeTypeRequest {
    codeTypeCode: string;
    nameAr: string;
    nameEn: string;
    descriptionAr: string;
    descriptionEn: string;
}

export interface CodeTypeResponse {
    statusCode: number;
    message: string;
    data: {
        id: number;
        codeTypeCode: string;
        nameAr: string;
        nameEn: string;
        descriptionAr: string;
        descriptionEn: string;
        isActive: boolean;
        createdAt: string;
        createdBy: string;
        approvedAt: string | null;
        approvedBy: string | null;
    };
}

export interface CodeTypeListResponse {
    statusCode: number;
    message: string;
    data: {
        id: number;
        codeTypeCode: string;
        nameAr: string;
        nameEn: string;
        descriptionAr: string;
        descriptionEn: string;
        isActive: boolean;
        createdAt: string;
        createdBy: string;
        approvedAt: string | null;
        approvedBy: string | null;
    }[];
}

@Injectable({
    providedIn: 'root'
})
export class CodeTypeService {
    private apiUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    createCodeType(data: CodeTypeRequest): Observable<CodeTypeResponse> {
        return this.http.post<CodeTypeResponse>(
            `${this.apiUrl}/CodeTypes/Create`,
            data
        );
    }

    getAllCodeTypes(): Observable<CodeTypeListResponse> {
        return this.http.get<CodeTypeListResponse>(
            `${this.apiUrl}/CodeTypes/GetAll`
        );
    }
}
