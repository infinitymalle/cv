import type { paths } from "../api/schema";

/** Languages the backend supports, taken straight from the API contract. */
export type Language = NonNullable<NonNullable<paths["/api/v1/profile"]["get"]["parameters"]["query"]>["lang"]>;

export const defaultLanguage: Language = "en";

const storageKey = "cv.language";

const locales: Record<Language, string> = {
  en: "en-GB",
  sv: "sv-SE",
};

/** BCP 47 locale for formatting dates and numbers. */
export function localeOf(language: Language): string {
  return locales[language];
}

function isLanguage(value: unknown): value is Language {
  return typeof value === "string" && value in locales;
}

/** The visitor's saved choice, otherwise their browser language, otherwise English. */
export function detectInitialLanguage(): Language {
  try {
    const saved = localStorage.getItem(storageKey);
    if (isLanguage(saved)) return saved;
  } catch {
    // Storage can be blocked (private mode, strict privacy settings); fall through.
  }

  const browser = navigator.language?.slice(0, 2).toLowerCase();
  return isLanguage(browser) ? browser : defaultLanguage;
}

export function saveLanguage(language: Language): void {
  try {
    localStorage.setItem(storageKey, language);
  } catch {
    // Not critical: the choice just won't be remembered.
  }
}
