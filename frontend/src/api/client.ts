import createClient from "openapi-fetch";
import type { Language } from "../i18n/language";
import type { components, paths } from "./schema";

/**
 * Typed API client. Paths, parameters and response shapes all come from schema.d.ts,
 * which is generated from the backend (npm run gen:api). If the backend changes, this won't compile.
 */
export const api = createClient<paths>({ baseUrl: window.location.origin });

type Schemas = components["schemas"];
export type Profile = Schemas["ProfileDto"];
export type Project = Schemas["ProjectDto"];
export type TimelineEntry = Schemas["TimelineEntryDto"];
export type CourseOverview = Schemas["CourseOverviewDto"];
export type Course = Schemas["CourseDto"];

/** Unwraps an openapi-fetch result, turning API errors into exceptions for React Query. */
export function unwrap<T>(result: { data?: T; error?: unknown }, what: string): T {
  if (result.error !== undefined || result.data === undefined) {
    throw new Error(`Could not load ${what}`);
  }
  return result.data;
}

export const langQuery = (lang: Language) => ({ params: { query: { lang } } });
