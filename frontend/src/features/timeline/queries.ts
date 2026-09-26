import { useLocalizedQuery } from "../../api/useLocalizedQuery";
import { fetchEducation, fetchExperience } from "./timelineApi";

export const useExperience = () => useLocalizedQuery("experience", fetchExperience);
export const useEducation = () => useLocalizedQuery("education", fetchEducation);
