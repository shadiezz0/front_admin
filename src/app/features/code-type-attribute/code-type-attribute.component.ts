import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeAttributeTypeService } from '../../core/services/code-attribute-type.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';

@Component({
    selector: 'app-code-type-attribute',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule],
    templateUrl: './code-type-attribute.component.html',
    styleUrl: './code-type-attribute.component.css'
})
export class CodeTypeAttributeComponent implements OnInit {
    attributeForm!: FormGroup;
    isLoading = false;
    errorMessage = '';
    successMessage = '';

    currentEntry = 0;
    totalEntries = 3;
    savedIds: number[] = [];

    constructor(
        private fb: FormBuilder,
        private codeAttributeTypeService: CodeAttributeTypeService,
        private codeGeneratorService: CodeGeneratorService,
        private router: Router
    ) { }

    ngOnInit() {
        this.initForm();
    }

    initForm() {
        this.attributeForm = this.fb.group({
            nameAr: ['', [Validators.required]],
            nameEn: ['', [Validators.required]],
            descriptionAr: ['', [Validators.required]],
            descriptionEn: ['', [Validators.required]]
        });
    }

    onSubmit() {
        if (this.attributeForm.valid && !this.isLoading) {
            this.isLoading = true;
            this.errorMessage = '';
            this.successMessage = '';

            this.codeAttributeTypeService.createCodeAttributeType(this.attributeForm.value).subscribe({
                next: (response) => {
                    this.savedIds.push(response.data.id);
                    this.codeGeneratorService.addCodeAttributeTypeId(response.data.id);

                    if (this.currentEntry < this.totalEntries - 1) {
                        this.currentEntry++;
                        this.successMessage = `Attribute ${this.currentEntry}/${this.totalEntries} saved!`;
                        this.attributeForm.reset();
                        this.isLoading = false;
                    } else {
                        this.successMessage = 'All 3 attributes saved successfully!';
                        this.codeGeneratorService.completeStep(1);

                        setTimeout(() => {
                            this.router.navigate(['/code-type-main']);
                        }, 1000);
                    }
                },
                error: (error) => {
                    this.isLoading = false;
                    this.errorMessage = error.error?.message || 'Failed to create code attribute type. Please try again.';
                }
            });
        } else {
            Object.keys(this.attributeForm.controls).forEach(key => {
                this.attributeForm.get(key)?.markAsTouched();
            });
        }
    }

    get nameAr() { return this.attributeForm.get('nameAr'); }
    get nameEn() { return this.attributeForm.get('nameEn'); }
    get descriptionAr() { return this.attributeForm.get('descriptionAr'); }
    get descriptionEn() { return this.attributeForm.get('descriptionEn'); }
}
