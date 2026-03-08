import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CodeTypeSettingService } from '../../core/services/code-type-setting.service';
import { CodeTypeService } from '../../core/services/code-type.service';
import { CodeAttributeDetailService, CodeAttributeDetailItem } from '../../core/services/code-attribute-detail.service';


interface SavedSetting {
    id: number;
    codeTypeLabel: string;
    detailLabel: string;
    separator: string;
    sortOrder: number;
    isRequired: boolean;
}

import { AlertComponent } from '../../shared/components/alert/alert.component';
import { Router } from '@angular/router';

@Component({
    selector: 'app-code-settings',
    standalone: true,
    imports: [CommonModule, FormsModule, AlertComponent],
    templateUrl: './code-settings.component.html',
    styleUrl: './code-settings.component.css'
})
export class CodeSettingsComponent implements OnInit {

    // Dropdown data
    codeTypes: any[] = [];
    codeAttributeDetails: CodeAttributeDetailItem[] = [];
    isLoadingData = true;
    private _loadTarget = 2;

    // Selections
    selectedCodeTypeId: number | null = null;
    selectedAttributeDetailId: number | null = null;
    separator = '-';
    sortOrder = 1;
    isRequired = true;

    // State
    isSaving = false;
    errorMessage = '';
    successMessage = '';

    // Session log of successfully saved settings
    savedSettings: SavedSetting[] = [];

    constructor(
        private codeTypeSettingService: CodeTypeSettingService,
        private codeTypeService: CodeTypeService,
        private codeAttributeDetailService: CodeAttributeDetailService,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.loadDropdownData();
    }

    loadDropdownData(): void {
        this.isLoadingData = true;
        this._loadCount = 0;

        // Safety fallback — dismiss skeleton after 10 s even if APIs hang
        const timeout = setTimeout(() => { this.isLoadingData = false; }, 10000);

        this.codeTypeService.getAllCodeTypes().subscribe({
            next: (res) => { this.codeTypes = res.data ?? []; this.checkLoadDone(timeout); },
            error: () => { this.checkLoadDone(timeout); }
        });

        this.codeAttributeDetailService.getAllCodeAttributeDetails().subscribe({
            next: (res) => { this.codeAttributeDetails = res.data ?? []; this.checkLoadDone(timeout); },
            error: () => { this.checkLoadDone(timeout); }
        });
    }

    private _loadCount = 0;
    private checkLoadDone(timeout?: ReturnType<typeof setTimeout>): void {
        this._loadCount++;
        if (this._loadCount >= this._loadTarget) {
            this.isLoadingData = false;
            if (timeout) clearTimeout(timeout);
        }
    }

    // Called when 'Add Setting & Continue' is clicked
    onSave(): void {
        if (!this.selectedCodeTypeId || !this.selectedAttributeDetailId) {
            this.errorMessage = 'Please select both Code Type and Attribute Detail before continuing.';
            setTimeout(() => this.errorMessage = '', 4000);
            return;
        }
        this.saveSettingNow();
    }

    saveSettingNow(): void {
        if (!this.selectedCodeTypeId || !this.selectedAttributeDetailId) return;

        this.isSaving = true;
        this.errorMessage = '';
        this.successMessage = '';

        const payload = {
            codeTypeId: +this.selectedCodeTypeId,
            attributeDetailId: +this.selectedAttributeDetailId,
            sortOrder: this.sortOrder,
            separator: this.separator,
            isRequired: this.isRequired
        };

        this.codeTypeSettingService.createCodeTypeSetting(payload).subscribe({
            next: (res) => {
                this.isSaving = false;
                this.savedSettings.unshift({
                    id: res.data.id,
                    codeTypeLabel: this.getCodeTypeLabel(+this.selectedCodeTypeId!),
                    detailLabel: this.getDetailLabel(+this.selectedAttributeDetailId!),
                    separator: res.data.separator,
                    sortOrder: res.data.sortOrder,
                    isRequired: res.data.isRequired
                });

                this.successMessage = `✓ Setting saved (ID: ${res.data.id})`;
                this.sortOrder = this.savedSettings.length + 1;

                this.selectedCodeTypeId = null;
                this.selectedAttributeDetailId = null;

                // Automatically navigate to the next step (Sequence) after successful save
                setTimeout(() => {
                    this.router.navigate(['/code-sequence']);
                }, 1000);
            },
            error: (err) => {
                this.isSaving = false;
                if (err.status === 409) {
                    this.successMessage = 'Setting already exists.';
                    this.selectedCodeTypeId = null;
                    this.selectedAttributeDetailId = null;
                    // Still navigate — the setting exists so we can proceed
                    setTimeout(() => {
                        this.router.navigate(['/code-sequence']);
                    }, 1000);
                } else {
                    this.errorMessage = err.error?.message || 'Failed to save setting.';
                }
            }
        });
    }

    continueToSequence(): void {
        this.router.navigate(['/code-sequence']);
    }

    removeSavedSetting(index: number): void {
        this.savedSettings.splice(index, 1);
    }

    getCodeTypeLabel(id: number): string {
        const ct = this.codeTypes.find(c => c.id === id);
        return ct ? `${ct.codeTypeCode} — ${ct.nameEn}` : `ID:${id}`;
    }

    getDetailLabel(id: number): string {
        const d = this.codeAttributeDetails.find(x => x.id === id);
        return d ? `${d.code} — ${d.nameEn}` : `ID:${id}`;
    }
}
