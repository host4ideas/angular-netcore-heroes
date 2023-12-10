import { ContainerClient } from '@azure/storage-blob';
import { FileValidation } from '../interfaces/validations';

export const downloadBlobImage = async (
  stream: NodeJS.ReadableStream
): Promise<string | null> => {
  const blobData = await new Promise<Blob>((resolve, reject) => {
    const chunks: Uint8Array[] = [];
    stream
      ?.on('data', (chunk) => {
        chunks.push(chunk);
      })
      .on('end', () => {
        resolve(new Blob(chunks));
      })
      .on('error', (error) => {
        reject(error);
      });
  });

  return URL.createObjectURL(blobData); // Convert Blob data to a data URL
};

export const validateImageFile = (inputImageFile: File): boolean => {
  // Check if the selected file is of the supported image types
  const supportedTypes = ['image/jpeg', 'image/jpg', 'image/webp', 'image/png'];
  if (!supportedTypes.includes(inputImageFile.type)) {
    return false;
  }
  return true;
};

export const filterFileName = (
  filename: string,
  maxNameLength = 30
): string => {
  filename
    .toLowerCase()
    .replace(/[^\w\s]/gi, '_')
    .replace(/\s+/g, '_');
  filename = limitString(filename, maxNameLength);
  return filename;
};

export const limitString = (inputString: string, maxLength: number) => {
  return inputString.length > maxLength
    ? inputString.slice(0, maxLength)
    : inputString;
};
