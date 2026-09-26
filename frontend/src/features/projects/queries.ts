import { useLocalizedQuery } from "../../api/useLocalizedQuery";
import { fetchProjects } from "./projectsApi";

export const useProjects = () => useLocalizedQuery("projects", fetchProjects);
