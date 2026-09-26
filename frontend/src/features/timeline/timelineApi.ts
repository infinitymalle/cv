import { api, langQuery, unwrap, type TimelineEntry } from "../../api/client";
import type { Language } from "../../i18n/language";

export async function fetchExperience(language: Language, signal: AbortSignal): Promise<TimelineEntry[]> {
  return unwrap(await api.GET("/api/v1/experience", { ...langQuery(language), signal }), "experience");
}

export async function fetchEducation(language: Language, signal: AbortSignal): Promise<TimelineEntry[]> {
  return unwrap(await api.GET("/api/v1/education", { ...langQuery(language), signal }), "education");
}
