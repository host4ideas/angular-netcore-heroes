import { Component, inject, afterNextRender } from '@angular/core';
import { UserService } from '../../services/user.service';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  userName?: string;
  userService?: UserService;

  onSubmit(form: NgForm) {
    console.warn(form);
  }
}
