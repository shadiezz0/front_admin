import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeAttributeMainService } from '../../core/services/code-attribute-main.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';
import { HeaderComponent } from '../../shared/components/header/header.component';
import { FooterComponent } from '../../shared/components/footer/footer.component';
import { SidebarComponent } from '../../shared/components/sidebar/sidebar.component';

@Component({
    selector: 'app-code-type-main',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule, HeaderComponent, SidebarComponent, FooterComponent],
    templateUrl: './code-type-main.component.html',
    styleUrl: './code-type-main.component.css'
})
export class CodeTypeMainComponent implements OnInit {
    mainForm!: FormGroup;
    isLoading = false;
    errorMessage = '';
    successMessage = '';

    mains: any[] = [];
    savedIds: number[] = [];

    codeTypeId!: number;
    codeAttributeTypeIds: number[] = [];

    isSidebarCollapsed = false;

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

        if (!this.codeTypeId || this.codeAttributeTypeIds.length === 0) {
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
        if (this.mainForm.valid) {
            this.addMain();
        } else {
            Object.keys(this.mainForm.controls).forEach(key => {
                this.mainForm.get(key)?.markAsTouched();
            });
        }
    }

    addMain() {
        if (this.mainForm.valid) {
            if (this.mains.length >= this.codeAttributeTypeIds.length) {
                this.errorMessage = `You can only add ${this.codeAttributeTypeIds.length} main entries (one for each attribute type).`;
                return;
            }

            this.mains.push({ ...this.mainForm.value });
            this.successMessage = `Main added! Total: ${this.mains.length}`;
            this.mainForm.reset();
            this.errorMessage = '';
            
            setTimeout(() => {
                this.successMessage = '';
            }, 2000);
        }
    }

    removeMain(index: number) {
        this.mains.splice(index, 1);
        this.successMessage = `Main removed! Total: ${this.mains.length}`;
        
        setTimeout(() => {
            this.successMessage = '';
        }, 2000);
    }

    saveAllMains() {
        if (this.mains.length === 0) {
            this.errorMessage = 'Please add at least one main entry before saving.';
            return;
        }

        if (this.mains.length !== this.codeAttributeTypeIds.length) {
            this.errorMessage = `You must add exactly ${this.codeAttributeTypeIds.length} main entries (one for each attribute type).`;
            return;
        }

        this.isLoading = true;
        this.errorMessage = '';
        this.successMessage = '';

        let completedCount = 0;
        const totalCount = this.mains.length;

        this.mains.forEach((main, index) => {
            const requestData = {
                ...main,
                codeTypeId: this.codeTypeId,
                codeAttributeTypeId: this.codeAttributeTypeIds[index]
            };

            this.codeAttributeMainService.createCodeAttributeMain(requestData).subscribe({
                next: (response) => {
                    this.savedIds.push(response.data.id);
                    this.codeGeneratorService.addCodeAttributeMainId(response.data.id);
                    completedCount++;

                    if (completedCount === totalCount) {
                        this.isLoading = false;
                        this.successMessage = `All ${totalCount} mains saved successfully!`;
                        this.codeGeneratorService.completeStep(2);
                        
                        setTimeout(() => {
                            this.router.navigate(['/code-details']);
                        }, 1500);
                    }
                },
                error: (error) => {
                    this.isLoading = false;
                    this.errorMessage = error.error?.message || `Failed to save main ${index + 1}. Please try again.`;
                }
            });
        });
    }

    toggleSidebar(): void {
        this.isSidebarCollapsed = !this.isSidebarCollapsed;
    }

    get code() { return this.mainForm.get('code'); }
    get nameAr() { return this.mainForm.get('nameAr'); }
    get nameEn() { return this.mainForm.get('nameEn'); }
    get descriptionAr() { return this.mainForm.get('descriptionAr'); }
    get descriptionEn() { return this.mainForm.get('descriptionEn'); }
}
