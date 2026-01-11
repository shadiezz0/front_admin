import { Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';

interface MenuItem {
    label: string;
    icon: string;
    route?: string;
    badge?: string;
    children?: MenuItem[];
    expanded?: boolean;
}

@Component({
    selector: 'app-sidebar',
    standalone: true,
    imports: [CommonModule, RouterLink, RouterLinkActive],
    templateUrl: './sidebar.component.html',
    styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
    @Input() isCollapsed = false;

    menuItems = signal<MenuItem[]>([
        {
            label: 'MAIN',
            icon: '',
            children: [
                {
                    label: 'Dashboards',
                    icon: 'home',
                    expanded: true,
                    children: [
                        { label: 'Code Generator', icon: '', route: '/dashboard' }
                    ]
                }
            ]
        },
        {
            label: 'WEB APPS',
            icon: '',
            children: [
                { label: 'Apps', icon: 'grid', route: '/apps' },
                { label: 'Elements', icon: 'layers', route: '/elements' },
                { label: 'Advanced UI', icon: 'box', route: '/advanced-ui' }
            ]
        },
        {
            label: 'PAGES',
            icon: '',
            children: [
                { label: 'Pages', icon: 'file-text', route: '/pages' },
                { label: 'Utilities', icon: 'tool', route: '/utilities' }
            ]
        },
        {
            label: 'GENERAL',
            icon: '',
            children: [
                { label: 'Icons', icon: 'star', route: '/icons' },
                { label: 'Charts', icon: 'bar-chart', route: '/charts' }
            ]
        },
        {
            label: 'MULTI LEVELS',
            icon: '',
            children: [
                {
                    label: 'Menu-levels',
                    icon: 'menu',
                    children: [
                        { label: 'Level 1.1', icon: '', route: '/level-1-1' },
                        { label: 'Level 1.2', icon: '', route: '/level-1-2' }
                    ]
                }
            ]
        }
    ]);

    toggleMenuItem(item: MenuItem): void {
        if (item.children && item.children.length > 0) {
            item.expanded = !item.expanded;
        }
    }

    getIcon(iconName: string): string {
        const icons: { [key: string]: string } = {
            'home': 'M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z',
            'grid': 'M10 3H3v7h7V3zM21 3h-7v7h7V3zM21 14h-7v7h7v-7zM10 14H3v7h7v-7z',
            'layers': 'M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5',
            'box': 'M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z',
            'file-text': 'M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z',
            'tool': 'M14.7 6.3a1 1 0 0 0 0 1.4l1.6 1.6a1 1 0 0 0 1.4 0l3.77-3.77a6 6 0 0 1-7.94 7.94l-6.91 6.91a2.12 2.12 0 0 1-3-3l6.91-6.91a6 6 0 0 1 7.94-7.94l-3.76 3.76z',
            'star': 'M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z',
            'bar-chart': 'M18 20V10M12 20V4M6 20v-6',
            'menu': 'M3 12h18M3 6h18M3 18h18'
        };
        return icons[iconName] || '';
    }
}
