import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { PROTECTED_ROUTES, ROUTES } from '../../router/routes';
import { NgIf } from '@angular/common';
import { UserService } from '../../services/user.service';
import { User } from '../../interfaces/user';

@Component({
  selector: 'app-navigation-links',
  standalone: true,
  imports: [RouterModule, NgIf],
  templateUrl: './navigation-links.component.html',
  styleUrl: './navigation-links.component.scss',
})
export class AppNavigationLinksComponent implements OnInit {
  routes = ROUTES;
  user?: User | null;
  protectedRoutes = PROTECTED_ROUTES;
  router = inject(Router);
  userService = inject(UserService);

  ngOnInit(): void {
    this.userService.userSubject$.subscribe((user) => {
      this.user = user;
    });
  }

  logout() {
    this.userService.logout();
  }
}
