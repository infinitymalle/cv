import { useLocalizedQuery } from "../../api/useLocalizedQuery";
import { fetchProfile } from "./profileApi";

// Used by both the hero and the skills section; React Query fetches it only once.
export const useProfile = () => useLocalizedQuery("profile", fetchProfile);
