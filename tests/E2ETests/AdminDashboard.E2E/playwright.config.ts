import { defineConfig, devices } from "@playwright/test";
import { existsSync, readFileSync } from "node:fs";
import { dirname, resolve } from "node:path";
import { fileURLToPath } from "node:url";

const projectDirectory = dirname(fileURLToPath(import.meta.url));
const environmentFile = resolve(projectDirectory, "..", "..", "..", ".env.e2e.test");

if (existsSync(environmentFile)) {
  for (const line of readFileSync(environmentFile, "utf8").split(/\r?\n/)) {
    const separator = line.indexOf("=");
    if (separator < 1 || line.trimStart().startsWith("#")) continue;
    const key = line.slice(0, separator).trim();
    const value = line.slice(separator + 1).trim().replace(/^(["'])(.*)\1$/, "$2");
    if (key && process.env[key] === undefined) process.env[key] = value;
  }
}

// AuthServer seeds the E2E admin with ADMIN_PASSWORD. Use that password for the browser login.
process.env.E2E_ADMIN_PASSWORD ??= process.env.ADMIN_PASSWORD;

export default defineConfig({
  testDir: "./e2e",
  testMatch: "**/*.live.spec.ts",
  forbidOnly: Boolean(process.env.CI),
  retries: process.env.CI ? 2 : 0,
  reporter: process.env.CI ? "github" : [["list"], ["html", { open: "never" }]],
  use: {
    baseURL: process.env.E2E_BASE_URL ?? "http://localhost:3000",
    trace: "on-first-retry",
  },
  projects: [{ name: "chromium", use: { ...devices["Desktop Chrome"] } }],
});
