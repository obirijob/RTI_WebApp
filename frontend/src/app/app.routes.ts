import { Routes } from '@angular/router';
import { Board } from './board';
import { Inventory } from './inventory';

export const routes: Routes = [
  { path: 'inventory', component: Inventory },
  { path: 'board', component: Board },
  { path: '', pathMatch: 'full', redirectTo: 'inventory' },
  { path: '**', redirectTo: 'inventory' },
];
