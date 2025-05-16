import { useEffect, useState } from "react";
import Modal from "@/components/modal";

interface ProductFormModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (product: { name: string; price: number }) => void;
  initialData?: {
    name: string;
    price: number;
  };
  mode: "create" | "edit";
}

export default function ProductFormModal({
  isOpen,
  onClose,
  onSubmit,
  initialData = { name: "", price: 0 },
  mode,
}: ProductFormModalProps) {
  const [name, setName] = useState(initialData.name);
  const [price, setPrice] = useState(initialData.price);

  useEffect(() => {
    if (isOpen) {
      setName(initialData.name);
      setPrice(initialData.price);
    }
  }, [isOpen, initialData]);

  const handleSubmit = () => {
    onSubmit({ name, price });
    onClose();
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title={mode === "create" ? "Create Product" : "Edit Product"}
      footer={
        <>
          <button
            onClick={onClose}
            className="bg-gray-300 hover:bg-gray-400 text-black px-4 py-2 rounded"
          >
            Cancel
          </button>
          <button
            onClick={handleSubmit}
            className={`${
              mode === "create"
                ? "bg-green-500 hover:bg-green-600"
                : "bg-blue-500 hover:bg-blue-600"
            } text-white px-4 py-2 rounded`}
          >
            {mode === "create" ? "Create" : "Save"}
          </button>
        </>
      }
    >
      <div className="space-y-4">
        <div>
          <label className="block text-sm font-medium mb-1">Name</label>
          <input
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="w-full border border-gray-300 rounded px-3 py-2"
            placeholder="Product name"
          />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Price</label>
          <input
            type="number"
            value={price}
            onChange={(e) => setPrice(Number(e.target.value))}
            className="w-full border border-gray-300 rounded px-3 py-2"
            placeholder="Product price"
          />
        </div>
      </div>
    </Modal>
  );
}
