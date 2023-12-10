import {
  AfterRenderPhase,
  Component,
  afterNextRender,
  inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { HeroesComponent } from './components/heroes/heroes.component';
import { MessagesComponent } from './components/messages/messages.component';
import { ROUTES } from './router/routes';
import { AppNavigationLinksComponent } from './components/app-navigation-links/navigation-links.component';
import { UserService } from './services/user.service';
import { User } from './interfaces/user';
import { UploadTestComponent } from './components/upload-test/upload-test.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    HeroesComponent,
    MessagesComponent,
    AppNavigationLinksComponent,
    UploadTestComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'Tour of Heroes';
  routes = ROUTES;
  userService = inject(UserService);
  user: User | null = null;

  constructor() {
    afterNextRender(
      () => {
        this.user = this.userService.getUser();
      },
      {
        phase: AfterRenderPhase.Read,
      }
    );
  }
}
