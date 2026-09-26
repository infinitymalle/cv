import type { Language } from "./language";

/** Texts that belong to the UI itself. CV content comes translated from the API. */
const en = {
  nav: {
    projects: "Projects",
    experience: "Experience",
    education: "Education",
    courses: "Courses",
    skills: "Skills",
  },
  languageSwitch: "English",
  loading: "Loading…",
  loadError: "This section couldn't be loaded right now.",
  present: "present",
  sourceCode: "Source code",
  noProjects: "No projects yet.",
  creditsUnit: "credits",
  creditsCompleted: (credits: string, count: number) => `${credits} credits · ${count} completed courses`,
  keyCourse: "Key course",
  showAllCourses: (count: number) => `Show all ${count} courses`,
  hideAllCourses: "Show fewer courses",
  courseLevel: { basic: "Basic level", advanced: "Advanced level" },
  spokenLanguages: "Languages",
  footer: "Built with ASP.NET Core and React.",
  footerSource: "Source on GitHub",
};

export type Translations = typeof en;

// `satisfies` makes the compiler check that Swedish has exactly the same keys as English.
const sv = {
  nav: {
    projects: "Projekt",
    experience: "Erfarenhet",
    education: "Utbildning",
    courses: "Kurser",
    skills: "Kompetenser",
  },
  languageSwitch: "Engelska",
  loading: "Laddar…",
  loadError: "Det gick inte att ladda den här delen just nu.",
  present: "pågående",
  sourceCode: "Källkod",
  noProjects: "Inga projekt ännu.",
  creditsUnit: "hp",
  creditsCompleted: (credits: string, count: number) => `${credits} hp · ${count} avklarade kurser`,
  keyCourse: "Nyckelkurs",
  showAllCourses: (count: number) => `Visa alla ${count} kurser`,
  hideAllCourses: "Visa färre kurser",
  courseLevel: { basic: "Grundnivå", advanced: "Avancerad nivå" },
  spokenLanguages: "Språk",
  footer: "Byggd med ASP.NET Core och React.",
  footerSource: "Källkod på GitHub",
} satisfies Translations;

export const translations: Record<Language, Translations> = { en, sv };
