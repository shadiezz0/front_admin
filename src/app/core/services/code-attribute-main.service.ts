import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface CodeAttributeMainRequest {
    codeTypeId: number;
    code: string;
    nameAr: string;
    nameEn: string;
    descriptionAr: string;
    descriptionEn: string;
    codeAttributeTypeId: number;
}

export interface CodeAttributeMainResponse {
    statusCode: number;
    message: string;
    data: {
        id: number;
        codeTypeId: number;
        code: string;
        nameAr: string;
        nameEn: string;
        descriptionAr: string;
        descriptionEn: string;
        codeAttributeTypeId: number;
        isActive: boolean;
        createdAt: string;
        createdBy: string;
    };
}

export interface CodeAttributeMainBulkResponse {
    statusCode: number;
    message: string;
    data: {
        id: number;
        codeTypeId: number;
        code: string;
        nameAr: string;
        nameEn: string;
        descriptionAr: string;
        descriptionEn: string;
        codeAttributeTypeId: number;
        isActive: boolean;
        createdAt: string;
        createdBy: string;
    }[];
}

@Injectable({
    providedIn: 'root'
})
export class CodeAttributeMainService {
    private apiUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    createCodeAttributeMain(data: CodeAttributeMainRequest): Observable<CodeAttributeMainResponse> {
        return this.http.post<CodeAttributeMainResponse>(
            `${this.apiUrl}/CodeAttributeMains/Create`,
            data
        );
    }

    createCodeAttributeMainsBulk(data: CodeAttributeMainRequest[]): Observable<CodeAttributeMainBulkResponse> {
        return this.http.post<CodeAttributeMainBulkResponse>(
            `${this.apiUrl}/CodeAttributeMains/CreateBulk`,
            data
        );
    }
}
