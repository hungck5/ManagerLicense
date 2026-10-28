import { defineConfig, devices } from "@playwright/test";
import { dirname, resolve } from "node:path";
import { fileURLToPath } from "node:url";

const projectDirectory = dirname(fileURLToPath(import.meta.url));

export default defineConfig({
  testDir: "./e2e",
  testIgnore: "**/*.live.spec.ts",
  fullyParallel: true,
  forbidOnly: Boolean(process.env.CI),
  retries: process.env.CI ? 2 : 0,
  reporter: process.env.CI ? "github" : [["list"], ["html", { open: "never" }]],
  use: {
    baseURL: "http://127.0.0.1:4180",
    trace: "on-first-retry",
  },
  projects: [{ name: "chromium", use: { ...devices["Desktop Chrome"] } }],
  webServer: {
    command: "pnpm exec vite --host 127.0.0.1 --port 4180",
    cwd: resolve(projectDirectory, "..", "..", "..", "my-admin-dashboard"),
    url: "http://127.0.0.1:4180",
    reuseExistingServer: false,
    env: {
      VITE_E2E: "1",
      VITE_API_BASE_URL: "http://e2e-api/api",
      VITE_AUTHORITY_URL: "http://e2e-auth",
      VITE_CLIENT_ID: "e2e-client",
      VITE_REDIRECT_URI: "http://127.0.0.1:4180/callback",
      VITE_POST_LOGOUT_REDIRECT_URI: "http://127.0.0.1:4180/",
      VITE_RESPONSE_TYPE: "code",
      VITE_SCOPE: "openid profile",
      VITE_CODE_CHALLENGE_METHOD: "S256",
    },
  },
});
