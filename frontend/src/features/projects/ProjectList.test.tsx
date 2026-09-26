import { screen } from "@testing-library/react";
import { afterEach, describe, expect, it, vi } from "vitest";
import type { Project } from "../../api/client";
import { renderWithProviders } from "../../test/render";
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

    renderWithProviders(<ProjectList />);

    expect(await screen.findByRole("heading", { name: "CV website" })).toBeInTheDocument();
    expect(screen.getByText("React")).toBeInTheDocument();
    expect(screen.getByRole("link", { name: "Source code" })).toHaveAttribute("href", project.repositoryUrl);
    expect(screen.getByText(/2026 – present/)).toBeInTheDocument();
  });

  it("requests projects in the current language and shows Swedish UI texts", async () => {
    vi.mocked(fetchProjects).mockResolvedValue([project]);

    renderWithProviders(<ProjectList />, { language: "sv" });

    expect(await screen.findByRole("link", { name: "Källkod" })).toBeInTheDocument();
    expect(screen.getByText(/pågående/)).toBeInTheDocument();
    expect(fetchProjects).toHaveBeenCalledWith("sv", expect.anything());
  });

  it("shows an error when the API fails", async () => {
    vi.mocked(fetchProjects).mockRejectedValue(new Error("boom"));

    renderWithProviders(<ProjectList />);

    expect(await screen.findByRole("alert")).toBeInTheDocument();
  });
});
