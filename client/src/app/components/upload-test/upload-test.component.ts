import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-upload-test',
  standalone: true,
  imports: [],
  templateUrl: './upload-test.component.html',
  styleUrl: './upload-test.component.scss',
})
export class UploadTestComponent {
  selectedFile: File | undefined;
  http = inject(HttpClient);

  handleFileInput(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onSubmit(): void {
    if (!this.selectedFile) {
      console.error('No file selected');
      return;
    }

    const formData: FormData = new FormData();
    formData.append('file', this.selectedFile, this.selectedFile.name);

    this.http.post('/api/blobheroes', formData).subscribe(
      (response: any) => {
        console.log('File uploaded successfully:', response);
        // Handle success response
      },
      (error: any) => {
        console.error('Error uploading file:', error);
        // Handle error
      }
    );
  }
}
