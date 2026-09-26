import { useEffect, useMemo, useState, type ReactNode } from "react";
import { detectInitialLanguage, saveLanguage, type Language } from "./language";
import { LanguageContext } from "./LanguageContext";

export function LanguageProvider({ children, initial }: { children: ReactNode; initial?: Language }) {
  const [language, setLanguage] = useState<Language>(() => initial ?? detectInitialLanguage());

  useEffect(() => {
    // Screen readers, search engines and hyphenation all use <html lang>.
    document.documentElement.lang = language;
    saveLanguage(language);
  }, [language]);

  const value = useMemo(() => ({ language, setLanguage }), [language]);

  return <LanguageContext.Provider value={value}>{children}</LanguageContext.Provider>;
}
