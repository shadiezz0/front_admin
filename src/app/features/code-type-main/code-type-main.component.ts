import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeAttributeMainService } from '../../core/services/code-attribute-main.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';

@Component({
    selector: 'app-code-type-main',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule],
    templateUrl: './code-type-main.component.html',
    styleUrl: './code-type-main.component.css'
})
export class CodeTypeMainComponent implements OnInit {
    mainForm!: FormGroup;
    isLoading = false;
    errorMessage = '';
    successMessage = '';

    currentEntry = 0;
    totalEntries = 3;
    savedIds: number[] = [];

    codeTypeId!: number;
    codeAttributeTypeIds: number[] = [];

    constructor(
        private fb: FormBuilder,
        private codeAttributeMainService: CodeAttributeMainService,
        private codeGeneratorService: CodeGeneratorService,
        private router: Router
    ) { }

    ngOnInit() {
        const state = this.codeGeneratorService.getState();
        this.codeTypeId = state.codeTypeId!;
        this.codeAttributeTypeIds = state.codeAttributeTypeIds;

        if (!this.codeTypeId || this.codeAttributeTypeIds.length !== 3) {
            this.errorMessage = 'Missing required data. Please complete previous steps.';
            return;
        }

        this.initForm();
    }

    initForm() {
        this.mainForm = this.fb.group({
            code: ['', [Validators.required, Validators.minLength(2)]],
            nameAr: ['', [Validators.required]],
            nameEn: ['', [Validators.required]],
            descriptionAr: ['', [Validators.required]],
            descriptionEn: ['', [Validators.required]]
        });
    }

    onSubmit() {
        if (this.mainForm.valid && !this.isLoading) {
            this.isLoading = true;
            this.errorMessage = '';
            this.successMessage = '';

            const requestData = {
                ...this.mainForm.value,
                codeTypeId: this.codeTypeId,
                codeAttributeTypeId: this.codeAttributeTypeIds[this.currentEntry]
            };

            this.codeAttributeMainService.createCodeAttributeMain(requestData).subscribe({
                next: (response) => {
                    this.savedIds.push(response.data.id);
                    this.codeGeneratorService.addCodeAttributeMainId(response.data.id);

                    if (this.currentEntry < this.totalEntries - 1) {
                        this.currentEntry++;
                        this.successMessage = `Main ${this.currentEntry}/${this.totalEntries} saved!`;
                        this.mainForm.reset();
                        this.isLoading = false;
                    } else {
                        this.successMessage = 'All 3 mains saved successfully!';
                        this.codeGeneratorService.completeStep(2);

                        setTimeout(() => {
                            this.router.navigate(['/code-details']);
                        }, 1000);
                    }
                },
                error: (error) => {
                    this.isLoading = false;
                    this.errorMessage = error.error?.message || 'Failed to create code attribute main. Please try again.';
                }
            });
        } else {
            Object.keys(this.mainForm.controls).forEach(key => {
                this.mainForm.get(key)?.markAsTouched();
            });
        }
    }

    get code() { return this.mainForm.get('code'); }
    get nameAr() { return this.mainForm.get('nameAr'); }
    get nameEn() { return this.mainForm.get('nameEn'); }
    get descriptionAr() { return this.mainForm.get('descriptionAr'); }
    get descriptionEn() { return this.mainForm.get('descriptionEn'); }
}
