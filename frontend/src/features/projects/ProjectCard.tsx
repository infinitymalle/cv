import type { Project } from "../../api/client";

const monthFormat = new Intl.DateTimeFormat("en-GB", { month: "short", year: "numeric" });

function formatPeriod(startedOn: string, finishedOn: string | null): string {
  const start = monthFormat.format(new Date(startedOn));
  return `${start} – ${finishedOn ? monthFormat.format(new Date(finishedOn)) : "present"}`;
}

export function ProjectCard({ project }: { project: Project }) {
  return (
    <article className="project-card">
      <header>
        <h3>{project.title}</h3>
        <p className="muted">{formatPeriod(project.startedOn, project.finishedOn)}</p>
      </header>

      <p>{project.summary}</p>

      {project.technologies.length > 0 && (
        <ul className="tags" aria-label="Technologies">
          {project.technologies.map((tech) => (
            <li key={tech}>{tech}</li>
          ))}
        </ul>
      )}

      {project.repositoryUrl && (
        <a href={project.repositoryUrl} target="_blank" rel="noopener noreferrer">
          Source code
        </a>
      )}
    </article>
  );
}
