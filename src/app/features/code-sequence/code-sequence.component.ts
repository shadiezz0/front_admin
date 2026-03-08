import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CodeTypeSequenceService } from '../../core/services/code-type-sequence.service';
import { CodeTypeService, CodeTypeItem } from '../../core/services/code-type.service';
import { AlertComponent } from '../../shared/components/alert/alert.component';
import { Router } from '@angular/router';

@Component({
    selector: 'app-code-sequence',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, AlertComponent],
    templateUrl: './code-sequence.component.html',
    styleUrl: './code-sequence.component.css'
})
export class CodeSequenceComponent implements OnInit {

    seqForm!: FormGroup;

    codeTypes: CodeTypeItem[] = [];
    isLoadingTypes = true;
    isSaving = false;
    errorMessage = '';
    successMessage = '';

    constructor(
        private fb: FormBuilder,
        private sequenceService: CodeTypeSequenceService,
        private codeTypeService: CodeTypeService,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.initForm();
        this.loadCodeTypes();
    }

    initForm(): void {
        this.seqForm = this.fb.group({
            codeTypeId: [null, Validators.required],
            sequenceName: ['', Validators.required],
            startValue: [1, Validators.required],
            minValue: [1],
            maxValue: [999999],
            cycling: [0]
        });
    }

    loadCodeTypes(): void {
        this.isLoadingTypes = true;
        this.codeTypeService.getAllCodeTypes().subscribe({
            next: (res) => {
                this.codeTypes = res.data;
                this.isLoadingTypes = false;
            },
            error: () => {
                this.errorMessage = 'Failed to load code types. Please refresh.';
                this.isLoadingTypes = false;
            }
        });
    }

    onSubmit(): void {
        if (this.seqForm.invalid || this.isSaving) {
            this.seqForm.markAllAsTouched();
            return;
        }

        this.isSaving = true;
        this.errorMessage = '';
        this.successMessage = '';

        const fv = this.seqForm.value;
        const payload = {
            nameEn: fv.sequenceName,
            codeTypeId: +fv.codeTypeId,
            startWith: +fv.startValue,
            minValue: fv.minValue ? +fv.minValue : 0,
            maxValue: fv.maxValue ? +fv.maxValue : 0,
            cycling: +fv.cycling
        };

        this.sequenceService.createCodeTypeSequence(payload).subscribe({
            next: (res) => {
                this.isSaving = false;
                this.successMessage = `✓ Sequence "${fv.sequenceName}" saved! Redirecting...`;
                setTimeout(() => this.router.navigate(['/code-generation']), 1200);
            },
            error: (err) => {
                this.isSaving = false;
                // 200 with success text means already exists / updated — still proceed
                const msg: string = err.error?.message || '';
                if (err.status === 200 || msg.toLowerCase().includes('updated') || msg.toLowerCase().includes('existed')) {
                    this.successMessage = `✓ Sequence saved. Redirecting...`;
                    setTimeout(() => this.router.navigate(['/code-generation']), 1200);
                } else {
                    this.errorMessage = msg || 'Failed to save sequence.';
                }
            }
        });
    }

    get codeTypeId() { return this.seqForm.get('codeTypeId'); }
    get sequenceName() { return this.seqForm.get('sequenceName'); }
    get startValue() { return this.seqForm.get('startValue'); }
    get minValue() { return this.seqForm.get('minValue'); }
    get maxValue() { return this.seqForm.get('maxValue'); }
    get cycling() { return this.seqForm.get('cycling'); }
}
