import { test, expect } from "@playwright/test";

/**
 * End-to-end test: browse books and read a review.
 *
 * Use case: A visitor lands on the BookStore home page, sees a list of
 * books, clicks one to open the detail view, and reads a review.
 *
 * This test demonstrates the full Author -> Book -> Review data chain
 * as experienced by a real user.
 *
 * Prerequisites:
 *  - Backend API running on http://localhost:5000 with seed data loaded
 *  - Vite dev server running on http://localhost:5173 (started by Playwright)
 */
test("user can browse books and view a book's reviews", async ({ page }) => {
  // ── Step 1: navigate to the app ──────────────────────────────────────
  await page.goto("/");

  // The page heading should be visible
  await expect(page.getByRole("heading", { name: /BookStore/i })).toBeVisible();

  // ── Step 2: the book list is rendered ────────────────────────────────
  // Wait for the "Books" section heading to appear (data has loaded)
  await expect(page.getByRole("heading", { name: "Books", exact: true })).toBeVisible();

  // The seeded book "The Lord of the Rings" should be in the list as a button
  await expect(page.getByRole("button", { name: "The Lord of the Rings" })).toBeVisible();

  // ── Step 3: click the book to open its detail view ───────────────────
  await page.getByRole("button", { name: /The Lord of the Rings/i }).click();

  // The detail heading should appear
  await expect(
    page.getByRole("heading", { name: "The Lord of the Rings" })
  ).toBeVisible();

  // ── Step 4: the author is shown ──────────────────────────────────────
  await expect(page.getByText("J.R.R. Tolkien")).toBeVisible();

  // ── Step 5: reviews section is visible and contains seeded data ───────
  await expect(page.getByRole("heading", { name: "Reviews" })).toBeVisible();
  await expect(page.getByText("An epic masterpiece!")).toBeVisible();

  // ── Step 6: navigate back to the list ────────────────────────────────
  await page.getByRole("button", { name: /Back to list/i }).click();
  await expect(page.getByRole("heading", { name: "Books", exact: true })).toBeVisible();
});
