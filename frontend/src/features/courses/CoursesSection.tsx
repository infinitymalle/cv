import { useId, useState } from "react";
import type { Course, CourseOverview } from "../../api/client";
import { AsyncContent } from "../../components/AsyncContent";
import { Section } from "../../components/Section";
import { useLanguage, useTranslations } from "../../i18n/LanguageContext";
import { formatNumber } from "../../lib/format";
import { useCourses } from "./queries";

/** How many courses are visible before "Show all". The API already sorts the most relevant first. */
const initiallyVisible = 6;

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

  const { courses } = overview;
  const visible = showAll ? courses : courses.slice(0, initiallyVisible);
  const hiddenCount = courses.length - initiallyVisible;

  return (
    <>
      <p className="muted">{t.creditsCompleted(formatNumber(overview.totalCredits, language), courses.length)}</p>

      <ul id="course-list" className="course-list">
        {visible.map((course) => (
          <li key={course.code ?? course.name} className="course">
            <CourseRow course={course} />
          </li>
        ))}
      </ul>

      {hiddenCount > 0 && (
        <button
          type="button"
          className="text-button"
          aria-expanded={showAll}
          aria-controls="course-list"
          onClick={() => setShowAll((value) => !value)}
        >
          {showAll ? t.hideAllCourses : t.showAllCourses(courses.length)}
        </button>
      )}
    </>
  );
}

/** One course per row: name and details, expanding to show the summary when clicked. */
function CourseRow({ course }: { course: Course }) {
  const { language } = useLanguage();
  const t = useTranslations();
  const [open, setOpen] = useState(false);
  const summaryId = useId();

  const heading = (
    <span className="course-heading">
      <span className="course-name">
        {course.name}
        {course.highlighted && <span className="badge">{t.keyCourse}</span>}
      </span>
      <span className="course-meta">
        {[course.code, t.courseLevel[course.level], `${formatNumber(course.credits, language)} ${t.creditsUnit}`]
          .filter(Boolean)
          .join(" · ")}
      </span>
    </span>
  );

  if (!course.summary) {
    return <div className="course-row">{heading}</div>;
  }

  return (
    <>
      <button
        type="button"
        className="course-row course-toggle"
        aria-expanded={open}
        aria-controls={summaryId}
        onClick={() => setOpen((value) => !value)}
      >
        {heading}
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
