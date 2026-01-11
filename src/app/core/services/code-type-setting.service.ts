import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface CodeTypeSettingRequest {
    codeTypeId: number;
    attributeDetailId: number;
    sortOrder: number;
    separator: string;
    isRequired: boolean;
}

export interface CodeTypeSettingResponse {
    statusCode: number;
    message: string;
    data: {
        id: number;
        codeTypeId: number;
        attributeDetailId: number;
        sortOrder: number;
        separator: string;
        isRequired: boolean;
        createdAt: string;
    };
}

@Injectable({
    providedIn: 'root'
})
export class CodeTypeSettingService {
    private apiUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    createCodeTypeSetting(data: CodeTypeSettingRequest): Observable<CodeTypeSettingResponse> {
        return this.http.post<CodeTypeSettingResponse>(
            `${this.apiUrl}/CodeTypeSettings/Create`,
            data
        );
    }
}
