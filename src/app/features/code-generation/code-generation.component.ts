import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CodeGenerationService } from '../../core/services/code-generation.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';
import { AlertComponent } from '../../shared/components/alert/alert.component';
import { ButtonComponent } from '../../shared/components/button/button.component';

@Component({
    selector: 'app-code-generation',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule, AlertComponent, ButtonComponent],
    templateUrl: './code-generation.component.html',
    styleUrl: './code-generation.component.css'
})
export class CodeGenerationComponent implements OnInit {
    generationForm!: FormGroup;
    isLoading = false;
    errorMessage = '';

    generatedCode = '';
    showCodeReveal = false;
    copiedToClipboard = false;

    codeTypeId!: number;

    isSidebarCollapsed = false;

    constructor(
        private fb: FormBuilder,
        private codeGenerationService: CodeGenerationService,
        private codeGeneratorService: CodeGeneratorService
    ) { }

    ngOnInit() {
        const state = this.codeGeneratorService.getState();
        this.codeTypeId = state.codeTypeId!;

        if (!this.codeTypeId) {
            this.errorMessage = 'Missing required data. Please complete previous steps.';
            return;
        }

        this.initForm();
    }

    initForm() {
        this.generationForm = this.fb.group({
            nameAr: ['', [Validators.required]],
            nameEn: ['', [Validators.required]],
            descriptionAr: ['', [Validators.required]],
            descriptionEn: ['', [Validators.required]],
            externalSystem: ['', [Validators.required]],
            externalReferenceId: ['', [Validators.required]]
        });
    }

    onGenerate() {
        if (this.generationForm.valid && !this.isLoading) {
            this.isLoading = true;
            this.errorMessage = '';
            this.showCodeReveal = false;

            const requestData = {
                ...this.generationForm.value,
                codeTypeId: this.codeTypeId
            };

            this.codeGenerationService.generateCode(requestData).subscribe({
                next: (response) => {
                    this.generatedCode = response.data.codeGenerated;
                    this.isLoading = false;

                    // Trigger animated reveal
                    setTimeout(() => {
                        this.showCodeReveal = true;
                        this.codeGeneratorService.completeStep(6);
                    }, 500);
                },
                error: (error) => {
                    this.isLoading = false;
                    this.errorMessage = error.error?.message || 'Failed to generate code. Please try again.';
                }
            });
        } else {
            Object.keys(this.generationForm.controls).forEach(key => {
                this.generationForm.get(key)?.markAsTouched();
            });
        }
    }

    copyToClipboard() {
        navigator.clipboard.writeText(this.generatedCode).then(() => {
            this.copiedToClipboard = true;
            setTimeout(() => {
                this.copiedToClipboard = false;
            }, 2000);
        });
    }

    toggleSidebar(): void {
        this.isSidebarCollapsed = !this.isSidebarCollapsed;
    }

    get nameAr() { return this.generationForm.get('nameAr'); }
    get nameEn() { return this.generationForm.get('nameEn'); }
    get descriptionAr() { return this.generationForm.get('descriptionAr'); }
    get descriptionEn() { return this.generationForm.get('descriptionEn'); }
    get externalSystem() { return this.generationForm.get('externalSystem'); }
    get externalReferenceId() { return this.generationForm.get('externalReferenceId'); }
}
