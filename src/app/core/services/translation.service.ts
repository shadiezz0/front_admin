import { Injectable, signal } from '@angular/core';
import { STORAGE_KEYS } from '../constants';
import { environment } from '../../../environments/environment';

interface Translations {
    [key: string]: {
        [lang: string]: string;
    };
}

@Injectable({
    providedIn: 'root'
})
export class TranslationService {
    currentLanguage = signal<string>(environment.defaultLanguage);
    availableLanguages = environment.supportedLanguages;

    // Simple translation dictionary
    private translations: Translations = {
        'app.title': {
            en: 'Code Generator',
            ar: 'مولد الأكواد'
        },
        'auth.login': {
            en: 'Login',
            ar: 'تسجيل الدخول'
        },
        'auth.register': {
            en: 'Register',
            ar: 'التسجيل'
        }
    };

    constructor() {
        this.initializeLanguage();
    }

    /**
     * Initialize language from storage or default
     */
    private initializeLanguage(): void {
        const savedLanguage = typeof window !== 'undefined' && window.localStorage ? window.localStorage.getItem(STORAGE_KEYS.LANGUAGE) : null;
        const browserLanguage = typeof window !== 'undefined' && window.navigator ? navigator.language.split('-')[0] : environment.defaultLanguage;

        const languageToUse = savedLanguage ||
            (this.availableLanguages.includes(browserLanguage)
                ? browserLanguage
                : environment.defaultLanguage);

        this.setLanguage(languageToUse);
    }

    /**
     * Change application language
     */
    setLanguage(language: string): void {
        if (this.availableLanguages.includes(language)) {
            this.currentLanguage.set(language);
            if (typeof window !== 'undefined' && window.localStorage) {
                window.localStorage.setItem(STORAGE_KEYS.LANGUAGE, language);
            }
            this.updateDocumentDirection(language);
        }
    }

    /**
     * Update document direction for RTL languages
     */
    private updateDocumentDirection(language: string): void {
        const direction = language === 'ar' ? 'rtl' : 'ltr';
        document.documentElement.setAttribute('dir', direction);
        document.documentElement.setAttribute('lang', language);
    }

    /**
     * Get translation for a key
     */
    instant(key: string, params?: any): string {
        const lang = this.currentLanguage();
        return this.translations[key]?.[lang] || key;
    }

    /**
     * Switch between languages
     */
    toggleLanguage(): void {
        const currentLang = this.currentLanguage();
        const newLang = currentLang === 'en' ? 'ar' : 'en';
        this.setLanguage(newLang);
    }
}
