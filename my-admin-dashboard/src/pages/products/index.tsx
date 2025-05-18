import { Button } from "@/components/button";
import CardBox from "@/components/cardBox";
import { Input } from "@/components/input";
import { Table } from "@/components/table";
import { Column } from "@/components/table/Column";
import { useEffect, useState } from "react";
import { v4 as uuidv4 } from 'uuid';
import ProductFormModal from "./ProductModal";
import { getProducts, Product } from "@/services/productService";

const mockData: Product[] = [
  { id: uuidv4(), name: "Apple", price: 1.2, stock: 10 },
  { id: uuidv4(), name: "Banana", price: 0.8, stock: 20 },
  { id: uuidv4(), name: "Orange", price: 1.5, stock: 15 },
];

const productsMock: Product[] = [
    { id: uuidv4(), name: "Product A", price: 100, stock: 5 },
    { id: uuidv4(), name: "Product B", price: 150, stock: 10 },
  ];
type ModalMode = 'create' | 'edit' | null;

export default function Products() {
  const [modalMode, setModalMode] = useState<ModalMode>(null);
  //const [isModalOpen, setIsModalOpen] = useState(false);
  const [search, setSearch] = useState("");
  const [products, setProducts] = useState<Product[]>(productsMock);
  const [selectedProduct, setSelectedProduct] = useState<{ name: string; price: number } | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await getProducts();
        setProducts(data);
      } catch (error) {
        console.error("Failed to fetch products", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, []);

  const filteredData = mockData.filter((p) =>
    p.name.toLowerCase().includes(search.toLowerCase())
  );

  const handleCreateProduct = (product: { name: string; price: number; stock: number }) => {
    const newProduct = {
      id: uuidv4(),
      ...product,
    };
    
    setProducts((prev) => [...prev, newProduct]);
  };


  const handleCreate = (product: { name: string; price: number }) => {
    console.log("Create product:", product);
    setModalMode(null);
    // Gọi API tạo
  };

  const handleEdit = (product: { name: string; price: number }) => {
    console.log("Edit product:", product);
    setModalMode(null);
    // Gọi API cập nhật
  };

  const openCreateModal = () => {
    setSelectedProduct(null);
    setModalMode("create");
  };

  const openEditModal = (product: Product) => {
    setSelectedProduct(product);
    setModalMode("edit");
  };

  const closeModal = () => {
    setModalMode(null);
    setSelectedProduct(null);
  };

  return (
    <>
      <CardBox title="Products" >
        <div className="space-y-4">
          <div className="flex items-center justify-between gap-4">
            <Input
              placeholder="Search product..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="max-w-sm"
            />
            <Button onClick={openCreateModal}>
              + Create
            </Button>
          </div>
          <Table keyField="id" data={products}>
            <Column header="ID" accessor="id" />
            <Column header="Name" accessor="name" />
            <Column header="Price" accessor="price" render={(v) => `$${v}`} />
            <Column
              header="Actions"
              accessor="actions"
              render={(_, row: Product) => (
                <div className="flex gap-2">
                  <Button
                    size="sm"
                    variant="outline"
                    onClick={() => openEditModal(row)}
                  >
                    Edit
                  </Button>
                  <Button
                    size="sm"
                    variant="destructive"
                    onClick={() => alert(`Delete ${row.name}`)}
                  >
                    Delete
                  </Button>
                </div>
              )}
            /> 
          </Table>
        </div>

        <ProductFormModal
          isOpen={modalMode !== null}
          onClose={closeModal}
          onSubmit={modalMode === "create" ? handleCreate : handleEdit}
          initialData={selectedProduct ?? { name: "", price: 0 }}
          mode={modalMode === "edit" ? "edit" : "create"}
        />

      </CardBox>
    </>
  );
}