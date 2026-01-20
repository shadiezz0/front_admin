import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeTypeSettingService } from '../../core/services/code-type-setting.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';
import { CodeTypeService } from '../../core/services/code-type.service';
import { CodeAttributeDetailService } from '../../core/services/code-attribute-detail.service';
import { forkJoin } from 'rxjs';

interface SettingDisplay {
    id?: number;
    code: string;
    codeTypeId: number;
    attributeDetailId: number;
    sortOrder: number;
    separator: string;
    isRequired: boolean;
}

@Component({
    selector: 'app-code-settings',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule],
    templateUrl: './code-settings.component.html',
    styleUrl: './code-settings.component.css'
})
export class CodeSettingsComponent implements OnInit {
    settings: SettingDisplay[] = [];
    savedSettingIds: number[] = [];

    isLoading = false;
    errorMessage = '';
    successMessage = '';

    // Dropdown data
    codeTypes: any[] = [];
    codeAttributeDetails: any[] = [];

    // Selected filters
    selectedCodeTypeId: number | null = null;
    selectedAttributeDetailId: number | null = null;

    codeTypeId!: number;
    detailIds: number[] = [];
    savedCodes: string[] = [];

    constructor(
        private fb: FormBuilder,
        private codeTypeSettingService: CodeTypeSettingService,
        private codeGeneratorService: CodeGeneratorService,
        private codeTypeService: CodeTypeService,
        private codeAttributeDetailService: CodeAttributeDetailService,
        private router: Router
    ) { }

    ngOnInit() {
        const state = this.codeGeneratorService.getState();
        this.codeTypeId = state.codeTypeId!;
        this.detailIds = state.codeAttributeDetailIds;

        if (!this.codeTypeId || this.detailIds.length === 0) {
            this.errorMessage = 'Missing required data. Please complete previous steps.';
            return;
        }

        // Get saved codes from the previous component (Code Details step)
        this.savedCodes = this.getSavedCodesFromDetails();

        this.loadDropdownData();
        this.autoFillSettings();
    }

    loadDropdownData() {
        // Load Code Types
        this.codeTypeService.getAllCodeTypes().subscribe({
            next: (response) => {
                this.codeTypes = response.data;
            },
            error: (error) => {
                console.error('Error loading code types:', error);
            }
        });

        // Load Code Attribute Details
        this.codeAttributeDetailService.getAllCodeAttributeDetails().subscribe({
            next: (response) => {
                this.codeAttributeDetails = response.data;
            },
            error: (error) => {
                console.error('Error loading code attribute details:', error);
            }
        });
    }

    getSavedCodesFromDetails(): string[] {
        // In production, fetch from API using detailIds
        // For now, return placeholder codes in order based on detail IDs
        // You would call an API endpoint like: GET /CodeAttributeDetails/{id}
        return this.detailIds.map((id, index) => `CODE${index + 1}`);
    }

    autoFillSettings() {
        // Auto-create settings from the details saved in previous step
        this.settings = this.detailIds.map((detailId, index) => ({
            code: this.savedCodes[index] || `CODE${index + 1}`,
            codeTypeId: this.codeTypeId,
            attributeDetailId: detailId,
            sortOrder: index + 1,
            separator: '-',
            isRequired: true
        }));

        this.successMessage = 'Settings loaded successfully!';
    }

    saveAllSettings() {
        if (this.settings.length === 0) {
            this.errorMessage = 'No settings to save.';
            return;
        }

        this.isLoading = true;
        this.errorMessage = '';
        this.successMessage = '';

        const settingRequests = this.settings.map((setting) => {
            return this.codeTypeSettingService.createCodeTypeSetting({
                codeTypeId: setting.codeTypeId,
                attributeDetailId: setting.attributeDetailId,
                sortOrder: setting.sortOrder,
                separator: setting.separator,
                isRequired: setting.isRequired
            });
        });

        forkJoin(settingRequests).subscribe({
            next: (responses) => {
                this.savedSettingIds = responses.map(r => r.data.id);
                this.isLoading = false;
                this.successMessage = `All ${responses.length} settings saved successfully!`;
                this.codeGeneratorService.completeStep(3);

                setTimeout(() => {
                    this.router.navigate(['/code-sequence']);
                }, 1500);
            },
            error: (error) => {
                this.isLoading = false;

                // Handle 409 Conflict - settings already exist
                if (error.status === 409) {
                    this.successMessage = 'Settings already exist and loaded successfully!';
                    this.codeGeneratorService.completeStep(3);

                    setTimeout(() => {
                        this.router.navigate(['/code-sequence']);
                    }, 1500);
                } else {
                    this.errorMessage = error.error?.message || 'Failed to save settings. Please try again.';
                }
            }
        });
    }

    getFilteredSettings(): SettingDisplay[] {
        let filtered = this.settings;

        if (this.selectedCodeTypeId) {
            filtered = filtered.filter(s => s.codeTypeId === this.selectedCodeTypeId);
        }

        if (this.selectedAttributeDetailId) {
            filtered = filtered.filter(s => s.attributeDetailId === this.selectedAttributeDetailId);
        }

        return filtered;
    }

    getCodeTypeLabel(id: number): string {
        const codeType = this.codeTypes.find(ct => ct.id === id);
        return codeType ? codeType.codeTypeCode : `ID: ${id}`;
    }

    getAttributeDetailLabel(id: number): string {
        const detail = this.codeAttributeDetails.find(ad => ad.id === id);
        return detail ? detail.code : `ID: ${id}`;
    }
}
