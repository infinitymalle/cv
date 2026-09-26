import { api, type Project } from "../../api/client";

export async function fetchProjects(signal?: AbortSignal): Promise<Project[]> {
  const { data, error } = await api.GET("/api/v1/projects", { signal });
  if (error || !data) {
    throw new Error("Could not load projects");
  }
  return data;
}
