import { Component, inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  userService? = inject(UserService);
  formBuilder = inject(FormBuilder);
  // loginForm = new FormGroup({
  //   userName: new FormControl(''),
  //   password: new FormControl(''),
  // });

  loginForm = this.formBuilder.group({
    userName: [
      '',
      [Validators.required, Validators.minLength(4), Validators.maxLength(8)],
    ],
    password: [
      '',
      [
        Validators.required,
        Validators.pattern(
          /^(?=\D*\d)(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z]).{8,30}$/
        ),
      ],
    ],
  });

  onSubmit() {
    const userName = this.loginForm.value.userName;
    const password = this.loginForm.value.password;
    if (userName && password && this.loginForm.valid) {
      this.userService?.login(userName, password);
    }
  }
}
