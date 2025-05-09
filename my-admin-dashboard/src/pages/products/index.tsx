import { Table } from "@/components/table";
import { Column } from "@/components/table/Column";

interface Product {
  id: number;
  name: string;
  price: number;
  stock: number;
}

export default function Products() {
  const products: Product[] = [
    { id: 1, name: "Product A", price: 100, stock: 5 },
    { id: 2, name: "Product B", price: 150, stock: 10 },
  ];
  return (
    <div>
      <h1>Products</h1>
      <p>This is the products page.</p>

      <Table keyField="id" data={products}>
        <Column header="ID" accessor="id" />
        <Column header="Name" accessor="name" />
        <Column header="Price" accessor="price" render={(v) => `$${v}`} />
      </Table>
    </div>
  );
}