import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { afterEach, describe, expect, it, vi } from "vitest";
import type { CourseOverview } from "../../api/client";
import { renderWithProviders } from "../../test/render";
import { fetchCourses } from "./coursesApi";
import { CoursesSection } from "./CoursesSection";

vi.mock("./coursesApi");

const overview: CourseOverview = {
  totalCredits: 22.5,
  courses: [
    {
      name: "Real-Time Systems",
      code: "D0003E",
      summary: "Scheduling and concurrency in embedded systems.",
      credits: 15,
      level: "advanced",
      completedOn: "2022-05-30",
      highlighted: true,
    },
    {
      name: "Discrete Mathematics",
      code: null,
      summary: null,
      credits: 7.5,
      level: "basic",
      completedOn: "2021-01-13",
      highlighted: false,
    },
  ],
};

describe("CoursesSection", () => {
  afterEach(() => vi.resetAllMocks());

  it("shows total credits and key courses, with the full list collapsed", async () => {
    vi.mocked(fetchCourses).mockResolvedValue(overview);

    renderWithProviders(<CoursesSection />);

    expect(await screen.findByText("22.5 credits · 2 completed courses")).toBeInTheDocument();
    expect(screen.getByText("Real-Time Systems")).toBeInTheDocument();
    expect(screen.queryByText("Discrete Mathematics")).not.toBeInTheDocument();
  });

  it("expands a course to show its summary when clicked", async () => {
    const user = userEvent.setup();
    vi.mocked(fetchCourses).mockResolvedValue(overview);
    renderWithProviders(<CoursesSection />);

    const course = await screen.findByRole("button", { name: /Real-Time Systems/ });
    expect(course).toHaveAttribute("aria-expanded", "false");
    expect(screen.queryByText(/Scheduling and concurrency/)).not.toBeInTheDocument();

    await user.click(course);

    expect(course).toHaveAttribute("aria-expanded", "true");
    expect(screen.getByText(/Scheduling and concurrency/)).toBeInTheDocument();
  });

  it("expands to show all courses; courses without a summary are not clickable", async () => {
    const user = userEvent.setup();
    vi.mocked(fetchCourses).mockResolvedValue(overview);
    renderWithProviders(<CoursesSection />);

    await user.click(await screen.findByRole("button", { name: "Show all 2 courses" }));

    expect(screen.getByText("Discrete Mathematics")).toBeInTheDocument();
    expect(screen.queryByRole("button", { name: /Discrete Mathematics/ })).not.toBeInTheDocument();
    expect(screen.getByText(/D0003E · Advanced level · 15 credits/)).toBeInTheDocument();
  });

  it("uses Swedish number format and texts", async () => {
    vi.mocked(fetchCourses).mockResolvedValue(overview);

    renderWithProviders(<CoursesSection />, { language: "sv" });

    expect(await screen.findByText("22,5 hp · 2 avklarade kurser")).toBeInTheDocument();
    expect(screen.getByRole("heading", { name: "Nyckelkurser" })).toBeInTheDocument();
  });
});
