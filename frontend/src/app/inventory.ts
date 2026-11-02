import { CurrencyPipe } from '@angular/common';
import { Component, DestroyRef, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize, Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Product } from './models';
import { StockRealtimeService } from './stock-realtime.service';

@Component({
  selector: 'app-inventory',
  imports: [CurrencyPipe, FormsModule],
  templateUrl: './inventory.html',
  styleUrl: './inventory.css',
})
export class Inventory {
  readonly products = signal<Product[]>([]);
  readonly total = signal(0);
  readonly loading = signal(false);
  readonly error = signal('');
  readonly busyProductId = signal<number | null>(null);
  search = '';
  quantity = 1;

  private readonly api = inject(ApiService);
  private readonly realtime = inject(StockRealtimeService);
  private readonly destroyRef = inject(DestroyRef);

  constructor() {
    this.realtime.stockUpdated
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(({ productId, stock }) => this.patchStock(productId, stock));
    this.realtime.refreshRequested
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.load());

    this.load();
    void this.realtime.start().catch(() => this.error.set('Live updates are unavailable.'));
  }

  load(): void {
    this.loading.set(true);
    this.error.set('');
    this.api
      .products(this.search)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (result) => {
          this.products.set(result.items);
          this.total.set(result.total);
        },
        error: () => this.error.set('Could not load inventory.'),
      });
  }

  order(product: Product): void {
    const quantity = Math.max(1, Math.floor(this.quantity));
    this.runAction(
      product.id,
      this.api.placeOrder(product.id, quantity, this.realtime.connectionId),
      'Order could not be placed.',
    );
  }

  restock(product: Product): void {
    const quantity = Math.max(1, Math.floor(this.quantity));
    this.runAction(product.id, this.api.restock(product.id, quantity), 'Product could not be restocked.');
  }

  stockState(product: Product): 'critical' | 'watch' | 'healthy' {
    if (product.stock < product.reorderThreshold) return 'critical';
    if (product.stock < product.reorderThreshold * 2) return 'watch';
    return 'healthy';
  }

  private runAction(productId: number, action: Observable<unknown>, message: string): void {
    this.busyProductId.set(productId);
    this.error.set('');
    action.pipe(finalize(() => this.busyProductId.set(null))).subscribe({
      next: () => this.load(),
      error: () => this.error.set(message),
    });
  }

  private patchStock(productId: number, stock: number): void {
    this.products.update((products) =>
      products.map((product) => (product.id === productId ? { ...product, stock } : product)),
    );
  }
}
