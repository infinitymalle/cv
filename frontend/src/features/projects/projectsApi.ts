import { api, langQuery, unwrap, type Project } from "../../api/client";
import type { Language } from "../../i18n/language";

export async function fetchProjects(language: Language, signal: AbortSignal): Promise<Project[]> {
  return unwrap(await api.GET("/api/v1/projects", { ...langQuery(language), signal }), "projects");
}
