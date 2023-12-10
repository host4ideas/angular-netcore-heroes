import { Injectable, inject } from '@angular/core';
import { BlockBlobClient } from '@azure/storage-blob';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './messages.service';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BlobService {
  private readonly API_URL = '/api/blobheroes';
  httpClient = inject(HttpClient);
  messageService = inject(MessageService);

  getHeroImageUrl(fileName: string): Observable<string | null> {
    try {
      const imageUrl = this.httpClient.get<string>(
        `${this.API_URL}/${fileName}`
      );
      this.messageService.add('BlobService: fetch hero image');
      return imageUrl;
    } catch (error: any) {
      this.messageService.add('BlobService error: fetch hero image');
      console.error(error.message);
      return of(null);
    }
  }

  listHeroesImages(): Observable<BlockBlobClient[] | null> {
    try {
      const imageList = this.httpClient.get<BlockBlobClient[]>(this.API_URL);
      this.messageService.add('BlobService: fetch heroes images');
      return imageList;
    } catch (error: any) {
      this.messageService.add('BlobService error: fetch heroes images');
      console.error(error.message);
      return of(null);
    }
  }

  removeHeroImage(fileName: string): Promise<boolean> {
    return new Promise((resolve) => {
      try {
        this.httpClient.delete(`${this.API_URL}/${fileName}`).subscribe(() => {
          this.messageService.add(`BlobService: remove hero image=${fileName}`);
          resolve(true);
        });
      } catch (error: any) {
        this.messageService.add(
          `BlobService error: remove hero image=${fileName}`
        );
        console.error(error.message);
        resolve(false);
      }
    });
  }

  upsertHeroImage(fileName: string, file: File): Promise<string | null> {
    return new Promise((resolve) => {
      try {
        const formData = new FormData();
        formData.append('file', file, fileName);
        this.httpClient
          .post<string>(this.API_URL, formData)
          .subscribe((imageUrl) => {
            this.messageService.add(`BlobService: uploading ${fileName}`);
            resolve(imageUrl);
          });
      } catch (error: any) {
        this.messageService.add(`BlobService error: uploading ${fileName}`);
        console.error(error.message);
        resolve(null);
      }
    });
  }
}
