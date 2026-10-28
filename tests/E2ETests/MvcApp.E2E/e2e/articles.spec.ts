import { expect, test } from "@playwright/test";

const gatewayUrl = (process.env.E2E_GATEWAY_URL ?? "http://localhost:7130").replace(/\/$/, "");

test("MvcApp renders a published article fetched through ApiGateway", async ({ page, request }) => {
  const title = `MvcApp E2E article ${Date.now()}`;
  let articleId: string | undefined;

  try {
    const createResponse = await request.post(`${gatewayUrl}/api/articles`, {
      data: {
        title,
        content: "<p>Article content served through MvcApp.</p>",
        excerpt: "E2E excerpt",
        author: "E2E",
        status: 1,
      },
    });
    expect(createResponse.status()).toBe(201);
    articleId = (await createResponse.json()).id;

    await page.goto("/bai-viet");
    await page.getByRole("link", { name: title }).click();

    await expect(page.getByRole("heading", { name: title })).toBeVisible();
    await expect(page.getByText("Article content served through MvcApp.")).toBeVisible();
  } finally {
    if (articleId) {
      const deleteResponse = await request.delete(`${gatewayUrl}/api/articles/${articleId}`);
      expect(deleteResponse.status()).toBe(204);
    }
  }
});

