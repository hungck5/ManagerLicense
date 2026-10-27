import { useEffect, useState } from "react";
import { Button } from "@/components/button";
import CardBox from "@/components/cardBox";
import { Column } from "@/components/table/Column";
import { Table } from "@/components/table";
import Modal from "@/components/modal";
import ArticleModal from "./ArticleModal";
import { Article, createArticle, deleteArticle, getArticles, SaveArticle, updateArticle } from "@/services/articleService";

export default function Articles() {
  const [articles, setArticles] = useState<Article[]>([]);
  const [selected, setSelected] = useState<Article | undefined>();
  const [modalOpen, setModalOpen] = useState(false);
  const [previewing, setPreviewing] = useState<Article | undefined>();
  const [loading, setLoading] = useState(true);
  const load = async () => { setLoading(true); try { setArticles(await getArticles()); } finally { setLoading(false); } };
  useEffect(() => { void load(); }, []);

  const save = async (article: SaveArticle) => {
    if (selected) await updateArticle(selected.id, article); else await createArticle(article);
    await load();
  };
  const remove = async (article: Article) => {
    if (window.confirm(`Delete "${article.title}"?`)) { await deleteArticle(article.id); await load(); }
  };
  const isPublished = (article: Article) => article.status === 1 || article.status === "Published";

  return <CardBox title="Articles">
    <div className="space-y-4">
      <div className="flex justify-end"><Button onClick={() => { setSelected(undefined); setModalOpen(true); }}>+ New article</Button></div>
      <Table keyField="id" data={articles} emptyText={loading ? "Loading..." : "No articles"}>
        <Column header="Title" accessor="title" />
        <Column header="Slug" accessor="slug" />
        <Column header="Status" accessor="status" render={(value) => value === 1 || value === "Published" ? "Published" : "Draft"} />
        <Column header="Actions" accessor="id" render={(_, row: Article) => <div className="flex gap-2">
          <Button size="sm" variant="outline" onClick={() => setPreviewing(row)}>Preview</Button>
          <Button size="sm" variant="outline" onClick={() => { setSelected(row); setModalOpen(true); }}>Edit</Button>
          <Button size="sm" variant="destructive" onClick={() => void remove(row)}>Delete</Button>
        </div>} />
      </Table>
    </div>
    <ArticleModal isOpen={modalOpen} onClose={() => setModalOpen(false)} onSubmit={save} article={selected} />
    <Modal isOpen={Boolean(previewing)} onClose={() => setPreviewing(undefined)} title="Article preview" size="lg" footer={<Button variant="outline" onClick={() => setPreviewing(undefined)}>Close</Button>}>
      {previewing && <article className="article-preview mx-auto max-h-[65vh] max-w-3xl overflow-y-auto px-2">
        <p className="mb-2 text-sm font-medium text-blue-600">{isPublished(previewing) ? "Published article" : "Draft preview — only visible in admin"}</p>
        <h1>{previewing.title}</h1>
        {previewing.excerpt && <p className="article-preview__excerpt">{previewing.excerpt}</p>}
        <p className="mb-6 text-sm text-gray-500">{previewing.author}</p>
        <div className="article-preview__content" dangerouslySetInnerHTML={{ __html: previewing.content }} />
      </article>}
    </Modal>
  </CardBox>;
}
