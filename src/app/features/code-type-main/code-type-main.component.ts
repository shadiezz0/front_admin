import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeAttributeMainService } from '../../core/services/code-attribute-main.service';
import { CodeAttributeDetailService } from '../../core/services/code-attribute-detail.service';
import { CodeGeneratorService } from '../../core/services/code-generator.service';
import { CodeTypeService } from '../../core/services/code-type.service';
import { CodeAttributeTypeService } from '../../core/services/code-attribute-type.service';

@Component({
    selector: 'app-code-type-main',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule],
    templateUrl: './code-type-main.component.html',
    styleUrl: './code-type-main.component.css'
})
export class CodeTypeMainComponent implements OnInit {
    mainForm!: FormGroup;
    mains: any[] = [];
    savedMainIds: number[] = [];

    detailForm!: FormGroup;
    details: any[] = [];
    savedDetailIds: number[] = [];

    isLoading = false;
    errorMessage = '';
    successMessage = '';
    codeTypeId!: number;
    codeAttributeTypeIds: number[] = [];
    isSidebarCollapsed = false;

    mainsSaved = false;

    // Dropdown data
    codeTypes: any[] = [];
    codeAttributeTypes: any[] = [];

    constructor(
        private fb: FormBuilder,
        private codeAttributeMainService: CodeAttributeMainService,
        private codeAttributeDetailService: CodeAttributeDetailService,
        private codeGeneratorService: CodeGeneratorService,
        private codeTypeService: CodeTypeService,
        private codeAttributeTypeService: CodeAttributeTypeService,
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

        this.initForms();
        this.loadDropdownData();
    }

    loadDropdownData() {
        // Load Code Types
        this.codeTypeService.getAllCodeTypes().subscribe({
            next: (response) => {
                this.codeTypes = response.data;
            },
            error: (error) => {
                console.error('Error loading code types:', error);
            }
        });

        // Load Code Attribute Types
        this.codeAttributeTypeService.getAllCodeAttributeTypes().subscribe({
            next: (response) => {
                this.codeAttributeTypes = response.data;
            },
            error: (error) => {
                console.error('Error loading code attribute types:', error);
            }
        });
    }

    initForms() {
        this.mainForm = this.fb.group({
            codeTypeId: ['', [Validators.required]],
            codeAttributeTypeId: ['', [Validators.required]],
            code: ['', [Validators.required, Validators.minLength(2)]],
            nameAr: ['', [Validators.required]],
            nameEn: ['', [Validators.required]],
            descriptionAr: ['', [Validators.required]],
            descriptionEn: ['', [Validators.required]]
        });

        this.detailForm = this.fb.group({
            code: ['', [Validators.required, Validators.minLength(2)]],
            nameAr: ['', [Validators.required]],
            nameEn: ['', [Validators.required]],
            descriptionAr: ['', [Validators.required]],
            descriptionEn: ['', [Validators.required]],
            sortOrder: [this.details.length + 1, [Validators.required]]
        });
    }

    onSubmitMain() {
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

        this.isLoading = true;
        this.errorMessage = '';
        this.successMessage = '';

        let completedCount = 0;
        const totalCount = this.mains.length;

        this.mains.forEach((main, index) => {
            const requestData = {
                code: main.code,
                nameAr: main.nameAr,
                nameEn: main.nameEn,
                descriptionAr: main.descriptionAr,
                descriptionEn: main.descriptionEn,
                codeTypeId: main.codeTypeId,
                codeAttributeTypeId: main.codeAttributeTypeId
            };

            this.codeAttributeMainService.createCodeAttributeMain(requestData).subscribe({
                next: (response) => {
                    this.savedMainIds.push(response.data.id);
                    this.codeGeneratorService.addCodeAttributeMainId(response.data.id);
                    completedCount++;

                    if (completedCount === totalCount) {
                        this.isLoading = false;
                        this.successMessage = `All ${totalCount} mains saved successfully! Now you can add details below.`;
                        this.mainsSaved = true;

                        setTimeout(() => {
                            document.getElementById('details-section')?.scrollIntoView({ behavior: 'smooth' });
                        }, 1000);
                    }
                },
                error: (error) => {
                    this.isLoading = false;
                    this.errorMessage = error.error?.message || `Failed to save main ${index + 1}. Please try again.`;
                }
            });
        });
    }

    onSubmitDetail() {
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

        if (this.savedMainIds.length === 0) {
            this.errorMessage = 'No main entries found. Please save main entries first.';
            return;
        }

        this.isLoading = true;
        this.errorMessage = '';
        this.successMessage = '';

        let completedCount = 0;
        const totalCount = this.details.length;

        this.details.forEach((detail, index) => {
            // Cycle through savedMainIds if there are more details than mains
            const mainIdIndex = index % this.savedMainIds.length;
            const requestData = {
                ...detail,
                attributeMainId: this.savedMainIds[mainIdIndex]
            };

            this.codeAttributeDetailService.createCodeAttributeDetail(requestData).subscribe({
                next: (response) => {
                    this.savedDetailIds.push(response.data.id);
                    this.codeGeneratorService.addCodeAttributeDetailId(response.data.id);
                    completedCount++;

                    if (completedCount === totalCount) {
                        this.isLoading = false;
                        this.successMessage = `All ${totalCount} details saved successfully!`;
                        this.codeGeneratorService.completeStep(2);

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

    toggleSidebar(): void {
        this.isSidebarCollapsed = !this.isSidebarCollapsed;
    }

    get mainCodeTypeId() { return this.mainForm.get('codeTypeId'); }
    get mainCodeAttributeTypeId() { return this.mainForm.get('codeAttributeTypeId'); }
    get mainCode() { return this.mainForm.get('code'); }
    get mainNameAr() { return this.mainForm.get('nameAr'); }
    get mainNameEn() { return this.mainForm.get('nameEn'); }
    get mainDescriptionAr() { return this.mainForm.get('descriptionAr'); }
    get mainDescriptionEn() { return this.mainForm.get('descriptionEn'); }

    get detailCode() { return this.detailForm.get('code'); }
    get detailNameAr() { return this.detailForm.get('nameAr'); }
    get detailNameEn() { return this.detailForm.get('nameEn'); }
    get detailDescriptionAr() { return this.detailForm.get('descriptionAr'); }
    get detailDescriptionEn() { return this.detailForm.get('descriptionEn'); }
    get sortOrder() { return this.detailForm.get('sortOrder'); }
}
