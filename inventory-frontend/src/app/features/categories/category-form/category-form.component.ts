import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../../core/services/category.service';
import { CategoryCreateDto, CategoryUpdateDto } from '../../../shared/models/category';

@Component({
  selector: 'app-category-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './category-form.component.html',
  styleUrls: ['./category-form.component.scss']
})
export class CategoryFormComponent implements OnInit {
  categoryForm: FormGroup;
  isEdit = false;
  categoryId: number | null = null;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.categoryForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEdit = true;
        this.categoryId = +params['id'];
        this.loadCategory(this.categoryId);
      }
    });
  }

  loadCategory(id: number): void {
    this.categoryService.getCategory(id).subscribe(category => {
      this.categoryForm.patchValue({
        name: category.name
      });
    });
  }

  onSubmit(): void {
    if (this.categoryForm.valid) {
      this.isLoading = true;

      if (this.isEdit && this.categoryId) {
        const categoryData: CategoryUpdateDto = this.categoryForm.value;
        this.categoryService.updateCategory(this.categoryId, categoryData).subscribe({
          next: () => {
            this.router.navigate(['/categories']);
          },
          error: () => {
            this.isLoading = false;
            alert('Error updating category');
          }
        });
      } else {
        const categoryData: CategoryCreateDto = this.categoryForm.value;
        this.categoryService.createCategory(categoryData).subscribe({
          next: () => {
            this.router.navigate(['/categories']);
          },
          error: () => {
            this.isLoading = false;
            alert('Error creating category');
          }
        });
      }
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.categoryForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched) : false;
  }
}
