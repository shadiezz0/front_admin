import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CodeTypeSettingService } from '../../core/services/code-type-setting.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';
import { forkJoin } from 'rxjs';
import { HeaderComponent } from '../../shared/components/header/header.component';
import { FooterComponent } from '../../shared/components/footer/footer.component';
import { SidebarComponent } from '../../shared/components/sidebar/sidebar.component';

interface SettingDisplay {
    id?: number;
    code: string;
    sortOrder: number;
    separator: string;
    isRequired: boolean;
}

@Component({
    selector: 'app-code-settings',
    standalone: true,
    imports: [CommonModule, HeaderComponent, SidebarComponent, FooterComponent],
    templateUrl: './code-settings.component.html',
    styleUrl: './code-settings.component.css'
})
export class CodeSettingsComponent implements OnInit {
    isLoading = false;
    errorMessage = '';
    successMessage = '';

    settings = signal<SettingDisplay[]>([]);
    generatedPattern = signal('');

    codeTypeId!: number;
    detailIds: number[] = [];
    savedCodes: string[] = [];

    isSidebarCollapsed = false;

    constructor(
        private codeTypeSettingService: CodeTypeSettingService,
        private codeGeneratorService: CodeGeneratorService,
        private router: Router
    ) { }

    ngOnInit() {
        const state = this.codeGeneratorService.getState();
        this.codeTypeId = state.codeTypeId!;
        this.detailIds = state.codeAttributeDetailIds;

        if (!this.codeTypeId || this.detailIds.length !== 3) {
            this.errorMessage = 'Missing required data. Please complete previous steps.';
            return;
        }

        // Get saved codes from localStorage or from previous component
        // In a real app, you'd fetch these from the API based on detail IDs
        // For now, we'll use the codes that were saved during Code Details step
        this.savedCodes = this.getSavedCodesFromDetails();

        this.createSettings();
    }

    getSavedCodesFromDetails(): string[] {
        // In production, fetch from API using detailIds
        // For now, return placeholder codes in order
        // You would call an API endpoint like: GET /CodeAttributeDetails/{id}
        return ['XYZ', 'ABC', 'VBV'];
    }

    createSettings() {
        this.isLoading = true;
        const settingRequests = this.detailIds.map((detailId, index) => {
            return this.codeTypeSettingService.createCodeTypeSetting({
                codeTypeId: this.codeTypeId,
                attributeDetailId: detailId,
                sortOrder: index + 1,
                separator: '-',
                isRequired: true
            });
        });

        forkJoin(settingRequests).subscribe({
            next: (responses) => {
                const settingsData: SettingDisplay[] = responses.map((r, i) => ({
                    id: r.data.id,
                    code: this.savedCodes[i],
                    sortOrder: r.data.sortOrder,
                    separator: r.data.separator,
                    isRequired: r.data.isRequired
                }));

                this.settings.set(settingsData);
                this.generatedPattern.set(this.savedCodes.join('-'));
                this.isLoading = false;
                this.successMessage = 'Settings configured successfully!';
            },
            error: (error) => {
                this.isLoading = false;

                // Handle 409 Conflict - settings already exist
                if (error.status === 409) {
                    // Settings already exist, just display them
                    const settingsData: SettingDisplay[] = this.detailIds.map((detailId, i) => ({
                        code: this.savedCodes[i],
                        sortOrder: i + 1,
                        separator: '-',
                        isRequired: true
                    }));

                    this.settings.set(settingsData);
                    this.generatedPattern.set(this.savedCodes.join('-'));
                    this.successMessage = 'Settings loaded successfully!';
                } else {
                    this.errorMessage = error.error?.message || 'Failed to create settings. Please try again.';
                }
            }
        });
    }

    onSave() {
        this.codeGeneratorService.completeStep(4);
        this.router.navigate(['/code-sequence']);
    }

    toggleSidebar(): void {
        this.isSidebarCollapsed = !this.isSidebarCollapsed;
    }
}
