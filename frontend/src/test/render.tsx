import { render } from "@testing-library/react";
import type { ReactElement } from "react";
import { createQueryClient, Providers } from "../app/Providers";
import type { Language } from "../i18n/language";

/** Renders a component with the same providers as the real app, and no retries so errors show immediately. */
export function renderWithProviders(ui: ReactElement, { language = "en" }: { language?: Language } = {}) {
  const queryClient = createQueryClient();
  queryClient.setDefaultOptions({ queries: { retry: false } });

  return render(
    <Providers queryClient={queryClient} language={language}>
      {ui}
    </Providers>,
  );
}
