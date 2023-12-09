import { Injectable, inject } from '@angular/core';
import { MessageService } from './messages.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable, of } from 'rxjs';
import { Power } from '../interfaces/power';

@Injectable({
  providedIn: 'root',
})
export class HeroPowerService {
  messageService = inject(MessageService);
  httpClient = inject(HttpClient);
  private powerUrl = environment.heroesApiUrl + '/api/powers';

  getPower(id: string): Observable<Power | null> {
    try {
      const power = this.httpClient.get<Power>(`${this.powerUrl}/${id}`);
      this.messageService.add(`HeroPowerService: fetch power id=${id}`);
      return power;
    } catch (error: any) {
      this.messageService.add(`HeroPowerService error: fetch power id=${id}`);
      console.error(error.message);
      return of(null);
    }
  }

  getPowers(): Observable<Power[] | null> {
    try {
      const powers = this.httpClient.get<Power[]>(this.powerUrl);
      this.messageService.add(`HeroPowerService: fetch powers`);
      return powers;
    } catch (error: any) {
      this.messageService.add(`HeroPowerService error: fetch powers`);
      console.error(error.message);
      return of(null);
    }
  }

  addHero(power: Power): Promise<boolean> {
    return new Promise((resolve) => {
      try {
        this.httpClient.post(this.powerUrl, power).subscribe(() => {
          this.messageService.add(`HeroPowerService: add power ${power.id}`);
          resolve(true);
        });
      } catch (error: any) {
        this.messageService.add(`HeroPowerService error: add power ${power.id}`);
        console.error(error.message);
        resolve(false);
      }
    });
  }

  updateHero(power: Power): Observable<Power | null> {
    try {
      const updatedPower = this.httpClient.put<Power>(this.powerUrl, power);
      this.messageService.add(`HeroPowerService: update power id=${power.id}`);
      return updatedPower;
    } catch (error: any) {
      this.messageService.add(`HeroPowerService error: update power id=${power.id}`);
      console.error(error.message);
      return of(null);
    }
  }

  removeHero(id: string): Promise<boolean> {
    return new Promise((resolve) => {
      try {
        this.httpClient.delete(this.powerUrl).subscribe(() => {
          this.messageService.add(`HeroPowerService: remove power id=${id}`);
          resolve(true);
        });
      } catch (error: any) {
        this.messageService.add(`HeroPowerService error: remove power id=${id}`);
        console.error(error.message);
        resolve(false);
      }
    });
  }
}
