import express from 'express';
import blobService from '../services/blob.service';
import { filterFileName, validateImageFile } from '../helpers/blobHelper';
const azureBlobRoutes = express.Router();

/**
 * Gets all blobs from the heroes container
 */
azureBlobRoutes.get('/', async (req, res) => {
  // try {
  //   const containerClient = blobService.containerHeroesImagesClient;
  //   if (!containerClient)
  //     return res.status(500).send("Container client hasn't been initialized.");

  //   const blobList = await blobService.listBlobClients(containerClient);
  //   return res.status(200).send(blobList);
  // } catch (error: any) {
  //   return res.status(500).send(error.message);
  // }

  await blobService.test();
});

/**
 * Gets a file from the heroes container given the blob name
 */
azureBlobRoutes.get('/:filename', async (req, res) => {
  try {
    const containerClient = blobService.containerHeroesImagesClient;
    if (!containerClient)
      return res.status(500).send("Container client hasn't been initialized.");

    const fileName = req.params.filename;
    if (!req.params.filename || Object.keys(req.params).length === 0)
      return res.status(400).send('No file name param provided.');

    const url = await blobService.getBlobUrl(fileName, containerClient);

    if (!url) return res.status(400).send(`File ${fileName} not found.`);

    return res.status(200).send(url);
  } catch (error: any) {
    return res.status(500).send(error.message);
  }
});

/**
 * Uploads or updates an image from the heroes container
 */
azureBlobRoutes.post('/', async (req: any, res) => {
  try {
    const containerClient = blobService.containerHeroesImagesClient;
    if (!containerClient)
      return res.status(500).send("Container client hasn't been initialized.");

    if (!req.files || Object.keys(req.files).length === 0)
      return res.status(400).send('No files were uploaded.');

    const inputFile: File = req.files[0];

    if (!validateImageFile(inputFile))
      return res
        .status(500)
        .send(
          `Invalid image type ${inputFile.type}, allowed types: .png, .jpg, .jpeg and .webp`
        );

    const fileName = filterFileName(req.body.fileName);
    const fileUrl = await blobService.uploadBlob(
      containerClient,
      inputFile,
      fileName
    );

    if (!fileUrl)
      return res.status(400).send(`Couldn't upload file ${inputFile.name}.`);

    return res.status(200).send(fileName);
  } catch (error: any) {
    return res.status(500).send(error.message);
  }
});

/**
 * Deletes a file from the heroes container given the blob name
 */
azureBlobRoutes.delete('/:filename', async (req, res) => {
  try {
    const containerClient = blobService.containerHeroesImagesClient;
    if (!containerClient)
      return res.status(500).send("Container client hasn't been initialized.");

    const fileName = req.params.filename;
    if (!req.params.filename || Object.keys(req.params).length === 0)
      return res.status(400).send('No file name param provided.');

    const isDeleted = await blobService.deleteBlob(fileName, containerClient);

    if (!isDeleted) return res.status(400).send(`File ${fileName} not found.`);

    return res.status(200).send(true);
  } catch (error: any) {
    return res.status(500).send(error.message);
  }
});

export default azureBlobRoutes;
