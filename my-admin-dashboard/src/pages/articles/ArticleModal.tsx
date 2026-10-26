import { useEffect, useState } from "react";
import Modal from "@/components/modal";
import { Article, SaveArticle } from "@/services/articleService";
import { ArticleContentEditor } from "@/components/compose";

interface Props {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (article: SaveArticle) => Promise<void>;
  article?: Article;
}

const emptyArticle: SaveArticle = { title: "", slug: "", excerpt: "", content: "", metaTitle: "", metaDescription: "", canonicalUrl: "", author: "", status: 0 };

export default function ArticleModal({ isOpen, onClose, onSubmit, article }: Props) {
  const [data, setData] = useState<SaveArticle>(emptyArticle);
  const [saving, setSaving] = useState(false);
  useEffect(() => {
    setData(article ? { title: article.title, slug: article.slug, excerpt: article.excerpt, content: article.content, metaTitle: article.metaTitle, metaDescription: article.metaDescription, canonicalUrl: article.canonicalUrl, author: article.author, status: article.status === "Published" ? 1 : article.status === "Draft" ? 0 : article.status } : emptyArticle);
  }, [article, isOpen]);
  const update = (key: keyof SaveArticle, value: string) => setData((current) => ({ ...current, [key]: value }));
  const save = async () => {
    if (!data.title.trim() || !data.content.trim()) return;
    setSaving(true);
    try { await onSubmit(data); onClose(); } finally { setSaving(false); }
  };
  return <Modal isOpen={isOpen} onClose={onClose} title={article ? "Edit article" : "New article"} size="lg" footer={<><button onClick={onClose} className="px-4 py-2 rounded bg-gray-200">Cancel</button><button disabled={saving} onClick={save} className="px-4 py-2 rounded bg-blue-600 text-white disabled:opacity-50">{saving ? "Saving..." : "Complete"}</button></>}>
    <div className="grid gap-3 max-h-[65vh] overflow-y-auto pr-2">
      <label>Title<input required value={data.title} onChange={(e) => update("title", e.target.value)} className="w-full border rounded p-2" /></label>
      <label>Slug <span className="text-xs text-gray-500">(leave blank to create automatically)</span><input value={data.slug} onChange={(e) => update("slug", e.target.value)} className="w-full border rounded p-2" /></label>
      <label>Excerpt<textarea value={data.excerpt} onChange={(e) => update("excerpt", e.target.value)} className="w-full border rounded p-2" rows={2} /></label>
      <div><span className="mb-1 block">Content</span><ArticleContentEditor key={article?.id ?? "new"} initialHtml={data.content} onChange={(content) => setData((current) => ({ ...current, content }))} /></div>
      <div className="grid md:grid-cols-2 gap-3"><label>Author<input value={data.author} onChange={(e) => update("author", e.target.value)} className="w-full border rounded p-2" /></label><label>Status<select value={String(data.status)} onChange={(e) => setData((current) => ({ ...current, status: Number(e.target.value) as 0 | 1 }))} className="w-full border rounded p-2"><option value="0">Draft</option><option value="1">Published</option></select></label></div>
      <fieldset className="border rounded p-3 grid gap-3"><legend className="px-1 font-medium">SEO</legend><label>SEO title <span className="text-xs text-gray-500">(max 60)</span><input maxLength={60} value={data.metaTitle} onChange={(e) => update("metaTitle", e.target.value)} className="w-full border rounded p-2" /></label><label>Meta description <span className="text-xs text-gray-500">(max 160)</span><textarea maxLength={160} value={data.metaDescription} onChange={(e) => update("metaDescription", e.target.value)} className="w-full border rounded p-2" rows={2} /></label><label>Canonical URL<input value={data.canonicalUrl} onChange={(e) => update("canonicalUrl", e.target.value)} className="w-full border rounded p-2" /></label></fieldset>
    </div>
  </Modal>;
}
