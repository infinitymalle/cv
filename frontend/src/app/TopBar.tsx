import { LanguageSwitch } from "../components/LanguageSwitch";
import { useTranslations } from "../i18n/LanguageContext";

const sections = ["projects", "experience", "education", "courses", "skills"] as const;

export function TopBar() {
  const t = useTranslations();

  return (
    <div className="topbar">
      <div className="topbar-inner">
        <nav aria-label="Sections">
          <ul>
            {sections.map((id) => (
              <li key={id}>
                <a href={`#${id}`}>{t.nav[id]}</a>
              </li>
            ))}
          </ul>
        </nav>
        <LanguageSwitch />
      </div>
    </div>
  );
}
