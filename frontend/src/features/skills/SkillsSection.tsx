import { AsyncContent } from "../../components/AsyncContent";
import { Section } from "../../components/Section";
import { useTranslations } from "../../i18n/LanguageContext";
import { useProfile } from "../profile/queries";

export function SkillsSection() {
  const t = useTranslations();

  return (
    <Section id="skills" title={t.nav.skills}>
      <AsyncContent query={useProfile()}>
        {(profile) => (
          <dl className="skills">
            {profile.skills.map((group) => (
              <div key={group.category} className="skill-group">
                <dt>{group.category}</dt>
                <dd>
                  <ul className="tags">
                    {group.items.map((item) => (
                      <li key={item}>{item}</li>
                    ))}
                  </ul>
                </dd>
              </div>
            ))}
            {profile.languages.length > 0 && (
              <div className="skill-group">
                <dt>{t.spokenLanguages}</dt>
                <dd>
                  <ul className="tags">
                    {profile.languages.map((l) => (
                      <li key={l.name}>
                        {l.name} <span className="muted">· {l.proficiency}</span>
                      </li>
                    ))}
                  </ul>
                </dd>
              </div>
            )}
          </dl>
        )}
      </AsyncContent>
    </Section>
  );
}
