import { defineConfig } from "vitest/config";
import path from "node:path";

const dashboardRoot = path.resolve(__dirname, "../../../my-admin-dashboard");

export default defineConfig({
  resolve: { alias: { "@": path.resolve(dashboardRoot, "src") } },
  test: { environment: "jsdom", include: ["./editor-utils.test.ts"] },
});
