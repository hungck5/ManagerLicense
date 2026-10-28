import assert from "node:assert/strict";
import { test } from "node:test";
import { createEditor, Editor, Transforms } from "slate";
import type { Descendant } from "slate";
import { withHistory } from "slate-history";
import { contentToHtml, emptyContent, isBlockActive, isMarkActive, parseDraft, plainText, toggleBlock, toggleMark } from "../../../my-admin-dashboard/src/components/compose/editor-utils";

function setup(content: Descendant[] = [{ type: "paragraph", children: [{ text: "Bài viết" }] }]) {
  const editor = withHistory(createEditor());
  editor.children = structuredClone(content);
  Transforms.select(editor, { path: [0, 0], offset: 0 });
  return editor;
}

test("typing marks follow the cursor instead of retaining toolbar state", () => {
  const editor = setup([
    { type: "paragraph", children: [{ text: "Đậm", bold: true }] },
    { type: "paragraph", children: [{ text: "Thường" }] },
  ]);
  assert.equal(isMarkActive(editor, "bold"), true);
  Transforms.select(editor, { path: [1, 0], offset: 1 });
  assert.equal(isMarkActive(editor, "bold"), false);
  toggleMark(editor, "italic");
  Editor.insertText(editor, "A");
  assert.match(contentToHtml(editor.children), /<em>A<\/em>/);
  toggleMark(editor, "italic");
  assert.equal(isMarkActive(editor, "italic"), false);
});

test("format selected text without changing adjacent text", () => {
  const editor = setup();
  Transforms.select(editor, { anchor: { path: [0, 0], offset: 0 }, focus: { path: [0, 0], offset: 3 } });
  toggleMark(editor, "bold");
  assert.equal(contentToHtml(editor.children), "<p><strong>Bài</strong> viết</p>");
});

test("convert headings back to paragraphs", () => {
  const editor = setup();
  toggleBlock(editor, "h2");
  assert.equal(isBlockActive(editor, "h2"), true);
  toggleBlock(editor, "h2");
  assert.equal(isBlockActive(editor, "paragraph"), true);
});

test("switch list types and remove list formatting without nested wrappers", () => {
  const editor = setup();
  toggleBlock(editor, "bulleted-list");
  assert.equal(contentToHtml(editor.children), "<ul><li>Bài viết</li></ul>");
  toggleBlock(editor, "numbered-list");
  assert.equal(contentToHtml(editor.children), "<ol><li>Bài viết</li></ol>");
  toggleBlock(editor, "numbered-list");
  assert.equal(contentToHtml(editor.children), "<p>Bài viết</p>");
});

test("format several selected paragraphs as a single list", () => {
  const editor = setup([
    { type: "paragraph", children: [{ text: "Một" }] },
    { type: "paragraph", children: [{ text: "Hai" }] },
  ]);
  Transforms.select(editor, { anchor: { path: [0, 0], offset: 0 }, focus: { path: [1, 0], offset: 3 } });
  toggleBlock(editor, "bulleted-list");
  assert.equal(contentToHtml(editor.children), "<ul><li>Một</li><li>Hai</li></ul>");
  assert.equal(plainText(editor.children), "Một\nHai");
});

test("undo and redo a content edit", async () => {
  const editor = setup(emptyContent());
  Editor.insertText(editor, "Bản nháp");
  await Promise.resolve();
  editor.undo();
  assert.equal(plainText(editor.children), "");
  editor.redo();
  assert.equal(plainText(editor.children), "Bản nháp");
});

test("draft JSON preserves Vietnamese text and rich formatting", () => {
  const editor = setup();
  toggleBlock(editor, "blockquote");
  const draft = { version: 1, title: "Tin tức", content: editor.children };
  assert.deepEqual(parseDraft(JSON.stringify(draft)), draft);
  for (const raw of ["{", "null", '{"version":1,"title":"x","content":[]}', JSON.stringify({ ...draft, content: [{ type: "script", children: [{ text: "x" }] }] }), JSON.stringify({ ...draft, content: [{ type: "paragraph", children: [{ type: "paragraph", children: [{ text: "x" }] }] }] })]) {
    assert.equal(parseDraft(raw), null);
  }
});

test("HTML export escapes pasted markup and preserves line breaks", () => {
  const html = contentToHtml([{ type: "paragraph", children: [{ text: '<script>alert("x")</script>\nTiếp theo', bold: true }] }]);
  assert.equal(html, "<p><strong>&lt;script&gt;alert(&quot;x&quot;)&lt;/script&gt;<br>Tiếp theo</strong></p>");
});
