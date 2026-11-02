import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ApiService } from './api.service';
import { Product } from './models';
import { StockRealtimeService } from './stock-realtime.service';

@Component({
  selector: 'app-board',
  templateUrl: './board.html',
  styleUrl: './board.css',
})
export class Board {
  readonly products = signal<Product[]>([]);
  readonly realtime = inject(StockRealtimeService);
  private readonly api = inject(ApiService);
  private readonly destroyRef = inject(DestroyRef);

  constructor() {
    this.load();
    this.realtime.stockUpdated
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(({ productId, stock }) => {
        this.products.update((products) =>
          products.map((product) => product.id === productId ? { ...product, stock } : product),
        );
      });
    this.realtime.refreshRequested
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.load());
    void this.realtime.start().catch(() => undefined);
  }

  private load(): void {
    this.api.products('', 200).subscribe((result) => {
      this.products.set(result.items.sort((a, b) => a.stock - b.stock));
    });
  }
}
