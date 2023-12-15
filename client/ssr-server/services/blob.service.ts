import { DefaultAzureCredential, useIdentityPlugin } from '@azure/identity';
import {
  BlobServiceClient,
  BlockBlobClient,
  ContainerClient,
} from '@azure/storage-blob';
import { environment } from '../../src/environments/environment';
import { createUrlFromBlob } from '../helpers/blobHelper';
import { vsCodePlugin } from '@azure/identity-vscode';

useIdentityPlugin(vsCodePlugin);

class BlobService {
  private blobServiceClient?: BlobServiceClient;
  containerHeroesImagesClient?: ContainerClient;

  constructor() {
    const azureStorageAccountUrl = environment.azureStorageAccountUrl;
    if (!azureStorageAccountUrl)
      throw Error('Azure Storage azureStorageAccountUrl not found');

    this.blobServiceClient = new BlobServiceClient(
      azureStorageAccountUrl,
      new DefaultAzureCredential()
    );

    // Initialize containers
    if (this.blobServiceClient)
      this.initHeroesContainer().then(
        (containerClient) =>
          containerClient &&
          (this.containerHeroesImagesClient = containerClient)
      );
  }

  private async initHeroesContainer(): Promise<ContainerClient | null> {
    const containerName = environment.azureBlobHeroesImagesName;

    if (!this.blobServiceClient) {
      console.log(
        `Unable to connect/create container ${containerName}.\n\tBlobServiceClient is undefined`
      );
      return null;
    }

    console.log('\nCreating container...');
    console.log('\t', containerName);

    // Get a reference to a container
    const containerClient =
      this.blobServiceClient.getContainerClient(containerName);
    // Create the container
    const createContainerResponse = await containerClient.createIfNotExists();
    console.log(
      `Container was created successfully.\n\trequestId:${createContainerResponse.requestId}\n\tURL: ${containerClient.url}`
    );

    return containerClient;
  }

  async listBlobClients(
    containerClient: ContainerClient
  ): Promise<BlockBlobClient[]> {
    const blobClients: BlockBlobClient[] = [];

    console.log('\nListing blobs...');

    // List the blob(s) in the container.
    for await (const blob of containerClient.listBlobsFlat()) {
      // Get Blob Client from name, to get the URL
      const tempBlockBlobClient = containerClient.getBlockBlobClient(blob.name);
      blobClients.push(tempBlockBlobClient);

      // Display blob name and URL
      console.log(
        `\n\tname: ${blob.name}\n\tURL: ${tempBlockBlobClient.url}\n`
      );
    }

    return blobClients;
  }

  async getBlobUrl(
    blobName: string,
    containerClient: ContainerClient
  ): Promise<string | null> {
    // Get blob content from position 0 to the end
    // In Node.js, get downloaded data by accessing downloadBlockBlobResponse.readableStreamBody
    // In browsers, get downloaded data by accessing downloadBlockBlobResponse.blobBody
    const blockBlobClient = containerClient.getBlockBlobClient(blobName);
    const downloadBlockBlobResponse = await blockBlobClient.download(0);
    console.log('\nDownloaded blob content...');
    console.log('\t');
    const stream = downloadBlockBlobResponse.readableStreamBody;
    if (!stream) return null;
    return await createUrlFromBlob(stream);
  }

  async deleteBlob(
    blobName: string,
    containerClient: ContainerClient
  ): Promise<boolean> {
    // Delete container
    console.log('\nDeleting container...');
    const blockBlobClient = containerClient.getBlockBlobClient(blobName);
    const deleteBlockBlobResponse = await blockBlobClient.deleteIfExists({
      deleteSnapshots: 'include',
    });
    if (deleteBlockBlobResponse.succeeded) {
      console.log(
        'Blob was deleted successfully. requestId: ',
        deleteBlockBlobResponse.requestId
      );
      return true;
    }
    return false;
  }

  async uploadBlob(
    containerClient: ContainerClient,
    inputFile: File,
    fileName: string
  ): Promise<string | null> {
    if (!this.blobServiceClient) return null;

    console.warn('inputFile');
    console.warn(inputFile);
    console.warn('fileName');
    console.warn(fileName);

    // Get a block blob client
    const blockBlobClient = containerClient.getBlockBlobClient(fileName);

    // Display blob name and url
    console.log(
      `\nUploading to Azure storage as blob\n\tname: ${fileName}:\n\tURL: ${blockBlobClient.url}`
    );

    const uploadOptions = {
      blobHTTPHeaders: {
        blobContentType: inputFile.type,
      },
    };

    const uploadBlobResponse = await blockBlobClient.uploadData(
      await inputFile.arrayBuffer(),
      uploadOptions
    );

    console.log(
      `Blob was uploaded successfully. requestId: ${uploadBlobResponse.requestId}`
    );

    return blockBlobClient.url;
  }
}

export default new BlobService();
