import { Injectable, inject } from '@angular/core';
import { MessageService } from './messages.service';
import { NewUser, User } from '../interfaces/user';
import { BehaviorSubject, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  public userSubject$: BehaviorSubject<User | null> =
    new BehaviorSubject<User | null>(null);

  messageService = inject(MessageService);

  login(userName: string, password: string): void {
    const user: User = {
      name: userName,
      userName: userName,
    };
    this.userSubject$.next(user);
    window.localStorage.setItem('user', JSON.stringify(user));
    this.messageService.add(`User ${user.userName} logged`);
  }

  singUp(newUser: NewUser) {
    window.localStorage.setItem('user', JSON.stringify(newUser));
    this.userSubject$.next({
      name: newUser.name,
      userName: newUser.userName,
    });
    this.messageService.add(`User ${this.userSubject$.getValue()} logged`);
  }

  logout() {
    window.localStorage.removeItem('user');
    this.userSubject$.next(null);
  }

  getUser(): User | null {
    // if in-memory username
    const currentUser = this.userSubject$.getValue();
    if (currentUser) return currentUser;
    // if in localStorage or undefined
    const strLocalStorageUser = window.localStorage.getItem('user');
    if (!strLocalStorageUser) return null;
    const localStorageUser = JSON.parse(strLocalStorageUser);
    this.userSubject$.next(localStorageUser);
    return localStorageUser;
  }

  currentIsLogged(): boolean {
    return this.getUser() !== null;
  }
}
