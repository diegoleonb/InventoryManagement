// Data Transfer Objects and interfaces for category management
export interface Category {
  id: number;
  name: string;
  productCount: number;
}

export interface CategoryCreateDto {
  name: string;
}

export interface CategoryUpdateDto {
  name: string;
}
