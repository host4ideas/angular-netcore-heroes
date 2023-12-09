import { Component, inject } from '@angular/core';
import { Hero } from '../../interfaces/hero';
import { NgFor, NgIf, UpperCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeroDetailComponent } from '../hero-detail/hero-detail.component';
import { HeroService } from '../../services/hero.service';
import { OnInit } from '@angular/core';
import { MessageService } from '../../services/messages.service';
import { RouterModule } from '@angular/router';
import { PROTECTED_ROUTES } from '../../router/routes';

@Component({
  selector: 'app-heroes',
  standalone: true,
  imports: [
    UpperCasePipe,
    FormsModule,
    NgFor,
    NgIf,
    HeroDetailComponent,
    RouterModule,
  ],
  templateUrl: './heroes.component.html',
  styleUrl: './heroes.component.scss',
})
export class HeroesComponent implements OnInit {
  heroes?: Hero[];
  selectedHero?: Hero;
  heroService = inject(HeroService);
  messageService = inject(MessageService);
  protectedRoutes = PROTECTED_ROUTES;

  ngOnInit(): void {
    this.getHeroes();
  }

  getHeroes(): void {
    this.heroService
      .getHeroes()
      .subscribe((heroes) => heroes && (this.heroes = heroes));
  }
}
