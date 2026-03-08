import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeTypeService } from '../../core/services/code-type.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';
import { AlertComponent } from '../../shared/components/alert/alert.component';

@Component({
    selector: 'app-code-type',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule, AlertComponent],
    templateUrl: './code-type.component.html',
    styleUrl: './code-type.component.css'
})
export class CodeTypeComponent implements OnInit {
    codeTypeForm!: FormGroup;
    isLoading = false;
    errorMessage = '';
    successMessage = '';
    codeTypes: any[] = [];

    constructor(
        private fb: FormBuilder,
        private codeTypeService: CodeTypeService,
        private codeGeneratorService: CodeGeneratorService,
        private router: Router
    ) { }

    ngOnInit() {
        this.codeTypeForm = this.fb.group({
            codeTypeCode: ['', [Validators.required, Validators.minLength(1)]],
            nameAr: [''],
            nameEn: ['', [Validators.required]],
            descriptionAr: [''],
            descriptionEn: ['']
        });
    }

    onSubmit() {
        if (this.codeTypeForm.valid && !this.isLoading) {
            this.addCodeType();
        } else {
            Object.keys(this.codeTypeForm.controls).forEach(key => {
                this.codeTypeForm.get(key)?.markAsTouched();
            });
        }
    }

    addCodeType() {
        this.codeTypes.push({ ...this.codeTypeForm.value });
        this.successMessage = `Code Type added to list! Total: ${this.codeTypes.length}`;
        this.codeTypeForm.reset();

        setTimeout(() => {
            this.successMessage = '';
        }, 2000);
    }

    removeCodeType(index: number) {
        this.codeTypes.splice(index, 1);
        this.successMessage = `Code Type removed from list!`;
        setTimeout(() => {
            this.successMessage = '';
        }, 2000);
    }

    saveAllCodeTypes() {
        if (this.codeTypes.length === 0) {
            this.errorMessage = 'Please add at least one code type before saving.';
            return;
        }

        this.isLoading = true;
        this.errorMessage = '';
        this.successMessage = '';

        let completedCount = 0;
        const totalCount = this.codeTypes.length;

        this.codeTypes.forEach((item, index) => {
            this.codeTypeService.createCodeType(item).subscribe({
                next: (response) => {
                    this.codeGeneratorService.setCodeTypeData(response.data.id, response.data.codeTypeCode);
                    completedCount++;

                    if (completedCount === totalCount) {
                        this.isLoading = false;
                        this.codeGeneratorService.completeStep(0);
                        this.successMessage = `All ${totalCount} code types saved successfully!`;

                        setTimeout(() => {
                            this.router.navigate(['/code-type-attribute']);
                        }, 1500);
                    }
                },
                error: (error) => {
                    this.isLoading = false;
                    this.errorMessage = error.error?.message || `Failed to save code type ${index + 1}.`;
                }
            });
        });
    }

    onCancel() {
        this.router.navigate(['/dashboard']);
    }

    get codeTypeCode() { return this.codeTypeForm.get('codeTypeCode'); }
    get nameAr() { return this.codeTypeForm.get('nameAr'); }
    get nameEn() { return this.codeTypeForm.get('nameEn'); }
    get descriptionAr() { return this.codeTypeForm.get('descriptionAr'); }
    get descriptionEn() { return this.codeTypeForm.get('descriptionEn'); }
}
