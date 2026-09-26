import type { TimelineEntry } from "../../api/client";
import { useLanguage, useTranslations } from "../../i18n/LanguageContext";
import { formatPeriod } from "../../lib/format";

/** A vertical timeline of jobs or schools, newest first. */
export function Timeline({ entries }: { entries: TimelineEntry[] }) {
  const { language } = useLanguage();
  const t = useTranslations();

  return (
    <ol className="timeline">
      {entries.map((entry) => (
        <li key={`${entry.organization}-${entry.startedOn}`} className="timeline-entry">
          <div className="timeline-heading">
            <h3>{entry.role}</h3>
            <p className="muted small">{formatPeriod(entry.startedOn, entry.finishedOn, language, t.present)}</p>
          </div>
          <p className="timeline-org">
            {entry.organization}
            {entry.location && <span className="muted"> · {entry.location}</span>}
          </p>
          {entry.highlights.length > 0 && (
            <ul className="highlights">
              {entry.highlights.map((highlight) => (
                <li key={highlight}>{highlight}</li>
              ))}
            </ul>
          )}
        </li>
      ))}
    </ol>
  );
}
