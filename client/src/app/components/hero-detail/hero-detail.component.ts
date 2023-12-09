import { Location, NgIf, UpperCasePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Hero } from '../../interfaces/hero';
import { ActivatedRoute, Router } from '@angular/router';
import { HeroService } from '../../services/hero.service';
import { ROUTES } from '../../router/routes';

@Component({
  selector: 'app-hero-detail',
  standalone: true,
  imports: [NgIf, UpperCasePipe, FormsModule],
  templateUrl: './hero-detail.component.html',
  styleUrl: './hero-detail.component.scss',
})
export class HeroDetailComponent implements OnInit {
  hero?: Hero;
  route = inject(ActivatedRoute);
  location = inject(Location);
  heroService = inject(HeroService);
  router = inject(Router);

  ngOnInit(): void {
    this.getHero();
  }

  getHero(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id)
      this.heroService
        .getHero(id)
        .subscribe((hero) => hero && (this.hero = hero));
  }

  async deleteHero() {
    if (this.hero) {
      await this.heroService.removeHero(this.hero.id);
      this.router.navigate(['/' + ROUTES.heroes]);
    }
  }
}
