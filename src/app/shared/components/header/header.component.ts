import { Component, Output, EventEmitter, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-header',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './header.component.html',
    styleUrl: './header.component.css'
})
export class HeaderComponent {
    @Output() toggleSidebar = new EventEmitter<void>();

    isDarkMode = signal(false);
    notificationCount = signal(4);
    userName = signal('Ashton Cox');
    userRole = signal('Web Developer');

    onToggleSidebar(): void {
        this.toggleSidebar.emit();
    }

    toggleDarkMode(): void {
        this.isDarkMode.update(mode => !mode);
        // TODO: Implement actual dark mode toggle
    }

    onSearch(event: Event): void {
        const target = event.target as HTMLInputElement;
        console.log('Search:', target.value);
        // TODO: Implement search functionality
    }
}
