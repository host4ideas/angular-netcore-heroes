import { Component, inject } from '@angular/core';
import { MessageService } from '../../services/messages.service';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.scss',
})
export class MessagesComponent {
  messageService = inject(MessageService);
}
