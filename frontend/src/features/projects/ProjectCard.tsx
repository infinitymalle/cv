import type { Project } from "../../api/client";
import { useLanguage, useTranslations } from "../../i18n/LanguageContext";
import { formatPeriod } from "../../lib/format";

export function ProjectCard({ project }: { project: Project }) {
  const { language } = useLanguage();
  const t = useTranslations();

  return (
    <article className="card">
      <header className="card-header">
        <h3>{project.title}</h3>
        <p className="muted small">{formatPeriod(project.startedOn, project.finishedOn, language, t.present)}</p>
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
          {t.sourceCode}
        </a>
      )}
    </article>
  );
}
