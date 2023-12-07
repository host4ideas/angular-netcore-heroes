import { Routes } from '@angular/router';
import { PROTECTED_ROUTES, ROUTES } from './routes';
import { HeroDetailComponent } from '../components/hero-detail/hero-detail.component';
import { NewHeroComponent } from '../components/new-hero/new-hero.component';

export const authRoutes: Routes = [
  {
    path: PROTECTED_ROUTES.detail + '/:id',
    component: HeroDetailComponent,
  },
  {
    path: PROTECTED_ROUTES.newHero,
    component: NewHeroComponent,
  },
];
