import { AsyncContent } from "../../components/AsyncContent";
import { Section } from "../../components/Section";
import { useTranslations } from "../../i18n/LanguageContext";
import { useEducation, useExperience } from "./queries";
import { Timeline } from "./Timeline";

export function ExperienceSection() {
  const t = useTranslations();

  return (
    <Section id="experience" title={t.nav.experience}>
      <AsyncContent query={useExperience()}>{(entries) => <Timeline entries={entries} />}</AsyncContent>
    </Section>
  );
}

export function EducationSection() {
  const t = useTranslations();

  return (
    <Section id="education" title={t.nav.education}>
      <AsyncContent query={useEducation()}>{(entries) => <Timeline entries={entries} />}</AsyncContent>
    </Section>
  );
}
