import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeGenerationService, CodeItem } from '../../core/services/code-generation.service';

@Component({
    selector: 'app-codes-list',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './codes-list.component.html',
    styleUrl: './codes-list.component.css'
})
export class CodesListComponent implements OnInit {
    codes: CodeItem[] = [];
    filteredCodes: CodeItem[] = [];
    isLoading = false;
    errorMessage = '';

    // Filter state
    showFilter = false;
    filterRequest = {
        nameEn: '',
        nameAr: '',
        status: '',
        externalSystem: ''
    };

    constructor(
        private codeGenerationService: CodeGenerationService,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.loadCodes();
    }

    loadCodes(): void {
        this.isLoading = true;
        this.errorMessage = '';
        this.codeGenerationService.getAllCodes().subscribe({
            next: (response) => {
                this.isLoading = false;
                this.codes = response.data;
                this.filteredCodes = [...this.codes];
            },
            error: (err) => {
                this.isLoading = false;
                this.errorMessage = err.error?.message || 'Failed to load generated codes.';
            }
        });
    }

    toggleFilter(): void {
        this.showFilter = !this.showFilter;
    }

    applyFilter(): void {
        const nameEn = this.filterRequest.nameEn.toLowerCase().trim();
        const nameAr = this.filterRequest.nameAr.trim();
        const status = this.filterRequest.status.toLowerCase().trim();
        const extSystem = this.filterRequest.externalSystem.toLowerCase().trim();

        this.filteredCodes = this.codes.filter(item => {
            const matchesNameEn = nameEn ? item.nameEn.toLowerCase().includes(nameEn) : true;
            const matchesNameAr = nameAr ? item.nameAr.includes(nameAr) : true;
            const matchesStatus = status ? item.status.toLowerCase().includes(status) : true;
            const matchesExtSystem = extSystem ? item.externalSystem.toLowerCase().includes(extSystem) : true;
            return matchesNameEn && matchesNameAr && matchesStatus && matchesExtSystem;
        });
    }

    resetFilter(): void {
        this.filterRequest = { nameEn: '', nameAr: '', status: '', externalSystem: '' };
        this.filteredCodes = [...this.codes];
    }

    navigateToAdd(): void {
        this.router.navigate(['/code-generation']);
    }

    getStatusClass(status: string): string {
        switch (status?.toUpperCase()) {
            case 'DRAFT': return 'badge-draft';
            case 'APPROVED': return 'badge-approved';
            case 'REJECTED': return 'badge-rejected';
            case 'CANCELLED': return 'badge-cancelled';
            default: return 'badge-default';
        }
    }

    formatDate(dateStr: string | null): string {
        if (!dateStr) return '—';
        return new Date(dateStr).toISOString().split('T')[0];
    }
}
