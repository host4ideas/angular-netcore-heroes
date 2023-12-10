import { environment } from '../../environments/environment';

export const validateImageFile = (inputImageFile: File): boolean => {
  // Check if the selected file is of the supported image types
  const supportedTypes = ['image/jpeg', 'image/jpg', 'image/webp', 'image/png'];
  if (!supportedTypes.includes(inputImageFile.type)) {
    return false;
  }
  return true;
};

export const limitString = (inputString: string, maxLength: number) => {
  return inputString.length > maxLength
    ? inputString.slice(0, maxLength)
    : inputString;
};

export const getUrlFromBlobName = (
  fileName: string,
  containerBlobName: string
): string => {
  return `${environment.azureStorageAccountUrl}/${containerBlobName}/${fileName}`;
};
