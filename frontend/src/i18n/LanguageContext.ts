import { createContext, useContext } from "react";
import type { Language } from "./language";
import { translations, type Translations } from "./translations";

export interface LanguageContextValue {
  language: Language;
  setLanguage: (language: Language) => void;
}

export const LanguageContext = createContext<LanguageContextValue | null>(null);

export function useLanguage(): LanguageContextValue {
  const context = useContext(LanguageContext);
  if (!context) {
    throw new Error("useLanguage must be used inside <LanguageProvider>");
  }
  return context;
}

/** UI texts in the current language. */
export function useTranslations(): Translations {
  return translations[useLanguage().language];
}
