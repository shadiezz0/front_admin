import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeAttributeTypeService } from '../../core/services/code-attribute-type.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';
import { AlertComponent } from '../../shared/components/alert/alert.component';

@Component({
    selector: 'app-code-type-attribute',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule, AlertComponent],
    templateUrl: './code-type-attribute.component.html',
    styleUrl: './code-type-attribute.component.css'
})
export class CodeTypeAttributeComponent implements OnInit {
    attributeForm!: FormGroup;
    isLoading = false;
    errorMessage = '';
    successMessage = '';

    attributes: any[] = [];
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
        if (this.attributeForm.valid) {
            this.addAttribute();
        } else {
            Object.keys(this.attributeForm.controls).forEach(key => {
                this.attributeForm.get(key)?.markAsTouched();
            });
        }
    }

    addAttribute() {
        if (this.attributeForm.valid) {
            this.attributes.push({ ...this.attributeForm.value });
            this.successMessage = `Attribute added! Total: ${this.attributes.length}`;
            this.attributeForm.reset();
            this.errorMessage = '';
            
            setTimeout(() => {
                this.successMessage = '';
            }, 2000);
        }
    }

    removeAttribute(index: number) {
        this.attributes.splice(index, 1);
        this.successMessage = `Attribute removed! Total: ${this.attributes.length}`;
        
        setTimeout(() => {
            this.successMessage = '';
        }, 2000);
    }

    saveAllAttributes() {
        if (this.attributes.length === 0) {
            this.errorMessage = 'Please add at least one attribute before saving.';
            return;
        }

        this.isLoading = true;
        this.errorMessage = '';
        this.successMessage = '';

        let completedCount = 0;
        const totalCount = this.attributes.length;

        this.attributes.forEach((attribute, index) => {
            this.codeAttributeTypeService.createCodeAttributeType(attribute).subscribe({
                next: (response) => {
                    this.savedIds.push(response.data.id);
                    this.codeGeneratorService.addCodeAttributeTypeId(response.data.id);
                    completedCount++;

                    if (completedCount === totalCount) {
                        this.isLoading = false;
                        this.successMessage = `All ${totalCount} attributes saved successfully!`;
                        this.codeGeneratorService.completeStep(1);
                        
                        setTimeout(() => {
                            this.router.navigate(['/code-type-main']);
                        }, 1500);
                    }
                },
                error: (error) => {
                    this.isLoading = false;
                    this.errorMessage = error.error?.message || `Failed to save attribute ${index + 1}. Please try again.`;
                }
            });
        });
    }



    get nameAr() { return this.attributeForm.get('nameAr'); }
    get nameEn() { return this.attributeForm.get('nameEn'); }
    get descriptionAr() { return this.attributeForm.get('descriptionAr'); }
    get descriptionEn() { return this.attributeForm.get('descriptionEn'); }
}
