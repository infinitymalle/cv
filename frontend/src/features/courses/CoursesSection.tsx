import { useId, useState } from "react";
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
              <li key={courseKey(course)} className="key-course">
                <CourseItem course={course} />
              </li>
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
            <li key={courseKey(course)} className="course-row">
              <CourseItem course={course} showMeta />
            </li>
          ))}
        </ul>
      )}
    </>
  );
}

const courseKey = (course: Course) => course.code ?? `${course.name}-${course.completedOn}`;

/** A course name that expands to show its summary when clicked (if it has one). */
function CourseItem({ course, showMeta = false }: { course: Course; showMeta?: boolean }) {
  const { language } = useLanguage();
  const t = useTranslations();
  const [open, setOpen] = useState(false);
  const summaryId = useId();

  const heading = (
    <>
      <span className="course-name">{course.name}</span>
      {showMeta && (
        <span className="course-meta muted small">
          {course.code && <>{course.code} · </>}
          {t.courseLevel[course.level]} · {formatNumber(course.credits, language)} {t.creditsUnit} ·{" "}
          {formatMonth(course.completedOn, language)}
        </span>
      )}
    </>
  );

  if (!course.summary) {
    return <div className="course-heading">{heading}</div>;
  }

  return (
    <>
      <button
        type="button"
        className="course-toggle"
        aria-expanded={open}
        aria-controls={summaryId}
        onClick={() => setOpen((value) => !value)}
      >
        <span className="course-heading">{heading}</span>
        <span className="chevron" aria-hidden="true" />
      </button>
      {open && (
        <p id={summaryId} className="course-summary">
          {course.summary}
        </p>
      )}
    </>
  );
}
