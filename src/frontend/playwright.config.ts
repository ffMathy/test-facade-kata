import { defineConfig, devices } from "@playwright/test";

/**
 * Playwright configuration for the BookStore frontend.
 *
 * The test suite automatically starts both servers:
 *  - The React dev server on http://localhost:5173
 *  - The .NET backend API on http://localhost:5000
 *
 * Run with:  npx playwright test
 */
export default defineConfig({
  testDir: "./e2e",
  // Run tests in files in parallel
  fullyParallel: false,
  // Retry failed tests once in CI
  retries: process.env.CI ? 1 : 0,
  use: {
    // Base URL for page.goto("/") calls
    baseURL: "http://localhost:5173",
    // Capture a screenshot on failure to help with debugging
    screenshot: "only-on-failure",
  },
  projects: [
    {
      name: "chromium",
      use: { ...devices["Desktop Chrome"] },
    },
  ],
  // Start both the backend and the Vite dev server before running tests.
  webServer: [
    {
      // Start the .NET API
      command:
        "dotnet run --project ../backend/BookStore.Api/BookStore.Api.csproj --urls http://localhost:5000",
      url: "http://localhost:5000/api/books",
      reuseExistingServer: !process.env.CI,
      timeout: 60_000,
    },
    {
      // Start the Vite dev server
      command: "npm run dev",
      url: "http://localhost:5173",
      reuseExistingServer: !process.env.CI,
      timeout: 30_000,
    },
  ],
});
