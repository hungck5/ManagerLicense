import { Button } from "@/components/button";
import CardBox from "@/components/cardBox";
import { Input } from "@/components/input";
import { Table } from "@/components/table";
import { Column } from "@/components/table/Column";
import { useState } from "react";

interface Product {
  id: number;
  name: string;
  price: number;
  stock: number;
}

const mockData: Product[] = [
  { id: 1, name: "Apple", price: 1.2, stock: 10 },
  { id: 2, name: "Banana", price: 0.8, stock: 20 },
  { id: 3, name: "Orange", price: 1.5, stock: 15 },
];

export default function Products() {
  const [search, setSearch] = useState("");
  const filteredData = mockData.filter((p) =>
    p.name.toLowerCase().includes(search.toLowerCase())
  );

  const products: Product[] = [
    { id: 1, name: "Product A", price: 100, stock: 5 },
    { id: 2, name: "Product B", price: 150, stock: 10 },
  ];

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
            <Button onClick={() => alert("Create product clicked")}>
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
                    onClick={() => alert(`Edit ${row.name}`)}
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
      </CardBox>
    </>
  );
}