import { Section } from "../components/Section";
import { TopBar } from "./TopBar";
import { Hero } from "../features/profile/Hero";
import { ProjectList } from "../features/projects/ProjectList";
import { ExperienceSection, EducationSection } from "../features/timeline/TimelineSections";
import { CoursesSection } from "../features/courses/CoursesSection";
import { SkillsSection } from "../features/skills/SkillsSection";
import { useTranslations } from "../i18n/LanguageContext";

export function App() {
  const t = useTranslations();

  return (
    <>
      <TopBar />
      <div className="page">
        <Hero />
        <main>
          <Section id="projects" title={t.nav.projects}>
            <ProjectList />
          </Section>
          <ExperienceSection />
          <EducationSection />
          <CoursesSection />
          <SkillsSection />
        </main>
        <footer className="footer muted small">
          {t.footer}{" "}
          <a href="https://github.com/infinitymalle/cv" target="_blank" rel="noopener noreferrer">
            {t.footerSource}
          </a>
        </footer>
      </div>
    </>
  );
}
