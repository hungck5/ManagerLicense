import React, { useMemo, useState, useCallback } from "react";
import { createEditor, Descendant, Transforms, Element as SlateElement } from "slate";
import { Slate, Editable, withReact, useSlate } from "slate-react";
import type { RenderLeafProps, RenderElementProps } from "slate-react";
import { Editor, Text } from 'slate';
import { Bold, Italic,Underline } from "lucide-react";
import { Button } from "./Button";
import { COMPOSE_PLEASE_HOLDER } from "@/constants/app-constants";

enum FormatTextEnum {
  Bold = "bold",
  Italic = "italic",
  Underline = "underline",
}

const FORMAT_TEXT_LIST = [FormatTextEnum.Bold, FormatTextEnum.Italic, FormatTextEnum.Underline] as const;
type FormatText = (typeof FORMAT_TEXT_LIST)[number];

interface ToggleMarkProps {
  event: React.MouseEvent;
  editor: Editor;
  format: FormatText;
  setMarks: React.Dispatch<React.SetStateAction<MarksType>>;
}
type MarksType = {
  [key in FormatText]?: boolean;
}

export function ComposeEditor() {
  
  const [marks, setMarks] = useState<MarksType>({});
  const [value, setValue] = useState<Descendant[]>([
    {
      type: "paragraph",
      children: [{ text: "" }],
    },
  ]);
  
  const editor = useMemo(() => withReact(createEditor()), []);
  const renderLeaf = useCallback((props: RenderLeafProps) => <Leaf {...props} />, []);
  const renderElement = useCallback((props: RenderElementProps) => <Element {...props} />, []);

  return (
    <div className="border rounded-lg p-4 shadow-sm space-y-2 border-gray-300">
      <Slate editor={editor} initialValue={value}
        onChange={(newValue: any) => setValue(newValue)}>
        <Toolbar marks={marks} setMarks={setMarks} />
        <Editable
          renderElement={renderElement}
          renderLeaf={renderLeaf}
          className="min-h-[150px] p-2 border rounded focus:outline-none border-gray-300"
          onKeyDown={() => {
            if (marks.bold) {
              Editor.addMark(editor, FormatTextEnum.Bold, true);
            }
            if (marks.italic) {
              Editor.addMark(editor, FormatTextEnum.Italic, true);
            }
            if (marks.underline) {
              Editor.addMark(editor, FormatTextEnum.Underline, true);
            } 
          }}
          placeholder={COMPOSE_PLEASE_HOLDER}
        />
      </Slate>
    </div>
  );
}

function Toolbar({ marks, setMarks }: {
   marks: MarksType; 
   setMarks: React.Dispatch<React.SetStateAction<MarksType>> 
  }) {
  const editor = useSlate();
  const setBlock = (type: string) => { 
    const isActive = isBlockActive(editor, type); 
    Transforms.setNodes(
      editor,
      { type: isActive ? "paragraph" : type }, 
      { match: (n) => SlateElement.isElement(n) && Editor.isBlock(editor, n) }
    );
  };

  return (
    <div className="flex gap-2 border-b border-gray-200 p-2">
      {/* <Button onMouseDown={(e) => toggleMark({ event: e, editor, format: 'bold', setMarks })}
        className={`p-1 rounded hover:bg-gray-200 ${
          marks.bold ? "bg-gray-300 text-black" : "text-gray-500"
        }`}>
          <Bold size={18} />
      </Button>
      <Button onMouseDown={(e) => toggleMark({ event: e, editor, format: 'italic', setMarks })}
        className={`p-1 rounded hover:bg-gray-200 ${
          marks.italic ? "bg-gray-300 text-black" : "text-gray-500"
        }`}>
          <Italic size={18} />
      </Button> */}
      {(FORMAT_TEXT_LIST).map((format) => (
        <Button
          key={format}
          onMouseDown={(e) => toggleMark({ event: e, editor, format, setMarks })}
          className={`p-1 rounded hover:bg-gray-200 ${marks[format] ? "bg-gray-300 text-black" : "text-gray-500"}`}
        >
          {format === FormatTextEnum.Bold && <Bold size={18} />}
          {format === FormatTextEnum.Italic && <Italic size={18} />}
          {format === FormatTextEnum.Underline  && <Underline size={18} />}
        </Button>
      ))}

      {Array.from({ length: 6 }, (_, i) => ( 
        <Button 
          key={i} 
          onMouseDown={(e) => { 
            e.preventDefault(); 
            setBlock(`h${i + 1}`); 
          }} 
          className="text-xs px-2 py-1 rounded hover:bg-gray-200 text-gray-600" 
        > 
          H{i + 1} 
        </Button> 
      ))} 
    </div>
  );
}

function toggleMark({ event, editor, format, setMarks }: ToggleMarkProps) {
  event.preventDefault();

  const isActive = isMarkActive(editor, format);
  const updateMarks = (active: boolean) => setMarks((prev) => ({ ...prev, [format]: active }));

  if (isActive) {
    Editor.removeMark(editor, format);
    updateMarks(false);
  } else {
    Editor.addMark(editor, format, true);
    updateMarks(true);
  }
}

function isMarkActive(editor: Editor, format: FormatText) {
  const [match] = Editor.nodes(editor, {
    match: (n) => Text.isText(n) && n[format] === true,
    universal: true,
  });

  return !!match;
}

function isBlockActive(editor: Editor, format: string) { 
  const [match] = Editor.nodes(editor, { 
    match: (n) => !Editor.isEditor(n) && SlateElement.isElement(n) && n.type === format, 
  }); 
  return !!match; 
}

const Leaf = ({ attributes, children, leaf }: any) => {
  if (leaf.bold) children = <strong>{children}</strong>;
  if (leaf.italic) children = <em>{children}</em>;
  if (leaf.underline) children = <u>{children}</u>;
  return <span {...attributes}>{children}</span>;
};

const Element = ({ attributes, children, element }: RenderElementProps) => {
  switch (element.type) { 
    case 'h1': return <h1 className="text-3xl font-bold" {...attributes}>{children}</h1>;
    case 'h2': return <h2 className="text-2xl font-bold" {...attributes}>{children}</h2>;
    case 'h3': return <h3 className="text-xl font-bold" {...attributes}>{children}</h3>;
    case 'h4': return <h4 className="text-lg font-bold" {...attributes}>{children}</h4>;
    case 'h5': return <h5 className="text-base font-bold" {...attributes}>{children}</h5>;
    case 'h6': return <h6 className="text-sm font-bold" {...attributes}>{children}</h6>;
    default: return <p {...attributes}>{children}</p>;
  }
};