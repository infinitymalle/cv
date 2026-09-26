import { api, langQuery, unwrap, type Profile } from "../../api/client";
import type { Language } from "../../i18n/language";

export async function fetchProfile(language: Language, signal: AbortSignal): Promise<Profile> {
  return unwrap(await api.GET("/api/v1/profile", { ...langQuery(language), signal }), "profile");
}
