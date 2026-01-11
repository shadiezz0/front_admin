import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeTypeService } from '../../core/services/code-type.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';

@Component({
    selector: 'app-code-type',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule],
    templateUrl: './code-type.component.html',
    styleUrl: './code-type.component.css'
})
export class CodeTypeComponent implements OnInit {
    codeTypeForm!: FormGroup;
    isLoading = false;
    errorMessage = '';
    successMessage = '';

    constructor(
        private fb: FormBuilder,
        private codeTypeService: CodeTypeService,
        private codeGeneratorService: CodeGeneratorService,
        private router: Router
    ) { }

    ngOnInit() {
        this.codeTypeForm = this.fb.group({
            codeTypeCode: ['', [Validators.required, Validators.minLength(2)]],
            nameAr: ['', [Validators.required]],
            nameEn: ['', [Validators.required]],
            descriptionAr: ['', [Validators.required]],
            descriptionEn: ['', [Validators.required]]
        });
    }

    onSubmit() {
        if (this.codeTypeForm.valid && !this.isLoading) {
            this.isLoading = true;
            this.errorMessage = '';
            this.successMessage = '';

            this.codeTypeService.createCodeType(this.codeTypeForm.value).subscribe({
                next: (response) => {
                    this.successMessage = response.message;
                    this.codeGeneratorService.setCodeTypeData(response.data.id, response.data.codeTypeCode);
                    this.codeGeneratorService.completeStep(0); // Mark Code Type as completed

                    // Navigate to next step after short delay
                    setTimeout(() => {
                        this.router.navigate(['/code-type-attribute']);
                    }, 500);
                },
                error: (error) => {
                    this.isLoading = false;
                    this.errorMessage = error.error?.message || 'Failed to create code type. Please try again.';
                }
            });
        } else {
            Object.keys(this.codeTypeForm.controls).forEach(key => {
                this.codeTypeForm.get(key)?.markAsTouched();
            });
        }
    }

    get codeTypeCode() { return this.codeTypeForm.get('codeTypeCode'); }
    get nameAr() { return this.codeTypeForm.get('nameAr'); }
    get nameEn() { return this.codeTypeForm.get('nameEn'); }
    get descriptionAr() { return this.codeTypeForm.get('descriptionAr'); }
    get descriptionEn() { return this.codeTypeForm.get('descriptionEn'); }
}
