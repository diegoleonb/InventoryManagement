// Product model and DTOs for inventory management
export interface Product {
  id: number;
  name: string;
  description: string;
  stockQuantity: number;
  price: number;
  pictureUrl: string;
  categoryId: number;
  categoryName: string;
  createdAt: string;
  lastUpdatedAt: string;
  createdByUser: string;
}

export interface ProductCreateDto {
  name: string;
  description: string;
  stockQuantity: number;
  price: number;
  pictureUrl: string;
  categoryId: number;
}

export interface ProductUpdateDto {
  name: string;
  description: string;
  stockQuantity: number;
  price: number;
  pictureUrl: string;
  categoryId: number;
}
