import { Injectable, inject } from '@angular/core';
import { HEROES } from '../mock/mock-heroes';
import { Hero } from '../interfaces/hero';
import { Observable, of } from 'rxjs';
import { MessageService } from './messages.service';

@Injectable({
  providedIn: 'root',
})
export class HeroService {
  messageService = inject(MessageService);

  getHero(id: number): Observable<Hero> {
    const hero = HEROES.find((h) => h.id === id)!;
    this.messageService.add(`HeroService: fetched hero id=${id}`);
    return of(hero);
  }

  getHeroes(): Observable<Hero[]> {
    this.messageService.add('HeroService: fetched heroes');
    return of(HEROES);
  }
}
