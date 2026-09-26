import { api, langQuery, unwrap, type CourseOverview } from "../../api/client";
import type { Language } from "../../i18n/language";

export async function fetchCourses(language: Language, signal: AbortSignal): Promise<CourseOverview> {
  return unwrap(await api.GET("/api/v1/courses", { ...langQuery(language), signal }), "courses");
}
