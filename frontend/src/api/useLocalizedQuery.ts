import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { useLanguage } from "../i18n/LanguageContext";
import type { Language } from "../i18n/language";

/**
 * Fetches data in the current language. Each language is cached separately, so switching back is instant,
 * and the previous language stays on screen while the new one loads (no flicker).
 */
export function useLocalizedQuery<T>(key: string, fetcher: (language: Language, signal: AbortSignal) => Promise<T>) {
  const { language } = useLanguage();

  return useQuery({
    queryKey: [key, language],
    queryFn: ({ signal }) => fetcher(language, signal),
    placeholderData: keepPreviousData,
  });
}
