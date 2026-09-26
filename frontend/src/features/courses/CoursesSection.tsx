import { useState } from "react";
import type { Course, CourseOverview } from "../../api/client";
import { AsyncContent } from "../../components/AsyncContent";
import { Section } from "../../components/Section";
import { useLanguage, useTranslations } from "../../i18n/LanguageContext";
import { formatMonth, formatNumber } from "../../lib/format";
import { useCourses } from "./queries";

export function CoursesSection() {
  const t = useTranslations();

  return (
    <Section id="courses" title={t.nav.courses}>
      <AsyncContent query={useCourses()}>{(overview) => <Courses overview={overview} />}</AsyncContent>
    </Section>
  );
}

function Courses({ overview }: { overview: CourseOverview }) {
  const { language } = useLanguage();
  const t = useTranslations();
  const [showAll, setShowAll] = useState(false);

  const keyCourses = overview.courses.filter((c) => c.highlighted);

  return (
    <>
      <p className="muted">{t.creditsCompleted(formatNumber(overview.totalCredits, language), overview.courses.length)}</p>

      {keyCourses.length > 0 && (
        <>
          <h3 className="subheading">{t.keyCourses}</h3>
          <ul className="key-courses">
            {keyCourses.map((course) => (
              <li key={course.name}>{course.name}</li>
            ))}
          </ul>
        </>
      )}

      <button
        type="button"
        className="text-button"
        aria-expanded={showAll}
        aria-controls="all-courses"
        onClick={() => setShowAll((value) => !value)}
      >
        {showAll ? t.hideAllCourses : t.showAllCourses(overview.courses.length)}
      </button>

      {showAll && (
        <ul id="all-courses" className="course-list" aria-label={t.allCourses}>
          {overview.courses.map((course) => (
            <CourseRow key={`${course.name}-${course.completedOn}`} course={course} />
          ))}
        </ul>
      )}
    </>
  );
}

function CourseRow({ course }: { course: Course }) {
  const { language } = useLanguage();
  const t = useTranslations();

  return (
    <li className="course-row">
      <span className="course-name">{course.name}</span>
      <span className="course-meta muted small">
        {t.courseLevel[course.level]} · {formatNumber(course.credits, language)} {t.creditsUnit} ·{" "}
        {formatMonth(course.completedOn, language)}
      </span>
    </li>
  );
}
