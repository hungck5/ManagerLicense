import { Editor, Element, Node, Text, Transforms } from "slate";
import type { Descendant } from "slate";
import type { BlockType } from "@/types/slate-custom-types";

export type Mark = "bold" | "italic" | "underline" | "strikethrough";
export type ArticleDraft = { version: 1; title: string; content: Descendant[] };
const blockTypes: BlockType[] = ["paragraph", "h1", "h2", "h3", "h4", "h5", "h6", "blockquote", "bulleted-list", "numbered-list", "list-item"];

export function emptyContent(): Descendant[] {
  return [{ type: "paragraph", children: [{ text: "" }] }];
}

export function isList(type: BlockType) {
  return type === "bulleted-list" || type === "numbered-list";
}

export function isMarkActive(editor: Editor, mark: Mark) {
  return Editor.marks(editor)?.[mark] === true;
}

export function toggleMark(editor: Editor, mark: Mark) {
  if (!editor.selection) return;
  if (isMarkActive(editor, mark)) Editor.removeMark(editor, mark);
  else Editor.addMark(editor, mark, true);
}

export function isBlockActive(editor: Editor, type: BlockType) {
  if (!editor.selection) return false;
  const [match] = Editor.nodes(editor, {
    at: Editor.unhangRange(editor, editor.selection),
    match: node => Element.isElement(node) && node.type === type,
  });
  return Boolean(match);
}

export function toggleBlock(editor: Editor, type: BlockType) {
  if (!editor.selection) return;
  const active = isBlockActive(editor, type);
  Editor.withoutNormalizing(editor, () => {
    Transforms.unwrapNodes(editor, {
      match: node => Element.isElement(node) && isList(node.type), split: true,
    });
    Transforms.setNodes(editor, {
      type: active ? "paragraph" : isList(type) ? "list-item" : type,
    }, {
      match: node => Element.isElement(node) && Editor.isBlock(editor, node) && !isList(node.type),
    });
    if (!active && isList(type)) Transforms.wrapNodes(editor, { type, children: [] });
  });
}

function validNode(node: unknown, depth = 0): node is Descendant {
  if (!node || typeof node !== "object" || depth > 12) return false;
  if ("text" in node) {
    return typeof node.text === "string" && ["bold", "italic", "underline", "strikethrough"].every(mark =>
      !(mark in node) || typeof (node as Record<string, unknown>)[mark] === "boolean");
  }
  if (!("type" in node) || !blockTypes.includes(node.type as BlockType) || !("children" in node) ||
      !Array.isArray(node.children) || !node.children.length) return false;
  return node.children.every(child => validNode(child, depth + 1) &&
    (isList(node.type as BlockType) ? Element.isElement(child) && child.type === "list-item" : Text.isText(child)));
}

export function parseDraft(raw: string): ArticleDraft | null {
  try {
    const draft: unknown = JSON.parse(raw);
    if (!draft || typeof draft !== "object" || !("version" in draft) || draft.version !== 1 ||
        !("title" in draft) || typeof draft.title !== "string" || !("content" in draft) ||
        !Array.isArray(draft.content) || !draft.content.length || !draft.content.every(node => validNode(node) && Element.isElement(node) && node.type !== "list-item")) return null;
    return draft as ArticleDraft;
  } catch {
    return null;
  }
}

export function plainText(content: Descendant[]): string {
  return content.map(node => Element.isElement(node) && isList(node.type) ? plainText(node.children) : Node.string(node)).join("\n");
}

export function escapeHtml(text: string) {
  return text.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;").replace(/'/g, "&#39;");
}

export function contentToHtml(content: Descendant[]): string {
  return content.map(node => {
    if (Text.isText(node)) {
      let text = escapeHtml(node.text).replace(/\n/g, "<br>");
      if (node.bold) text = `<strong>${text}</strong>`;
      if (node.italic) text = `<em>${text}</em>`;
      if (node.underline) text = `<u>${text}</u>`;
      if (node.strikethrough) text = `<s>${text}</s>`;
      return text;
    }
    const tags: Record<BlockType, string> = {
      paragraph: "p", h1: "h1", h2: "h2", h3: "h3", h4: "h4", h5: "h5", h6: "h6",
      blockquote: "blockquote", "bulleted-list": "ul", "numbered-list": "ol", "list-item": "li",
    };
    const tag = tags[node.type];
    return `<${tag}>${contentToHtml(node.children) || "<br>"}</${tag}>`;
  }).join("");
}

/** Converts HTML previously emitted by contentToHtml back into Slate blocks. */
export function htmlToContent(html: string): Descendant[] {
  if (!html.trim()) return emptyContent();
  const document = new DOMParser().parseFromString(html, "text/html");
  const deserialize = (node: globalThis.Node, marks: Record<string, boolean> = {}): Descendant[] => {
    if (node.nodeType === globalThis.Node.TEXT_NODE) return [{ text: node.textContent ?? "", ...marks }];
    if (node.nodeType !== globalThis.Node.ELEMENT_NODE) return [];
    const element = node as HTMLElement;
    const tag = element.tagName.toLowerCase();
    const nextMarks = { ...marks };
    if (tag === "strong" || tag === "b") nextMarks.bold = true;
    if (tag === "em" || tag === "i") nextMarks.italic = true;
    if (tag === "u") nextMarks.underline = true;
    if (tag === "s" || tag === "del") nextMarks.strikethrough = true;
    if (tag === "br") return [{ text: "\n", ...marks }];
    const children = Array.from(element.childNodes).flatMap(child => deserialize(child, nextMarks));
    const blockTags: Record<string, BlockType> = { p: "paragraph", h1: "h1", h2: "h2", h3: "h3", h4: "h4", h5: "h5", h6: "h6", blockquote: "blockquote", li: "list-item" };
    if (tag === "ul" || tag === "ol") {
      const listChildren = children.filter(child => Element.isElement(child) && child.type === "list-item");
      return [{ type: tag === "ul" ? "bulleted-list" : "numbered-list", children: listChildren.length ? listChildren : [{ type: "list-item", children: [{ text: "" }] }] } as Descendant];
    }
    if (blockTags[tag]) return [{ type: blockTags[tag], children: children.length ? children : [{ text: "" }] } as Descendant];
    return children;
  };
  const result = Array.from(document.body.childNodes).flatMap(node => deserialize(node));
  const blocks = result.filter(node => Element.isElement(node) && node.type !== "list-item");
  return blocks.length ? blocks : [{ type: "paragraph", children: result.length ? result : [{ text: "" }] }];
}
