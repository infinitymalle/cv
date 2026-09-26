import { useLanguage, useTranslations } from "../i18n/LanguageContext";
import { BritishFlag, SwedishFlag } from "./Flags";

/** Swedish flag · slider · British flag. The slider is a switch: on = English. */
export function LanguageSwitch() {
  const { language, setLanguage } = useLanguage();
  const t = useTranslations();
  const isEnglish = language === "en";

  return (
    <button
      type="button"
      role="switch"
      aria-checked={isEnglish}
      aria-label={t.languageSwitch}
      className="language-switch"
      data-language={language}
      onClick={() => setLanguage(isEnglish ? "sv" : "en")}
    >
      <SwedishFlag className="flag flag-sv" />
      <span className="switch-track">
        <span className="switch-thumb" />
      </span>
      <BritishFlag className="flag flag-en" />
    </button>
  );
}
