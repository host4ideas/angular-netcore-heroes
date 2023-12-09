import { Routes } from '@angular/router';
import { HeroesComponent } from '../components/heroes/heroes.component';
import { DashboardComponent } from '../components/dashboard/dashboard.component';
import { ROUTES } from './routes';
import { authGuard } from './guards/CanActivateAuthUser';
import { LoginComponent } from '../components/login/login.component';

export const routes: Routes = [
  { path: '', redirectTo: `/${ROUTES.heroes}`, pathMatch: 'full' },
  { path: ROUTES.dashboard, component: DashboardComponent },
  {
    path: ROUTES.heroes,
    component: HeroesComponent,
  },
  {
    path: 'auth',
    canActivate: [authGuard],
    loadChildren: () => import('./auth.routes').then((r) => r.authRoutes),
  },
  {
    path: ROUTES.login,
    component: LoginComponent,
  },
];
