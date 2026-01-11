# 🎨 Layout Components - Usage Guide

## 📁 Components Created

Three professional layout components have been created in `src/app/shared/components/`:

### 1. **Header Component** (`header/`)
- Logo and branding
- Search bar
- User profile with avatar
- Notifications bell with badge counter
- Dark mode toggle
- Language selector
- Multiple action icons (trophy, apps grid, fullscreen, settings)
- Responsive design

### 2. **Sidebar Component** (`sidebar/`)
- Collapsible navigation menu
- Multi-level menu support
- Active state indicators
- Expandable menu items
- Section headers (MAIN, WEB APPS, PAGES, GENERAL, MULTI LEVELS)
- Icon-based navigation
- Smooth animations
- Version info in footer

### 3. **Footer Component** (`footer/`)
- Copyright information
- Animated heart icon
- Company branding
- Responsive layout

## 🚀 How to Use

### Method 1: Using Main Layout Component (Recommended)

The `MainLayoutComponent` combines all three components for you:

```typescript
// In your route configuration (app.routes.ts)
{
  path: 'dashboard',
  component: MainLayoutComponent,
  children: [
    { path: '', component: DashboardComponent },
    { path: 'profile', component: ProfileComponent }
  ]
}
```

### Method 2: Manual Integration

Use components individually in your templates:

```html
<!-- your-component.html -->
<div class="app-container">
  <app-sidebar></app-sidebar>
  
  <div class="main-content">
    <app-header></app-header>
    
    <main>
      <!-- Your content here -->
    </main>
    
    <app-footer></app-footer>
  </div>
</div>
```

```typescript
// your-component.ts
import { HeaderComponent } from './shared/components/header/header.component';
import { SidebarComponent } from './shared/components/sidebar/sidebar.component';
import { FooterComponent } from './shared/components/footer/footer.component';

@Component({
  // ...
  imports: [HeaderComponent, SidebarComponent, FooterComponent]
})
```

## 🎯 Component Features

### Header Component

**Inputs:**
- None (currently uses internal state)

**Outputs:**
- `toggleSidebar: EventEmitter<void>` - Emits when menu toggle is clicked

**Signals:**
- `isDarkMode` - Dark mode state
- `notificationCount` - Number of unread notifications
- `userName` - Current user name
- `userRole` - Current user role

**Example:**
```html
<app-header (toggleSidebar)="onToggleSidebar()"></app-header>
```

### Sidebar Component

**Inputs:**
- `isCollapsed: boolean` - Controls sidebar collapsed state

**Features:**
- Hierarchical navigation menu
- Expandable menu items
- Active route highlighting
- Smooth collapse/expand animations

**Example:**
```html
<app-sidebar [isCollapsed]="isSidebarCollapsed"></app-sidebar>
```

**Customizing Menu Items:**
```typescript
// In sidebar.component.ts
menuItems = signal<MenuItem[]>([
  {
    label: 'MAIN',
    icon: '',
    children: [
      {
        label: 'Dashboard',
        icon: 'home',
        route: '/dashboard'
      }
    ]
  }
]);
```

### Footer Component

**Signals:**
- `currentYear` - Automatically updates
- `appName` - Application name
- `companyName` - Company name

**Example:**
```html
<app-footer></app-footer>
```

## 🎨 Design Specifications

### Color Palette
- **Primary**: `#06b6d4` (Cyan/Turquoise)
- **Background**: `#ffffff` (White)
- **Text Primary**: `#111827` (Dark Gray)
- **Text Secondary**: `#6b7280` (Medium Gray)
- **Border**: `#e5e7eb` (Light Gray)
- **Hover**: `#f3f4f6` (Very Light Gray)

### Dimensions
- **Header Height**: ~64px (dynamic based on content)
- **Sidebar Width**: 260px (expanded), 70px (collapsed)
- **Footer Height**: Auto (based on content)

### Icons
All icons are SVG-based for crisp rendering at any size.

## 📱 Responsive Behavior

### Mobile (< 768px)
- Sidebar transforms off-screen
- Header search bar hidden
- User info condensed
- Footer text size reduced

### Tablet (768px - 1024px)
- Sidebar can be toggled
- Header maintains most features
- Optimized spacing

### Desktop (> 1024px)
- Full layout visible
- All features enabled
- Optimal spacing

## 🔧 Customization

### Changing Colors

Edit the component CSS files:

```css
/* header.component.css */
.header {
  background: #your-color;
}

/* For primary color changes */
.nav-item.active {
  color: #your-primary-color;
}
```

### Adding Menu Items

Modify the `menuItems` signal in `sidebar.component.ts`:

```typescript
menuItems = signal<MenuItem[]>([
  {
    label: 'YOUR SECTION',
    icon: '',
    children: [
      {
        label: 'Your Page',
        icon: 'icon-name',
        route: '/your-route'
      }
    ]
  }
]);
```

### Adding Header Actions

In `header.component.html`, add new buttons:

```html
<button class="icon-button" aria-label="Your action">
  <svg width="20" height="20" viewBox="0 0 24 24">
    <!-- Your icon SVG path -->
  </svg>
</button>
```

## 📦 Files Structure

```
src/app/shared/
├── components/
│   ├── header/
│   │   ├── header.component.ts
│   │   ├── header.component.html
│   │   └── header.component.css
│   │
│   ├── sidebar/
│   │   ├── sidebar.component.ts
│   │   ├── sidebar.component.html
│   │   └── sidebar.component.css
│   │
│   ├── footer/
│   │   ├── footer.component.ts
│   │   ├── footer.component.html
│   │   └── footer.component.css
│   │
│   └── index.ts (barrel export)
│
└── layouts/
    └── main-layout/
        ├── main-layout.component.ts
        ├── main-layout.component.html
        └── main-layout.component.css
```

## 🎯 Best Practices

1. **Use MainLayoutComponent** for pages that need the full dashboard layout
2. **Keep components standalone** - all components are already standalone
3. **Customize menu items** in one place (sidebar.component.ts)
4. **Use signals** for reactive state management
5. **Follow the color scheme** for consistency

## 🚀 Quick Start Example

```typescript
// app.routes.ts
import { MainLayoutComponent } from './shared/layouts/main-layout/main-layout.component';

export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/dashboard.component')
          .then(m => m.DashboardComponent)
      }
    ]
  }
];
```

## 💡 Tips

- The header automatically handles the sidebar toggle
- Sidebar highlights active routes automatically using `routerLinkActive`
- Footer updates the year automatically
- All components are fully responsive
- Dark mode toggle is ready (just needs implementation)

---

**Components Status**: ✅ Ready to Use  
**Design Match**: ✅ Matches provided screenshot  
**Responsive**: ✅ Mobile, Tablet, Desktop  
**Build Status**: Ready for testing
