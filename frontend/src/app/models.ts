export interface Product {
  id: number;
  name: string;
  sku: string;
  category: string;
  unitPrice: number;
  stock: number;
  reorderThreshold: number;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}

export interface LowStockAlert {
  productId: number;
  productName: string;
  stock: number;
  reorderThreshold: number;
}
