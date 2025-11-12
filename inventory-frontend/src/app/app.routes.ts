import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

// Define application routes with associated components and guards
export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [authGuard]
  },
  {
    path: 'products',
    loadComponent: () => import('./features/products/product-list/product-list.component').then(m => m.ProductListComponent),
    canActivate: [authGuard]
  },
  {
    path: 'products/new',
    loadComponent: () => import('././features/products/product-form/product-form.component').then(m => m.ProductFormComponent),
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Operator', 'Administrator'] }
  },
  {
    path: 'products/edit/:id',
    loadComponent: () => import('././features/products/product-form/product-form.component').then(m => m.ProductFormComponent),
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Operator', 'Administrator'] }
  },
  {
    path: 'categories',
    loadComponent: () => import('././features/categories/category-list/category-list.component').then(m => m.CategoryListComponent),
    canActivate: [authGuard]
  },
  {
    path: 'categories/new',
    loadComponent: () => import('././features/categories/category-form/category-form.component').then(m => m.CategoryFormComponent),
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Administrator'] }
  },
  {
    path: 'categories/edit/:id',
    loadComponent: () => import('././features/categories/category-form/category-form.component').then(m => m.CategoryFormComponent),
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Administrator'] }
  }
];
