import { Component, inject, afterNextRender } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { PROTECTED_ROUTES, ROUTES } from '../../router/routes';
import { NgIf } from '@angular/common';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-navigation-links',
  standalone: true,
  imports: [RouterModule, NgIf],
  templateUrl: './navigation-links.component.html',
  styleUrl: './navigation-links.component.scss',
})
export class AppNavigationLinksComponent {
  routes = ROUTES;
  protectedRoutes = PROTECTED_ROUTES;
  userService = inject(UserService);
  isUserLogged = false;
  router = inject(Router);

  constructor() {
    afterNextRender(() => {
      // Run only on the client, as it requires access to window
      this.isUserLogged = this.userService.currentIsLogged();
    });
  }
}
