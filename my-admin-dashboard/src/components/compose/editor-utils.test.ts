import { describe, expect, it } from "vitest";
import { contentToHtml, htmlToContent } from "./editor-utils";

describe("article content HTML conversion", () => {
  it("serializes an uploaded image as a responsive HTML image", () => {
    const html = contentToHtml([{ type: "image", url: "https://api.example/article-media/articles/photo.jpg", alt: "Cover", children: [{ text: "" }] }]);

    expect(html).toBe('<figure><img src="https://api.example/article-media/articles/photo.jpg" alt="Cover" loading="lazy"></figure>');
  });

  it("restores an uploaded image from saved HTML", () => {
    const content = htmlToContent('<p>Hello</p><figure><img src="https://api.example/article-media/articles/photo.jpg" alt="Cover"></figure>');

    expect(content).toEqual([
      { type: "paragraph", children: [{ text: "Hello" }] },
      { type: "image", url: "https://api.example/article-media/articles/photo.jpg", alt: "Cover", children: [{ text: "" }] },
    ]);
  });

  it("does not deserialize an image URL with an unsafe protocol", () => {
    expect(htmlToContent('<img src="javascript:alert(1)">')).toEqual([{ type: "paragraph", children: [{ text: "" }] }]);
  });
});
