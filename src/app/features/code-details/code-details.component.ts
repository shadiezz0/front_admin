import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeAttributeDetailService } from '../../core/services/code-attribute-detail.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';
import { AlertComponent } from '../../shared/components/alert/alert.component';

@Component({
    selector: 'app-code-details',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule, AlertComponent],
    templateUrl: './code-details.component.html',
    styleUrl: './code-details.component.css'
})
export class CodeDetailsComponent implements OnInit {
    detailForm!: FormGroup;
    isLoading = false;
    errorMessage = '';
    successMessage = '';

    details: any[] = [];
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

        if (this.attributeMainIds.length === 0) {
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
            sortOrder: [this.details.length + 1, [Validators.required]]
        });
    }

    onSubmit() {
        if (this.detailForm.valid) {
            this.addDetail();
        } else {
            Object.keys(this.detailForm.controls).forEach(key => {
                this.detailForm.get(key)?.markAsTouched();
            });
        }
    }

    addDetail() {
        if (this.detailForm.valid) {
            if (this.details.length >= this.attributeMainIds.length) {
                this.errorMessage = `You can only add ${this.attributeMainIds.length} detail entries (one for each main entry).`;
                return;
            }

            this.details.push({ ...this.detailForm.value });
            this.successMessage = `Detail added! Total: ${this.details.length}`;
            this.detailForm.reset();
            this.detailForm.patchValue({ sortOrder: this.details.length + 1 });
            this.errorMessage = '';
            
            setTimeout(() => {
                this.successMessage = '';
            }, 2000);
        }
    }

    removeDetail(index: number) {
        this.details.splice(index, 1);
        // Update sort order for remaining items
        this.details.forEach((detail, idx) => {
            detail.sortOrder = idx + 1;
        });
        this.detailForm.patchValue({ sortOrder: this.details.length + 1 });
        this.successMessage = `Detail removed! Total: ${this.details.length}`;
        
        setTimeout(() => {
            this.successMessage = '';
        }, 2000);
    }

    saveAllDetails() {
        if (this.details.length === 0) {
            this.errorMessage = 'Please add at least one detail entry before saving.';
            return;
        }

        if (this.details.length !== this.attributeMainIds.length) {
            this.errorMessage = `You must add exactly ${this.attributeMainIds.length} detail entries (one for each main entry).`;
            return;
        }

        this.isLoading = true;
        this.errorMessage = '';
        this.successMessage = '';

        let completedCount = 0;
        const totalCount = this.details.length;

        this.details.forEach((detail, index) => {
            const requestData = {
                ...detail,
                attributeMainId: this.attributeMainIds[index]
            };

            this.codeAttributeDetailService.createCodeAttributeDetail(requestData).subscribe({
                next: (response) => {
                    this.savedIds.push(response.data.id);
                    this.savedCodes.push(response.data.code);
                    this.codeGeneratorService.addCodeAttributeDetailId(response.data.id);
                    completedCount++;

                    if (completedCount === totalCount) {
                        this.isLoading = false;
                        this.successMessage = `All ${totalCount} details saved successfully!`;
                        this.codeGeneratorService.completeStep(3);
                        
                        setTimeout(() => {
                            this.router.navigate(['/code-settings']);
                        }, 1500);
                    }
                },
                error: (error) => {
                    this.isLoading = false;
                    this.errorMessage = error.error?.message || `Failed to save detail ${index + 1}. Please try again.`;
                }
            });
        });
    }

    get code() { return this.detailForm.get('code'); }
    get nameAr() { return this.detailForm.get('nameAr'); }
    get nameEn() { return this.detailForm.get('nameEn'); }
    get descriptionAr() { return this.detailForm.get('descriptionAr'); }
    get descriptionEn() { return this.detailForm.get('descriptionEn'); }
    get sortOrder() { return this.detailForm.get('sortOrder'); }
}
