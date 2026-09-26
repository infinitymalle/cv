import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { useState, type ReactNode } from "react";
import { LanguageProvider } from "../i18n/LanguageProvider";
import type { Language } from "../i18n/language";

export function createQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: 5 * 60 * 1000, // CV content rarely changes; don't refetch constantly
        refetchOnWindowFocus: false,
        retry: 1,
      },
    },
  });
}

/** Everything the app needs around it. Tests use this too, so they run with the real setup. */
export function Providers({
  children,
  queryClient,
  language,
}: {
  children: ReactNode;
  queryClient?: QueryClient;
  language?: Language;
}) {
  const [client] = useState(() => queryClient ?? createQueryClient());

  return (
    <QueryClientProvider client={client}>
      <LanguageProvider initial={language}>{children}</LanguageProvider>
    </QueryClientProvider>
  );
}
