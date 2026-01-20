import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CodeGeneratorService } from '../../core/services/code-generator.service';
import { AlertComponent } from '../../shared/components/alert/alert.component';
import { ButtonComponent } from '../../shared/components/button/button.component';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [CommonModule, AlertComponent, ButtonComponent],
    templateUrl: './dashboard.component.html',
    styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
    cardNames = [
        'Code Type',
        'Code Type Attribute',
        'Code Type Main',
        'Code Details',
        'Code Settings',
        'Code Sequence',
        'Code Generator'
    ];

    cardRoutes = [
        '/code-type',
        '/code-type-attribute',
        '/code-type-main',
        '/code-details',
        '/code-settings',
        '/code-sequence',
        '/code-generation'
    ];

    completed = signal<boolean[]>([]);
    activeCard: number | null = null;
    errorMessage = '';

    constructor(
        private codeGeneratorService: CodeGeneratorService,
        private router: Router
    ) { }

    ngOnInit() {
        // Initialize completed state from service
        const state = this.codeGeneratorService.getState();
        this.completed.set(state.completedSteps);
    }

    canAccess(index: number): boolean {
        return this.codeGeneratorService.canAccessStep(index);
    }

    openCard(index: number): void {
        this.clearError();
        this.router.navigate([this.cardRoutes[index]]);
    }

    completeCard(index: number): void {
        if (!this.canAccess(index)) {
            this.openCard(index);
            return;
        }
        this.codeGeneratorService.completeStep(index);
        const state = this.codeGeneratorService.getState();
        this.completed.set(state.completedSteps);
        this.activeCard = null;
        this.clearError();
    }

    clearError(): void {
        this.errorMessage = '';
    }
}
