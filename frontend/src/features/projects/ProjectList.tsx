import { AsyncContent } from "../../components/AsyncContent";
import { useTranslations } from "../../i18n/LanguageContext";
import { ProjectCard } from "./ProjectCard";
import { useProjects } from "./queries";

export function ProjectList() {
  const t = useTranslations();

  return (
    <AsyncContent query={useProjects()}>
      {(projects) =>
        projects.length === 0 ? (
          <p className="muted">{t.noProjects}</p>
        ) : (
          <ul className="card-list">
            {projects.map((project) => (
              <li key={project.slug}>
                <ProjectCard project={project} />
              </li>
            ))}
          </ul>
        )
      }
    </AsyncContent>
  );
}
