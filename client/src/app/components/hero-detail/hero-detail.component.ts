import { Location, NgIf, UpperCasePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Hero } from '../../interfaces/hero';
import { ActivatedRoute, Router } from '@angular/router';
import { HeroService } from '../../services/hero.service';
import { ROUTES } from '../../router/routes';
import { BlobService } from '../../services/blob.service';

@Component({
  selector: 'app-hero-detail',
  standalone: true,
  imports: [NgIf, UpperCasePipe, FormsModule],
  templateUrl: './hero-detail.component.html',
  styleUrl: './hero-detail.component.scss',
})
export class HeroDetailComponent implements OnInit {
  hero?: Hero;
  router = inject(Router);
  location = inject(Location);
  route = inject(ActivatedRoute);
  heroService = inject(HeroService);
  blobService = inject(BlobService);

  ngOnInit(): void {
    this.getHero();
  }

  getHero() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id)
      this.heroService.getHero(id).subscribe((hero) => {
        if (!hero) return;
        if (!hero.image) {
          this.hero = hero;
        } else {
          this.blobService.getHeroImageUrl(hero.image).subscribe((imageUrl) => {
            if (imageUrl) hero.image = imageUrl;
            this.hero = hero;
          });
        }
      });
  }

  async deleteHero() {
    if (this.hero) {
      await this.heroService.removeHero(this.hero.id);
      this.router.navigate(['/' + ROUTES.heroes]);
    }
  }
}
