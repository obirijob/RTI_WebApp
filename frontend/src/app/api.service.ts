import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult, Product } from './models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly http = inject(HttpClient);

  products(search = '', pageSize = 200): Observable<PagedResult<Product>> {
    let params = new HttpParams().set('pageSize', pageSize);
    if (search.trim()) params = params.set('search', search.trim());
    return this.http.get<PagedResult<Product>>('/api/products', { params });
  }

  placeOrder(productId: number, quantity: number, connectionId?: string): Observable<{ id: number }> {
    return this.http.post<{ id: number }>('/api/orders', {
      lines: [{ productId, quantity }],
      connectionId,
    });
  }

  restock(productId: number, quantity: number): Observable<void> {
    return this.http.post<void>(`/api/products/${productId}/restock`, { quantity });
  }
}
