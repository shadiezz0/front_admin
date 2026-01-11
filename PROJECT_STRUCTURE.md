# Code Generator FE - Project Structure

## 📁 Project Architecture

This Angular application follows a modular, scalable architecture based on best practices.

```
src/
├── environments/                   # Environment configuration
│   ├── environment.ts              # Development settings
│   └── environment.prod.ts         # Production settings
│
├── index.html                      # Main HTML entry
├── main.ts                         # Angular bootstrap
├── styles.css                      # Global styles & design tokens
│
└── app/
    ├── app.component.*             # Root component (minimal - just router-outlet)
    ├── app.config.ts               # App providers (HTTP, interceptors, etc.)
    ├── app.routes.ts               # Main routing configuration
    │
    ├── core/                       # 🔷 SINGLETON SERVICES & INFRASTRUCTURE
    │   ├── constants/              # Centralized constants (API endpoints, routes, etc.)
    │   ├── guards/                 # Route guards (auth, guest)
    │   ├── interceptors/           # HTTP interceptors (auth token injection)
    │   ├── models/                 # TypeScript interfaces and types
    │   └── services/               # Global singleton services
    │
    ├── features/                   # 🔶 FEATURE MODULES (Lazy-loaded)
    │   ├── auth/                   # Authentication (login, register)
    │   ├── dashboard/              # Main dashboard
    │   ├── landing/                # Landing/home page
    │   └── settings/               # User settings
    │
    └── shared/                     # 🔷 REUSABLE COMPONENTS
        ├── components/             # UI components (button, card, etc.)
        ├── directives/             # Custom directives
        └── pipes/                  # Custom pipes
```

## 🏗️ Architecture Layers

### Core Layer
**Purpose**: Singleton services, guards, interceptors, and models used throughout the app.

**Key Files:**
- **Services**: `AuthService`, `TranslationService`
- **Guards**: `authGuard`, `guestGuard`
- **Interceptors**: `authInterceptor` (adds JWT tokens to requests)
- **Models**: Data interfaces and types
- **Constants**: API endpoints, storage keys, routes

**Rule**: Never import feature modules into core. Core is imported by features.

### Features Layer
**Purpose**: Business logic organized by feature/domain.

**Characteristics:**
- Each feature is standalone and lazy-loaded
- Features can import from `core` and `shared`
- Features should NOT import from other features

**Current Features:**
- **auth**: Login, registration, authentication flows
- **dashboard**: Main user dashboard
- **landing**: Public landing page
- **settings**: User preferences and settings

### Shared Layer
**Purpose**: Reusable UI components, directives, and pipes.

**Components:**
- `ButtonComponent`: Reusable button with variants
- `CardComponent`: Content container card

**Rule**: Shared components should be generic and not contain business logic.

## 🚀 Key Features

### 1. Route Configuration
- **Lazy Loading**: All features are lazy-loaded for optimal performance
- **Route Guards**: Protected routes require authentication
- **Guest Routes**: Auth pages redirect if already logged in

### 2. Authentication System
- JWT token-based authentication
- HTTP interceptor automatically adds tokens to requests
- Auth state managed with Angular Signals
- Route guards protect authenticated routes

### 3. Translation Service
- Built-in i18n support (English/Arabic)
- RTL support for Arabic
- Persistent language preference

### 4. HTTP Configuration
- Global HTTP interceptor for auth tokens
- Centralized API endpoint constants
- Environment-based API URL configuration

## 📝 Development Guidelines

### Adding a New Feature
1. Create feature folder in `src/app/features/[feature-name]`
2. Create standalone component
3. Add route to `app.routes.ts`
4. Add guard if authentication required

```typescript
// Example: Adding a "profile" feature
{
  path: 'profile',
  loadComponent: () => import('./features/profile/profile.component')
    .then(m => m.ProfileComponent),
  canActivate: [authGuard]
}
```

### Adding a New Service
1. Create service in `src/app/core/services/`
2. Use `providedIn: 'root'` for singleton
3. Export from `core/services/index.ts`

```typescript
@Injectable({
  providedIn: 'root'
})
export class MyService { }
```

### Adding Shared Components
1. Create component in `src/app/shared/components/[component-name]`
2. Make it standalone
3. Export from `shared/components/index.ts`

## 🔒 Security

- **Auth Guard**: Protects routes requiring authentication
- **Guest Guard**: Prevents authenticated users from accessing login/register
- **HTTP Interceptor**: Automatically adds JWT tokens
- **Token Storage**: Tokens stored in localStorage

## 🎨 Styling

- **Global Styles**: `src/styles.css` contains CSS reset and design tokens
- **Component Styles**: Scoped to each component
- **Design Tokens**: CSS custom properties for consistency

## 🌐 Environment Configuration

### Development
```typescript
// environment.ts
{
  production: false,
  apiUrl: 'http://localhost:3000/api',
  defaultLanguage: 'en'
}
```

### Production
```typescript
// environment.prod.ts
{
  production: true,
  apiUrl: 'https://api.codegenerator.com/api',
  defaultLanguage: 'en'
}
```

## 🧪 Testing the Structure

Run the development server:
```bash
npm run dev
# or
ng serve
```

Build for production:
```bash
npm run build
# or
ng build
```

## 📦 Import Rules

```
✅ Features can import: core, shared
✅ Shared can import: core
✅ Core imports: Nothing (only Angular and 3rd party libs)
❌ Core cannot import: features, shared
❌ Features cannot import: other features
```

## 🔄 State Management

Currently using:
- **Angular Signals** for reactive state
- **RxJS** for async operations
- **Services** for global state

## 📚 Next Steps

1. Add more features as needed
2. Implement additional shared components
3. Add e2e tests
4. Configure CI/CD pipeline
5. Add more translation keys
6. Implement error handling service
7. Add loading indicators
8. Implement toast notifications

## 🤝 Contributing

1. Follow the folder structure strictly
2. Use barrel exports (`index.ts`) for cleaner imports
3. Keep components standalone
4. Use signals for reactive state
5. Document complex logic
6. Follow Angular style guide

---

**Last Updated**: January 2026  
**Angular Version**: 19.x  
**Architecture Pattern**: Feature-Based Modular Architecture
