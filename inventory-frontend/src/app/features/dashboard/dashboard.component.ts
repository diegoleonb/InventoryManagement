import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProductService } from '../../core/services/product.service';
import { CategoryService } from '../../core/services/category.service';
import { AuthService } from '../../core/services/auth.service';
import { Product } from '../../shared/models/product';
import { Category } from '../../shared/models/category';
import { StockLevelPipe } from '../../shared/pipes/stock-level.pipe';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, StockLevelPipe],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  lowStockProducts: Product[] = [];
  categories: Category[] = [];
  totalProducts: number = 0;
  totalCategories: number = 0;

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadLowStockProducts();
    this.loadCategories();
  }

  loadLowStockProducts(): void {
    this.productService.getLowStockProducts(10).subscribe(products => {
      this.lowStockProducts = products;
    });

    this.productService.getProducts().subscribe(products => {
      this.totalProducts = products.length;
    });
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe(categories => {
      this.categories = categories;
      this.totalCategories = categories.length;
    });
  }

  getStockClass(stock: number): string {
    if (stock === 0) return 'out-of-stock';
    if (stock <= 10) return 'low-stock';
    if (stock <= 25) return 'medium-stock';
    return 'high-stock';
  }
}
