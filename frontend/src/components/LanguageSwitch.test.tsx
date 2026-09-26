import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { beforeEach, describe, expect, it } from "vitest";
import { detectInitialLanguage } from "../i18n/language";
import { renderWithProviders } from "../test/render";
import { LanguageSwitch } from "./LanguageSwitch";

describe("LanguageSwitch", () => {
  beforeEach(() => localStorage.clear());

  it("is a switch that is on for English", () => {
    renderWithProviders(<LanguageSwitch />, { language: "en" });

    expect(screen.getByRole("switch")).toHaveAttribute("aria-checked", "true");
  });

  it("toggles between Swedish and English, updates <html lang> and remembers the choice", async () => {
    const user = userEvent.setup();
    renderWithProviders(<LanguageSwitch />, { language: "en" });

    await user.click(screen.getByRole("switch"));

    expect(screen.getByRole("switch")).toHaveAttribute("aria-checked", "false");
    expect(document.documentElement.lang).toBe("sv");
    expect(detectInitialLanguage()).toBe("sv");

    await user.click(screen.getByRole("switch"));

    expect(document.documentElement.lang).toBe("en");
    expect(detectInitialLanguage()).toBe("en");
  });

  it("can be operated with the keyboard", async () => {
    const user = userEvent.setup();
    renderWithProviders(<LanguageSwitch />, { language: "sv" });

    await user.tab();
    await user.keyboard("{Enter}");

    expect(screen.getByRole("switch")).toHaveAttribute("aria-checked", "true");
  });
});
