import { useEffect, useState } from "react";
import type { Project } from "../../api/client";
import { fetchProjects } from "./projectsApi";

export type ProjectsState =
  | { status: "loading" }
  | { status: "error" }
  | { status: "success"; projects: Project[] };

export function useProjects(): ProjectsState {
  const [state, setState] = useState<ProjectsState>({ status: "loading" });

  useEffect(() => {
    const controller = new AbortController();

    fetchProjects(controller.signal)
      .then((projects) => setState({ status: "success", projects }))
      .catch(() => {
        if (!controller.signal.aborted) setState({ status: "error" });
      });

    return () => controller.abort();
  }, []);

  return state;
}
