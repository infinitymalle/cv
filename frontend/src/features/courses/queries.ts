import { useLocalizedQuery } from "../../api/useLocalizedQuery";
import { fetchCourses } from "./coursesApi";

export const useCourses = () => useLocalizedQuery("courses", fetchCourses);
