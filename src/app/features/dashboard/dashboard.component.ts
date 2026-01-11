import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HeaderComponent } from '../../shared/components/header/header.component';
import { FooterComponent } from '../../shared/components/footer/footer.component';
import { SidebarComponent } from '../../shared/components/sidebar/sidebar.component';
import { CodeGeneratorService } from '../../core/services/code-generator.service';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [CommonModule, HeaderComponent, SidebarComponent, FooterComponent],
    templateUrl: './dashboard.component.html',
    styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
    isSidebarCollapsed = false;

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

    toggleSidebar(): void {
        this.isSidebarCollapsed = !this.isSidebarCollapsed;
    }

    canAccess(index: number): boolean {
        return this.codeGeneratorService.canAccessStep(index);
    }

    openCard(index: number): void {
        this.clearError();
        if (!this.canAccess(index)) {
            const prevName = this.cardNames[index - 1] || 'previous step';
            this.errorMessage = `Can't access without completing ${prevName.toLowerCase()} first`;
            return;
        }
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
