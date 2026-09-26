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
    { name: "Real-Time Systems", credits: 15, level: "advanced", completedOn: "2022-05-30", highlighted: true },
    { name: "Discrete Mathematics", credits: 7.5, level: "basic", completedOn: "2021-01-13", highlighted: false },
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

  it("expands to show all courses", async () => {
    const user = userEvent.setup();
    vi.mocked(fetchCourses).mockResolvedValue(overview);
    renderWithProviders(<CoursesSection />);

    await user.click(await screen.findByRole("button", { name: "Show all 2 courses" }));

    expect(screen.getByText("Discrete Mathematics")).toBeInTheDocument();
    expect(screen.getByText(/Basic level · 7.5 credits/)).toBeInTheDocument();
  });

  it("uses Swedish number format and texts", async () => {
    vi.mocked(fetchCourses).mockResolvedValue(overview);

    renderWithProviders(<CoursesSection />, { language: "sv" });

    expect(await screen.findByText("22,5 hp · 2 avklarade kurser")).toBeInTheDocument();
    expect(screen.getByRole("heading", { name: "Nyckelkurser" })).toBeInTheDocument();
  });
});
