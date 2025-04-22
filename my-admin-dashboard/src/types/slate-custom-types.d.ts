import { BaseEditor } from "slate";
import { ReactEditor } from "slate-react";

export type CustomText = { 
  text: string; 
  bold?: boolean; 
  italic?: boolean; 
  [key: string]: any;
};

declare module 'slate' {
  interface CustomTypes {
    Editor: BaseEditor & ReactEditor;
    Element: { type: 'paragraph'; children: CustomText[] };
    Text: CustomText;
  }
}