import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeAttributeDetailService } from '../../core/services/code-attribute-detail.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';

@Component({
    selector: 'app-code-details',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule],
    templateUrl: './code-details.component.html',
    styleUrl: './code-details.component.css'
})
export class CodeDetailsComponent implements OnInit {
    detailForm!: FormGroup;
    isLoading = false;
    errorMessage = '';
    successMessage = '';

    currentEntry = 0;
    totalEntries = 3;
    savedIds: number[] = [];
    savedCodes: string[] = [];

    attributeMainIds: number[] = [];

    constructor(
        private fb: FormBuilder,
        private codeAttributeDetailService: CodeAttributeDetailService,
        private codeGeneratorService: CodeGeneratorService,
        private router: Router
    ) { }

    ngOnInit() {
        const state = this.codeGeneratorService.getState();
        this.attributeMainIds = state.codeAttributeMainIds;

        if (this.attributeMainIds.length !== 3) {
            this.errorMessage = 'Missing required data. Please complete previous steps.';
            return;
        }

        this.initForm();
    }

    initForm() {
        this.detailForm = this.fb.group({
            code: ['', [Validators.required, Validators.minLength(2)]],
            nameAr: ['', [Validators.required]],
            nameEn: ['', [Validators.required]],
            descriptionAr: ['', [Validators.required]],
            descriptionEn: ['', [Validators.required]],
            sortOrder: [this.currentEntry + 1, [Validators.required]]
        });
    }

    onSubmit() {
        if (this.detailForm.valid && !this.isLoading) {
            this.isLoading = true;
            this.errorMessage = '';
            this.successMessage = '';

            const requestData = {
                ...this.detailForm.value,
                attributeMainId: this.attributeMainIds[this.currentEntry]
            };

            this.codeAttributeDetailService.createCodeAttributeDetail(requestData).subscribe({
                next: (response) => {
                    this.savedIds.push(response.data.id);
                    this.savedCodes.push(response.data.code);
                    this.codeGeneratorService.addCodeAttributeDetailId(response.data.id);

                    if (this.currentEntry < this.totalEntries - 1) {
                        this.currentEntry++;
                        this.successMessage = `Detail ${this.currentEntry}/${this.totalEntries} saved!`;
                        this.detailForm.reset();
                        this.detailForm.patchValue({ sortOrder: this.currentEntry + 1 });
                        this.isLoading = false;
                    } else {
                        this.successMessage = 'All 3 details saved successfully!';
                        this.codeGeneratorService.completeStep(3);

                        setTimeout(() => {
                            this.router.navigate(['/code-settings']);
                        }, 1000);
                    }
                },
                error: (error) => {
                    this.isLoading = false;
                    this.errorMessage = error.error?.message || 'Failed to create code attribute detail. Please try again.';
                }
            });
        } else {
            Object.keys(this.detailForm.controls).forEach(key => {
                this.detailForm.get(key)?.markAsTouched();
            });
        }
    }

    get code() { return this.detailForm.get('code'); }
    get nameAr() { return this.detailForm.get('nameAr'); }
    get nameEn() { return this.detailForm.get('nameEn'); }
    get descriptionAr() { return this.detailForm.get('descriptionAr'); }
    get descriptionEn() { return this.detailForm.get('descriptionEn'); }
    get sortOrder() { return this.detailForm.get('sortOrder'); }
}
