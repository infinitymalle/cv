import createClient from "openapi-fetch";
import type { components, paths } from "./schema";

/**
 * Typed API client. Paths, parameters and response shapes all come from schema.d.ts,
 * which is generated from the backend (npm run gen:api). If the backend changes, this won't compile.
 */
export const api = createClient<paths>({ baseUrl: window.location.origin });

export type Project = components["schemas"]["ProjectDto"];
