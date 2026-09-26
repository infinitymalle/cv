import { ProjectCard } from "./ProjectCard";
import { useProjects } from "./useProjects";

export function ProjectList() {
  const state = useProjects();

  switch (state.status) {
    case "loading":
      return <p className="muted">Loading projects…</p>;
    case "error":
      return <p role="alert">Projects could not be loaded right now.</p>;
    case "success":
      if (state.projects.length === 0) {
        return <p className="muted">No projects yet.</p>;
      }
      return (
        <ul className="project-list">
          {state.projects.map((project) => (
            <li key={project.slug}>
              <ProjectCard project={project} />
            </li>
          ))}
        </ul>
      );
  }
}
