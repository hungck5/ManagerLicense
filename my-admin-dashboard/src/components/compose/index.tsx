import { useCallback, useEffect, useRef, useState } from "react";
import type { ReactNode } from "react";
import { createEditor, Editor, Element as SlateElement, Range, Node } from "slate";
import type { Descendant } from "slate";
import { Editable, ReactEditor, Slate, useSlate, withReact } from "slate-react";
import type { RenderElementProps, RenderLeafProps } from "slate-react";
import { withHistory } from "slate-history";
import { Bold, Italic, Underline, Strikethrough, List, ListOrdered, Quote, Undo2, Redo2, Download } from "lucide-react";
import { Button } from "./Button";
import { contentToHtml, emptyContent, escapeHtml, htmlToContent, isBlockActive, isList, isMarkActive, parseDraft, plainText, toggleBlock, toggleMark } from "./editor-utils";
import type { ArticleDraft, Mark } from "./editor-utils";
import type { BlockType } from "@/types/slate-custom-types";

type SaveState = "empty" | "restored" | "saving" | "saved" | "error";

function loadDraft(key: string): { draft: ArticleDraft; status: SaveState } {
  const fallback: ArticleDraft = { version: 1, title: "", content: emptyContent() };
  try {
    const raw = localStorage.getItem(key);
    if (!raw) return { draft: fallback, status: "empty" };
    const draft = parseDraft(raw);
    return draft ? { draft, status: "restored" } : { draft: fallback, status: "error" };
  } catch {
    return { draft: fallback, status: "error" };
  }
}

export function ComposeEditor({ draftKey = "article-draft:local" }: { draftKey?: string }) {
  const [initial] = useState(() => loadDraft(draftKey));
  const [title, setTitle] = useState(initial.draft.title);
  const [content, setContent] = useState<Descendant[]>(initial.draft.content);
  const [dirty, setDirty] = useState(false);
  const [saveState, setSaveState] = useState<SaveState>(initial.status);
  const [editor] = useState(() => withReact(withHistory(createEditor())));
  const latest = useRef({ draft: initial.draft, dirty: false });
  const renderLeaf = useCallback((props: RenderLeafProps) => <Leaf {...props} />, []);
  const renderElement = useCallback((props: RenderElementProps) => <Element {...props} />, []);
  const text = plainText(content).trim();
  const wordCount = text ? text.split(/\s+/u).length : 0;

  useEffect(() => {
    latest.current = { draft: { version: 1, title, content }, dirty };
    if (!dirty) return;
    const timer = window.setTimeout(() => {
      try {
        localStorage.setItem(draftKey, JSON.stringify(latest.current.draft));
        setSaveState("saved");
      } catch {
        setSaveState("error");
      }
    }, 700);
    return () => window.clearTimeout(timer);
  }, [content, title, draftKey, dirty]);

  useEffect(() => {
    const flush = () => {
      if (!latest.current.dirty) return;
      try { localStorage.setItem(draftKey, JSON.stringify(latest.current.draft)); }
      catch { /* Storage failure is reported by autosave; HTML export remains available. */ }
    };
    window.addEventListener("pagehide", flush);
    return () => { window.removeEventListener("pagehide", flush); flush(); };
  }, [draftKey]);

  const exportHtml = () => {
    const heading = escapeHtml(title.trim() || "Bài viết chưa có tiêu đề");
    const html = `<!doctype html><html lang="vi"><head><meta charset="utf-8"><title>${heading}</title></head><body><article><h1>${heading}</h1>${contentToHtml(content)}</article></body></html>`;
    const url = URL.createObjectURL(new Blob([html], { type: "text/html;charset=utf-8" }));
    const link = document.createElement("a");
    link.href = url;
    link.download = `${(title.trim() || "bai-viet").replace(/[<>:"/\\|?*]/g, "-").slice(0, 100)}.html`;
    link.click();
    window.setTimeout(() => URL.revokeObjectURL(url), 1000);
  };

  const statuses: Record<SaveState, string> = {
    empty: "Bản nháp được lưu tự động trên trình duyệt này",
    restored: "Đã khôi phục bản nháp trên trình duyệt này",
    saving: "Đang lưu bản nháp…",
    saved: "Đã lưu bản nháp trên trình duyệt này",
    error: "Không thể đọc/lưu bản nháp. Hãy xuất HTML để giữ nội dung.",
  };

  return (
    <section className="overflow-hidden rounded-xl border border-gray-200 bg-white shadow-sm" aria-label="Soạn thảo bài viết">
      <div className="flex flex-wrap items-center justify-between gap-3 border-b border-gray-200 px-5 py-4">
        <div>
          <h2 className="font-semibold text-gray-900">Soạn thảo bài viết</h2>
          <p className="mt-1 text-sm text-gray-500">Viết nội dung và định dạng bài trước khi đăng.</p>
        </div>
        <button type="button" onClick={exportHtml} className="inline-flex items-center gap-2 rounded-md border border-gray-300 px-3 py-2 text-sm hover:bg-gray-50 focus-visible:outline-2 focus-visible:outline-blue-500">
          <Download size={16} /> Xuất HTML
        </button>
      </div>
      <div className="px-5 pt-5">
        <label htmlFor="article-title" className="text-sm font-medium text-gray-700">Tiêu đề bài viết</label>
        <input id="article-title" value={title} onChange={event => { setTitle(event.target.value); setDirty(true); setSaveState("saving"); }}
          placeholder="Nhập tiêu đề bài viết…" className="mt-2 w-full rounded-md border border-gray-300 px-3 py-3 text-lg font-semibold outline-none focus:border-blue-500" />
      </div>
      <Slate editor={editor} initialValue={initial.draft.content} onChange={value => {
        if (editor.operations.some(operation => operation.type !== "set_selection")) {
          setContent(value);
          setDirty(true);
          setSaveState("saving");
        }
      }}>
        <Toolbar />
        <Editable renderLeaf={renderLeaf} renderElement={renderElement}
          aria-label="Nội dung bài viết" placeholder="Bắt đầu viết nội dung bài viết…"
          className="min-h-[360px] px-5 py-4 leading-7 text-gray-800 outline-none"
          onKeyDown={event => {
            if (event.nativeEvent.isComposing) return;
            const key = event.key.toLowerCase();
            if (event.ctrlKey || event.metaKey) {
              const mark = ({ b: "bold", i: "italic", u: "underline" } as Record<string, Mark>)[key];
              if (mark) { event.preventDefault(); toggleMark(editor, mark); }
              else if (key === "z") { event.preventDefault(); if (event.shiftKey) editor.redo(); else editor.undo(); }
              else if (key === "y") { event.preventDefault(); editor.redo(); }
              return;
            }
            if (event.key !== "Enter" || event.shiftKey || !editor.selection || !Range.isCollapsed(editor.selection)) return;
            const entry = Editor.above(editor, { match: node => SlateElement.isElement(node) && Editor.isBlock(editor, node) && !isList(node.type) });
            if (!entry || !SlateElement.isElement(entry[0])) return;
            const [block, path] = entry;
            if (block.type === "list-item" && Node.string(block) === "") {
              event.preventDefault(); toggleBlock(editor, "paragraph");
            } else if ((block.type.startsWith("h") || block.type === "blockquote") && Editor.isEnd(editor, editor.selection.anchor, path)) {
              event.preventDefault(); editor.insertBreak();
              if (!isBlockActive(editor, "paragraph")) toggleBlock(editor, "paragraph");
            }
          }} />
      </Slate>
      <div className="flex flex-wrap items-center justify-between gap-2 border-t border-gray-200 px-5 py-3 text-xs text-gray-500">
        <span role="status" aria-live="polite" className={saveState === "error" ? "text-red-600" : ""}>{statuses[saveState]}</span>
        <span>{wordCount} từ · {text.length} ký tự</span>
      </div>
    </section>
  );
}

/** A controlled, compact version of the dashboard composer for article forms. */
export function ArticleContentEditor({ initialHtml, onChange }: { initialHtml: string; onChange: (html: string) => void }) {
  const [editor] = useState(() => withReact(withHistory(createEditor())));
  const [initial] = useState<Descendant[]>(() => htmlToContent(initialHtml));
  const renderLeaf = useCallback((props: RenderLeafProps) => <Leaf {...props} />, []);
  const renderElement = useCallback((props: RenderElementProps) => <Element {...props} />, []);

  return (
    <div className="overflow-hidden rounded-md border border-gray-300 bg-white">
      <Slate editor={editor} initialValue={initial} onChange={value => {
        if (editor.operations.some(operation => operation.type !== "set_selection")) onChange(contentToHtml(value));
      }}>
        <Toolbar />
        <Editable
          renderLeaf={renderLeaf}
          renderElement={renderElement}
          aria-label="Nội dung bài viết"
          placeholder="Bắt đầu viết nội dung bài viết…"
          className="min-h-64 px-4 py-3 leading-7 text-gray-800 outline-none"
        />
      </Slate>
    </div>
  );
}

function Toolbar() {
  const editor = useSlate();
  const marks = [
    { type: "bold", label: "In đậm (Ctrl+B)", icon: <Bold size={18} /> },
    { type: "italic", label: "In nghiêng (Ctrl+I)", icon: <Italic size={18} /> },
    { type: "underline", label: "Gạch chân (Ctrl+U)", icon: <Underline size={18} /> },
    { type: "strikethrough", label: "Gạch ngang", icon: <Strikethrough size={18} /> },
  ] as const;
  const blocks: { type: BlockType; label: string; icon: ReactNode }[] = [
    { type: "paragraph", label: "Đoạn văn", icon: "P" },
    ...(["h1", "h2", "h3", "h4", "h5", "h6"] as const).map(type => ({ type, label: `Tiêu đề ${type.toUpperCase()}`, icon: type.toUpperCase() })),
    { type: "blockquote", label: "Trích dẫn", icon: <Quote size={18} /> },
    { type: "bulleted-list", label: "Danh sách dấu đầu dòng", icon: <List size={18} /> },
    { type: "numbered-list", label: "Danh sách đánh số", icon: <ListOrdered size={18} /> },
  ];
  const apply = (action: () => void) => { action(); ReactEditor.focus(editor); };
  return (
    <div className="mx-5 mt-5 flex flex-wrap items-center gap-1 border-y border-gray-200 py-2" role="group" aria-label="Định dạng bài viết">
      <Button title="Hoàn tác (Ctrl+Z)" aria-label="Hoàn tác" aria-pressed={undefined} disabled={!editor.history.undos.length} onMouseDown={event => event.preventDefault()} onClick={() => apply(() => editor.undo())}><Undo2 size={18} /></Button>
      <Button title="Làm lại (Ctrl+Shift+Z)" aria-label="Làm lại" aria-pressed={undefined} disabled={!editor.history.redos.length} onMouseDown={event => event.preventDefault()} onClick={() => apply(() => editor.redo())}><Redo2 size={18} /></Button>
      <span className="mx-1 h-5 border-l border-gray-200" aria-hidden="true" />
      {marks.map(mark => <Button key={mark.type} title={mark.label} aria-label={mark.label} active={isMarkActive(editor, mark.type)} disabled={!editor.selection}
        onMouseDown={event => event.preventDefault()} onClick={() => apply(() => toggleMark(editor, mark.type))}>{mark.icon}</Button>)}
      <span className="mx-1 h-5 border-l border-gray-200" aria-hidden="true" />
      {blocks.map(block => <Button key={block.type} title={block.label} aria-label={block.label} active={isBlockActive(editor, block.type)} disabled={!editor.selection}
        onMouseDown={event => event.preventDefault()} onClick={() => apply(() => toggleBlock(editor, block.type))}>{block.icon}</Button>)}
    </div>
  );
}

function Leaf({ attributes, children, leaf }: RenderLeafProps) {
  let formatted = children;
  if (leaf.bold) formatted = <strong>{formatted}</strong>;
  if (leaf.italic) formatted = <em>{formatted}</em>;
  if (leaf.underline) formatted = <u>{formatted}</u>;
  if (leaf.strikethrough) formatted = <s>{formatted}</s>;
  return <span {...attributes}>{formatted}</span>;
}

function Element({ attributes, children, element }: RenderElementProps) {
  switch (element.type) {
    case "h1": return <h1 className="my-4 text-3xl font-bold leading-tight" {...attributes}>{children}</h1>;
    case "h2": return <h2 className="my-4 text-2xl font-bold leading-tight" {...attributes}>{children}</h2>;
    case "h3": return <h3 className="my-3 text-xl font-bold" {...attributes}>{children}</h3>;
    case "h4": return <h4 className="my-3 text-lg font-bold" {...attributes}>{children}</h4>;
    case "h5": return <h5 className="my-2 text-base font-bold" {...attributes}>{children}</h5>;
    case "h6": return <h6 className="my-2 text-sm font-bold" {...attributes}>{children}</h6>;
    case "blockquote": return <blockquote className="my-4 border-l-4 border-blue-300 pl-4 italic text-gray-600" {...attributes}>{children}</blockquote>;
    case "bulleted-list": return <ul className="my-3 list-disc pl-7" {...attributes}>{children}</ul>;
    case "numbered-list": return <ol className="my-3 list-decimal pl-7" {...attributes}>{children}</ol>;
    case "list-item": return <li {...attributes}>{children}</li>;
    default: return <p className="my-2" {...attributes}>{children}</p>;
  }
}
