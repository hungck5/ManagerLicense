import { BaseEditor, Descendant } from "slate";
import { ReactEditor } from "slate-react";

export type CustomText = { 
  text: string; 
  bold?: boolean; 
  italic?: boolean;
  underline?: boolean; 
  [key: string]: any;
};

type CustomElement = { type: "paragraph" | "h1" | "h2" | "h3" | "h4" | "h5" | "h6"; children: Descendant[] };
declare module 'slate' {
  interface CustomTypes {
    Editor: BaseEditor & ReactEditor;
    Element: CustomElement;
    Text: CustomText;
  }
}