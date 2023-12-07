import { Injectable, inject } from '@angular/core';
import { MessageService } from './messages.service';
import { User } from '../interfaces/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  user?: User | null;
  messageService = inject(MessageService);

  setUsername(user: User): void {
    window.localStorage.setItem('user', JSON.stringify(user));
    this.user = user;
    this.messageService.add(`User ${user} logged`);
  }

  getUser(): User | null {
    // if in-memory username
    if (this.user) return this.user;
    // if in localStorage username or undefined
    const localStorageUser = window.localStorage.getItem('user') as User | null;
    return (this.user = localStorageUser);
  }

  currentIsLogged(): boolean {
    return this.getUser() !== null;
  }
}
