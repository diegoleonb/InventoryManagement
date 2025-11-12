import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../../core/services/product.service';
import { CategoryService } from '../../../core/services/category.service';
import { ProductCreateDto, ProductUpdateDto } from '../../../shared/models/product';
import { Category } from '../../../shared/models/category';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.scss']
})
export class ProductFormComponent implements OnInit {
  productForm: FormGroup;
  categories: Category[] = [];
  isEdit = false;
  productId: number | null = null;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.productForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', [Validators.required, Validators.minLength(10)]],
      stockQuantity: [0, [Validators.required, Validators.min(0)]],
      price: [0, [Validators.required, Validators.min(0.01)]],
      pictureUrl: ['', [Validators.required]],
      categoryId: [null, [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.loadCategories();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEdit = true;
        this.productId = +params['id'];
        this.loadProduct(this.productId);
      }
    });
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe(categories => {
      this.categories = categories;
    });
  }

  loadProduct(id: number): void {
    this.productService.getProduct(id).subscribe(product => {
      this.productForm.patchValue({
        name: product.name,
        description: product.description,
        stockQuantity: product.stockQuantity,
        price: product.price,
        pictureUrl: product.pictureUrl,
        categoryId: product.categoryId
      });
    });
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      this.isLoading = true;

      if (this.isEdit && this.productId) {
        const productData: ProductUpdateDto = this.productForm.value;
        this.productService.updateProduct(this.productId, productData).subscribe({
          next: () => {
            this.router.navigate(['/products']);
          },
          error: () => {
            this.isLoading = false;
            alert('Error updating product');
          }
        });
      } else {
        const productData: ProductCreateDto = this.productForm.value;
        this.productService.createProduct(productData).subscribe({
          next: () => {
            this.router.navigate(['/products']);
          },
          error: () => {
            this.isLoading = false;
            alert('Error creating product');
          }
        });
      }
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.productForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched) : false;
  }
}
