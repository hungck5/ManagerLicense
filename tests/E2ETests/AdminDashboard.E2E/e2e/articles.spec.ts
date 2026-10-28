import { expect, test } from "@playwright/test";

type Article = {
  id: string;
  title: string;
  slug: string;
  excerpt: string;
  content: string;
  metaTitle: string;
  metaDescription: string;
  canonicalUrl: string;
  author: string;
  status: number;
  createdAt: string;
  updatedAt: string;
  publishedAt: string | null;
};

test("administrator can create and preview a published article", async ({ page }) => {
  const articles: Article[] = [];
  let createPayload: Partial<Article> | undefined;

  await page.route("http://e2e-api/api/articles", async route => {
    if (route.request().method() === "GET") {
      await route.fulfill({ json: articles });
      return;
    }

    if (route.request().method() === "POST") {
      createPayload = route.request().postDataJSON() as Partial<Article>;
      const now = new Date().toISOString();
      const article: Article = {
        id: "article-1",
        title: createPayload.title ?? "",
        slug: "e2e-published-article",
        excerpt: createPayload.excerpt ?? "",
        content: createPayload.content ?? "",
        metaTitle: createPayload.metaTitle ?? "",
        metaDescription: createPayload.metaDescription ?? "",
        canonicalUrl: createPayload.canonicalUrl ?? "",
        author: createPayload.author ?? "",
        status: createPayload.status ?? 0,
        createdAt: now,
        updatedAt: now,
        publishedAt: createPayload.status === 1 ? now : null,
      };
      articles.push(article);
      await route.fulfill({ status: 201, json: article });
      return;
    }

    await route.fallback();
  });

  await page.goto("/login");
  await page.evaluate(() => {
    localStorage.setItem("oidc.user:http://e2e-auth:e2e-client", JSON.stringify({
      access_token: "e2e-token",
      token_type: "Bearer",
      profile: { sub: "e2e-admin", name: "E2E Admin" },
      expires_at: Math.floor(Date.now() / 1000) + 3600,
    }));
  });

  await page.goto("/articles");
  await expect(page.getByRole("heading", { name: "Articles" })).toBeVisible();
  await expect(page.getByText("No articles")).toBeVisible();

  await page.getByRole("button", { name: "+ New article" }).click();
  await page.getByLabel("Title", { exact: true }).fill("E2E published article");
  await page.locator('[contenteditable="true"]').fill("Article body created by the E2E test.");
  await page.getByLabel("Author").fill("E2E Admin");
  await page.getByLabel("Status").selectOption("1");
  await page.getByRole("button", { name: "Complete" }).click();

  await expect(page.getByRole("button", { name: "Complete" })).toHaveCount(0);
  await expect(page.getByRole("cell", { name: "E2E published article", exact: true })).toBeVisible();
  await expect(page.getByRole("cell", { name: "Published", exact: true })).toBeVisible();
  expect(createPayload).toMatchObject({ title: "E2E published article", author: "E2E Admin", status: 1 });

  await page.getByRole("button", { name: "Preview" }).click();
  await expect(page.getByRole("heading", { name: "E2E published article" })).toBeVisible();
  await expect(page.getByText("Article body created by the E2E test.")).toBeVisible();
  await expect(page.getByText("Published article", { exact: true })).toBeVisible();
});
