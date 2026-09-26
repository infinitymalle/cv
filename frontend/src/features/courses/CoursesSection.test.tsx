import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { afterEach, describe, expect, it, vi } from "vitest";
import type { Course, CourseOverview } from "../../api/client";
import { renderWithProviders } from "../../test/render";
import { fetchCourses } from "./coursesApi";
import { CoursesSection } from "./CoursesSection";

vi.mock("./coursesApi");

const course = (name: string, overrides: Partial<Course> = {}): Course => ({
  name,
  code: null,
  summary: null,
  credits: 7.5,
  level: "basic",
  highlighted: false,
  ...overrides,
});

// Already in API order (most relevant first). 8 courses, so 2 are hidden initially.
const overview: CourseOverview = {
  totalCredits: 67.5,
  courses: [
    course("Real-Time Systems", {
      code: "D0003E",
      summary: "Scheduling and concurrency in embedded systems.",
      credits: 15,
      level: "advanced",
      highlighted: true,
    }),
    ...["Course 2", "Course 3", "Course 4", "Course 5", "Course 6", "Course 7"].map((name) => course(name)),
    course("Discrete Mathematics"),
  ],
};

describe("CoursesSection", () => {
  afterEach(() => vi.resetAllMocks());

  it("shows the total and the first six courses, one per row, without dates", async () => {
    vi.mocked(fetchCourses).mockResolvedValue(overview);

    renderWithProviders(<CoursesSection />);

    expect(await screen.findByText("67.5 credits · 8 completed courses")).toBeInTheDocument();
    expect(screen.getAllByRole("listitem")).toHaveLength(6);
    expect(screen.getByText("D0003E · Advanced level · 15 credits")).toBeInTheDocument();
    expect(screen.getByText("Key course")).toBeInTheDocument();
    expect(screen.queryByText("Discrete Mathematics")).not.toBeInTheDocument();
    expect(screen.queryByText(/20\d\d/)).not.toBeInTheDocument();
  });

  it("shows the rest when clicking 'Show all', and fewer again", async () => {
    const user = userEvent.setup();
    vi.mocked(fetchCourses).mockResolvedValue(overview);
    renderWithProviders(<CoursesSection />);

    await user.click(await screen.findByRole("button", { name: "Show all 8 courses" }));
    expect(screen.getAllByRole("listitem")).toHaveLength(8);
    expect(screen.getByText("Discrete Mathematics")).toBeInTheDocument();

    await user.click(screen.getByRole("button", { name: "Show fewer courses" }));
    expect(screen.getAllByRole("listitem")).toHaveLength(6);
  });

  it("expands a course to show its summary; courses without a summary are not clickable", async () => {
    const user = userEvent.setup();
    vi.mocked(fetchCourses).mockResolvedValue(overview);
    renderWithProviders(<CoursesSection />);

    const realTime = await screen.findByRole("button", { name: /Real-Time Systems/ });
    expect(realTime).toHaveAttribute("aria-expanded", "false");

    await user.click(realTime);

    expect(realTime).toHaveAttribute("aria-expanded", "true");
    expect(screen.getByText(/Scheduling and concurrency/)).toBeInTheDocument();
    expect(screen.queryByRole("button", { name: /Course 2/ })).not.toBeInTheDocument();
  });

  it("uses Swedish number format and texts", async () => {
    vi.mocked(fetchCourses).mockResolvedValue(overview);

    renderWithProviders(<CoursesSection />, { language: "sv" });

    expect(await screen.findByText("67,5 hp · 8 avklarade kurser")).toBeInTheDocument();
    expect(screen.getByText("D0003E · Avancerad nivå · 15 hp")).toBeInTheDocument();
    expect(screen.getByRole("button", { name: "Visa alla 8 kurser" })).toBeInTheDocument();
  });
});
