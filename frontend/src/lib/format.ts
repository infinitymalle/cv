import { localeOf, type Language } from "../i18n/language";

// Dates from the API are plain "YYYY-MM-DD". Formatting in UTC keeps the month from shifting
// for visitors in time zones behind UTC.
export function formatMonth(isoDate: string, language: Language): string {
  return new Intl.DateTimeFormat(localeOf(language), { month: "short", year: "numeric", timeZone: "UTC" }).format(
    new Date(isoDate),
  );
}

export function formatPeriod(startedOn: string, finishedOn: string | null, language: Language, present: string): string {
  const start = formatMonth(startedOn, language);
  const end = finishedOn ? formatMonth(finishedOn, language) : present;
  return `${start} – ${end}`;
}

export function formatNumber(value: number, language: Language): string {
  return new Intl.NumberFormat(localeOf(language), { maximumFractionDigits: 1 }).format(value);
}
