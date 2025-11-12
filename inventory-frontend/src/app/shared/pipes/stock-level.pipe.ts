import { Pipe, PipeTransform } from '@angular/core';

// Pipe to evaluate stock levels and return corresponding status strings
@Pipe({
  name: 'stockLevel',
  standalone: true
})
export class StockLevelPipe implements PipeTransform {
  transform(stock: number): string {
    if (stock === 0) return 'out-of-stock';
    if (stock <= 10) return 'low-stock';
    if (stock <= 25) return 'medium-stock';
    return 'high-stock';
  }
}
