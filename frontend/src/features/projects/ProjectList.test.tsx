import { render, screen } from "@testing-library/react";
import { afterEach, describe, expect, it, vi } from "vitest";
import type { Project } from "../../api/client";
import { ProjectList } from "./ProjectList";
import { fetchProjects } from "./projectsApi";

vi.mock("./projectsApi");

const project: Project = {
  slug: "cv-website",
  title: "CV website",
  summary: "This site.",
  technologies: ["C#", "React"],
  startedOn: "2026-09-01",
  finishedOn: null,
  repositoryUrl: "https://github.com/me/cv",
};

describe("ProjectList", () => {
  afterEach(() => vi.resetAllMocks());

  it("shows projects from the API", async () => {
    vi.mocked(fetchProjects).mockResolvedValue([project]);

    render(<ProjectList />);

    expect(await screen.findByRole("heading", { name: "CV website" })).toBeInTheDocument();
    expect(screen.getByText("React")).toBeInTheDocument();
    expect(screen.getByRole("link", { name: "Source code" })).toHaveAttribute("href", project.repositoryUrl);
  });

  it("shows an error when the API fails", async () => {
    vi.mocked(fetchProjects).mockRejectedValue(new Error("boom"));

    render(<ProjectList />);

    expect(await screen.findByRole("alert")).toBeInTheDocument();
  });
});
