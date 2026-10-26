import api from "@/lib/api";

export type ArticleStatus = "Draft" | "Published" | 0 | 1;

export interface Article {
  id: string;
  title: string;
  slug: string;
  excerpt: string;
  content: string;
  metaTitle: string;
  metaDescription: string;
  canonicalUrl: string;
  author: string;
  status: ArticleStatus;
  createdAt: string;
  updatedAt: string;
  publishedAt?: string | null;
}

export type SaveArticle = Omit<Article, "id" | "createdAt" | "updatedAt" | "publishedAt">;

export const getArticles = async () => (await api.get<Article[]>("/articles")).data;
export const createArticle = async (article: SaveArticle) => (await api.post<Article>("/articles", article)).data;
export const updateArticle = async (id: string, article: SaveArticle) => (await api.put<Article>(`/articles/${id}`, article)).data;
export const deleteArticle = async (id: string) => api.delete(`/articles/${id}`);
