import type { UseQueryResult } from "@tanstack/react-query";
import type { ReactNode } from "react";
import { useTranslations } from "../i18n/LanguageContext";

/** Renders loading / error states for a query, and `children(data)` once data is available. */
export function AsyncContent<T>({ query, children }: { query: UseQueryResult<T>; children: (data: T) => ReactNode }) {
  const t = useTranslations();

  if (query.data !== undefined) {
    return <>{children(query.data)}</>;
  }
  if (query.isError) {
    return (
      <p role="alert" className="muted">
        {t.loadError}
      </p>
    );
  }
  return <p className="muted">{t.loading}</p>;
}
