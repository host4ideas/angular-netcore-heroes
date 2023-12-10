import { Injectable, inject } from '@angular/core';
import { Hero, NewHeroForm } from '../interfaces/hero';
import { Observable, of } from 'rxjs';
import { MessageService } from './messages.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class HeroService {
  httpClient = inject(HttpClient);
  messageService = inject(MessageService);
  private readonly HEROES_URL = environment.heroesApiUrl + '/api/heroes';

  getHero(id: string): Observable<Hero | null> {
    try {
      const hero = this.httpClient.get<Hero>(`${this.HEROES_URL}/${id}`);
      this.messageService.add(`HeroService: fetch hero id=${id}`);
      return hero;
    } catch (error: any) {
      this.messageService.add(`HeroService error: fetch hero id=${id}`);
      console.error(error.message);
      return of(null);
    }
  }

  getHeroes(): Observable<Hero[] | null> {
    try {
      const heroes = this.httpClient.get<Hero[]>(this.HEROES_URL);
      this.messageService.add(`HeroService: fetch heroes`);
      return heroes;
    } catch (error: any) {
      this.messageService.add(`HeroService error: fetch heroes`);
      console.error(error.message);
      return of(null);
    }
  }

  addHero(hero: NewHeroForm): Promise<boolean> {
    return new Promise((resolve) => {
      try {
        this.httpClient.post(this.HEROES_URL, hero).subscribe(() => {
          this.messageService.add(`HeroService: add hero ${hero.name}`);
          resolve(true);
        });
      } catch (error: any) {
        this.messageService.add(`HeroService error: add hero ${hero.name}`);
        console.error(error.message);
        resolve(false);
      }
    });
  }

  updateHero(hero: Hero): Observable<Hero | null> {
    try {
      const updatedHero = this.httpClient.put<Hero>(this.HEROES_URL, hero);
      this.messageService.add(`HeroService: update hero id=${hero.id}`);
      return updatedHero;
    } catch (error: any) {
      this.messageService.add(`HeroService error: update hero id=${hero.id}`);
      console.error(error.message);
      return of(null);
    }
  }

  removeHero(id: string): Promise<boolean> {
    return new Promise((resolve) => {
      try {
        this.httpClient.delete(`${this.HEROES_URL}/${id}`).subscribe(() => {
          this.messageService.add(`HeroService: remove hero id=${id}`);
          resolve(true);
        });
      } catch (error: any) {
        this.messageService.add(`HeroService error: remove hero id=${id}`);
        console.error(error.message);
        resolve(false);
      }
    });
  }
}
