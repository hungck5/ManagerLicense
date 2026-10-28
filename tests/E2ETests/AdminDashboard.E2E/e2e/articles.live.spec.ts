import { expect, test } from "@playwright/test";

const password = process.env.E2E_ADMIN_PASSWORD;
const gatewayUrl = (process.env.E2E_GATEWAY_URL ?? "http://localhost:7130").replace(/\/$/, "");
const articlesUrl = `${gatewayUrl}/api/articles`;

test("admin creates, previews, and deletes an article through the deployed API", async ({ page }) => {
  test.skip(!password, "Set E2E_ADMIN_PASSWORD after starting the Docker stack.");

  const title = `E2E article ${Date.now()}`;
  await test.step("Auth: dashboard authenticates through ApiGateway to AuthServer", async () => {
    await page.goto("/articles");
    const authorizeRequest = page.waitForRequest(request =>
      request.url().startsWith(`${gatewayUrl}/connect/authorize`));
    await page.getByRole("button", { name: "Sign in" }).click();
    await authorizeRequest;
    await page.getByPlaceholder("Username").fill("admin");
    await page.getByPlaceholder("Password").fill(password!);
    const tokenResponse = page.waitForResponse(response =>
      response.url() === `${gatewayUrl}/connect/token` && response.request().method() === "POST");
    await page.getByRole("button", { name: "Login" }).click();
    await expect((await tokenResponse).status()).toBe(200);
  });

  await test.step("Dashboard loads articles through ApiGateway from ArticleService", async () => {
    const listResponse = page.waitForResponse(response =>
      response.url() === articlesUrl && response.request().method() === "GET");
    await page.goto("/articles");
    await expect((await listResponse).status()).toBe(200);
    await expect(page.getByRole("heading", { name: "Articles" })).toBeVisible();
  });

  await test.step("Dashboard creates an article through ApiGateway to ArticleService", async () => {
    await page.getByRole("button", { name: "+ New article" }).click();
    await page.getByLabel("Title", { exact: true }).fill(title);
    await page.locator('[contenteditable="true"]').fill("Content saved by the live E2E test.");
    await page.getByLabel("Author").fill("E2E Admin");
    await page.getByLabel("Status").selectOption("1");
    const createResponse = page.waitForResponse(response =>
      response.url() === articlesUrl && response.request().method() === "POST");
    await page.getByRole("button", { name: "Complete" }).click();
    await expect((await createResponse).status()).toBe(201);
  });

  const articleRow = page.getByRole("row").filter({ hasText: title });
  await expect(articleRow).toBeVisible();
  await expect(articleRow.getByText("Published", { exact: true })).toBeVisible();

  await test.step("Dashboard previews and deletes the ArticleService record", async () => {
    await articleRow.getByRole("button", { name: "Preview" }).click();
    await expect(page.getByRole("heading", { name: title })).toBeVisible();
    await expect(page.getByText("Content saved by the live E2E test.")).toBeVisible();
    await page.getByRole("button", { name: "Close", exact: true }).click();

    page.once("dialog", dialog => dialog.accept());
    const deleteResponse = page.waitForResponse(response =>
      response.url().startsWith(`${articlesUrl}/`) && response.request().method() === "DELETE");
    await articleRow.getByRole("button", { name: "Delete" }).click();
    await expect((await deleteResponse).status()).toBe(204);
    await expect(articleRow).toHaveCount(0);
  });
});
