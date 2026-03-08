import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CodeAttributeDetailService, CodeAttributeDetailItem } from '../../core/services/code-attribute-detail.service';

@Component({
    selector: 'app-code-details-list',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './code-details-list.component.html',
    styleUrl: './code-details-list.component.css'
})
export class CodeDetailsListComponent implements OnInit {
    codeDetails: CodeAttributeDetailItem[] = [];
    filteredCodeDetails: CodeAttributeDetailItem[] = [];
    isLoading = false;
    errorMessage = '';

    showFilter = false;
    filterRequest = { nameEn: '', nameAr: '', code: '' };

    constructor(
        private codeDetailService: CodeAttributeDetailService,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.loadCodeDetails();
    }

    // ── Load ──────────────────────────────────────────────────────────────────

    loadCodeDetails(): void {
        this.isLoading = true;
        this.errorMessage = '';
        this.codeDetailService.getAllCodeAttributeDetails().subscribe({
            next: (response) => {
                this.isLoading = false;
                this.codeDetails = response.data;
                this.filteredCodeDetails = [...this.codeDetails];
            },
            error: (err) => {
                this.isLoading = false;
                this.errorMessage = err.error?.message || 'Failed to load code details.';
            }
        });
    }

    // ── Filter ────────────────────────────────────────────────────────────────

    toggleFilter(): void { this.showFilter = !this.showFilter; }

    applyFilter(): void {
        const nameEn = this.filterRequest.nameEn.toLowerCase().trim();
        const nameAr = this.filterRequest.nameAr.trim();
        const code = this.filterRequest.code.toLowerCase().trim();

        this.filteredCodeDetails = this.codeDetails.filter(item => {
            const matchesNameEn = nameEn ? item.nameEn?.toLowerCase().includes(nameEn) : true;
            const matchesNameAr = nameAr ? item.nameAr?.includes(nameAr) : true;
            const matchesCode = code ? item.code?.toLowerCase().includes(code) : true;
            return matchesNameEn && matchesNameAr && matchesCode;
        });
    }

    resetFilter(): void {
        this.filterRequest = { nameEn: '', nameAr: '', code: '' };
        this.filteredCodeDetails = [...this.codeDetails];
    }

    // ── Navigation ────────────────────────────────────────────────────────────

    navigateToAdd(): void {
        this.router.navigate(['/code-details']);
    }

    navigateToEdit(item: CodeAttributeDetailItem): void {
        this.router.navigate(['/update-code-detail', item.id]);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    formatDate(dateStr: string | null): string {
        if (!dateStr) return '—';
        return new Date(dateStr).toISOString().split('T')[0];
    }
}
