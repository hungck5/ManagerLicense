import api from "@/lib/api";

export interface Product {
  id: string;
  name: string;
  price: number;
  stock: number;
}

export const getProducts = async (): Promise<Product[]> => {
  const response = await api.get("/products");
  console.log("response products", response);
  return response.data;
};

export const createProduct = async (data: Omit<Product, "id">) => {
  const response = await api.post("/products", data);
  return response.data;
};

export const updateProduct = async (id: string, data: Partial<Product>) => {
  const response = await api.put(`/products/${id}`, data);
  return response.data;
};

export const deleteProduct = async (id: string) => {
  const response = await api.delete(`/products/${id}`);
  return response.data;
};
