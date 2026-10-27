import { BaseEditor, Descendant } from "slate";
import { ReactEditor } from "slate-react";
import { HistoryEditor } from "slate-history";

export type CustomText = { 
  text: string; 
  bold?: boolean; 
  italic?: boolean;
  underline?: boolean; 
  strikethrough?: boolean;
};

export type BlockType = "paragraph" | "h1" | "h2" | "h3" | "h4" | "h5" | "h6" | "blockquote" | "bulleted-list" | "numbered-list" | "list-item" | "image";
export type CustomElement = { type: BlockType; children: Descendant[]; url?: string; alt?: string };
declare module 'slate' {
  interface CustomTypes {
    Editor: BaseEditor & ReactEditor & HistoryEditor;
    Element: CustomElement;
    Text: CustomText;
  }
}
