import React, { useMemo, useState, useCallback } from "react";
import { createEditor, Descendant } from "slate";
import { Slate, Editable, withReact, useSlate } from "slate-react";
import type { RenderLeafProps } from "slate-react";
import { Editor, Text } from 'slate';
import { Bold, Italic } from "lucide-react";

function Button({ onMouseDown, children, className }: { onMouseDown: (e: React.MouseEvent) => void; children: React.ReactNode; className?: string }) {
  return (
    <button
      onMouseDown={onMouseDown}
      className={`px-2 py-1 border rounded bg-gray-100 hover:bg-gray-200 ${className}`}
    >
      {children}
    </button>
  );
}

// type CustomText = { text: string; bold?: boolean; italic?: boolean, [key: string]: any;};
// declare module 'slate' {
//   interface CustomTypes {
//     Editor: BaseEditor & ReactEditor;
//     Element: { type: 'paragraph'; children: CustomText[] };
//     Text: CustomText;
//   }
// }

export function ComposeEditor() {
  const editor = useMemo(() => withReact(createEditor()), []);
  const [value, setValue] = useState<Descendant[]>([
    {
      type: "paragraph",
      children: [{ text: "Soạn tin nhắn tại đây..." }],
    },
  ]);

  //const renderElement = useCallback((props: RenderElementProps) => <DefaultElement {...props} />, []);
  //const renderLeaf = useCallback((props: RenderLeafProps) => <DefaultLeaf {...props} />, []);
  const renderLeaf = useCallback((props: RenderLeafProps) => <Leaf {...props} />, []);
  const [marks, setMarks] = useState<{ bold?: boolean; italic?: boolean }>({});

  return (
    <div className="border rounded-lg p-4 shadow-sm space-y-2 border-gray-300">
      <Slate editor={editor} initialValue={value}
        onChange={(newValue: any) => {
            setValue(newValue);
        
            // const marks = Editor.marks(editor);
            // if (marks?.bold) {
            //   Editor.addMark(editor, "bold", true);
            // }
            // if (marks?.italic) {
            //   Editor.addMark(editor, "italic", true);
            // }
        }}>
        <Toolbar marks={marks} setMarks={setMarks} />
        {/* <Editable
          renderElement={renderElement}
          renderLeaf={renderLeaf}
          placeholder="Soạn nội dung..."
          className="min-h-[120px] focus:outline-none"
        /> */}

        <Editable
          renderLeaf={renderLeaf}
          className="min-h-[150px] p-2 border rounded focus:outline-none border-gray-300"
          onKeyDown={() => {
            // Đảm bảo thêm/loại bỏ mark đúng khi gõ
            if (marks.bold) {
              // Tự động áp dụng bold khi gõ
              Editor.addMark(editor, 'bold', true);
            }
            if (marks.italic) {
              // Tự động áp dụng italic khi gõ
              Editor.addMark(editor, 'italic', true);
            }
          }}
        />
      </Slate>
    </div>
  );
}

// const DefaultElement = (props: RenderElementProps) => {
//   return <p {...props.attributes}>{props.children}</p>;
// };

// const DefaultLeaf = (props: RenderLeafProps) => {
//   return <span {...props.attributes}>{props.children}</span>;
// };

function Toolbar({ marks, setMarks }: { marks: { bold?: boolean; italic?: boolean }; setMarks: React.Dispatch<React.SetStateAction<{ bold?: boolean; italic?: boolean }>> }) {
  const editor = useSlate();
  return (
    <div className="flex gap-2 border-b border-gray-200 p-2">
      {/* <Button onMouseDown={(e) => toggleMark(e, editor, 'bold')}
        className={p-1 rounded hover:bg-gray-200 ${
          isMarkActive(editor, "bold") ? "bg-gray-300 text-black" : "text-gray-500"
        }}>
          <Bold size={18} />
      </Button>
      <Button onMouseDown={(e) => toggleMark(e, editor, 'italic')}
        className={p-1 rounded hover:bg-gray-200 ${
          isMarkActive(editor, "italic") ? "bg-gray-300 text-black" : "text-gray-500"
        }}>
          <Italic size={18} />
      </Button> */}

      <Button onMouseDown={(e) => toggleMark(e, editor, 'bold', setMarks)}
        className={`p-1 rounded hover:bg-gray-200 ${
          marks.bold ? "bg-gray-300 text-black" : "text-gray-500"
        }`}>
          <Bold size={18} />
      </Button>
      <Button onMouseDown={(e) => toggleMark(e, editor, 'italic', setMarks)}
        className={`p-1 rounded hover:bg-gray-200 ${
          marks.italic ? "bg-gray-300 text-black" : "text-gray-500"
        }`}>
          <Italic size={18} />
      </Button>

    </div>
  );
}

function toggleMark(event: React.MouseEvent, editor: Editor, format: 'bold' | 'italic', setMarks: React.Dispatch<React.SetStateAction<{ bold?: boolean; italic?: boolean }>> ) {
  event.preventDefault();
  // const isActive = isMarkActive(editor, format);
  // if (isActive) {
  //   Editor.removeMark(editor, format);
  // } else {
  //   Editor.addMark(editor, format, true);
  // }

  const isActive = isMarkActive(editor, format);
  if (isActive) {
    Editor.removeMark(editor, format);
    setMarks((prev) => ({ ...prev, [format]: false }));
  } else {
    Editor.addMark(editor, format, true);
    setMarks((prev) => ({ ...prev, [format]: true }));
  }
}

function isMarkActive(editor: Editor, format: string) {
  const [match] = Editor.nodes(editor, {
    match: (n) => Text.isText(n) && n[format] === true,
    universal: true,
  });

  return !!match;
}

const Leaf = ({ attributes, children, leaf }: any) => {
  if (leaf.bold) children = <strong>{children}</strong>;
  if (leaf.italic) children = <em>{children}</em>;
  return <span {...attributes}>{children}</span>;
};