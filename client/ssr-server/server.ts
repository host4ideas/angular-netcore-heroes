import { APP_BASE_HREF } from '@angular/common';
import { CommonEngine } from '@angular/ssr';
import express from 'express';
import { fileURLToPath } from 'node:url';
import { dirname, join, resolve } from 'node:path';
import bootstrap from '../src/main.server';
import azureHeroesBlobsRouter from './routes/blobheroesimages';
import { readFileSync } from 'node:fs';
import { createServer } from 'node:https';

// The Express app is exported so that it can be used by serverless Functions.
export function app(): express.Express {
  const server = express();
  const serverDistFolder = dirname(fileURLToPath(import.meta.url));
  const browserDistFolder = resolve(serverDistFolder, '../browser');
  const indexHtml = join(serverDistFolder, 'index.server.html');

  const commonEngine = new CommonEngine();

  server.set('view engine', 'html');
  server.set('views', browserDistFolder);

  // Custom endpoints
  server.use('/api/blobheroes', azureHeroesBlobsRouter);

  // Serve static files from /browser
  server.get(
    '*.*',
    express.static(browserDistFolder, {
      maxAge: '1y',
    })
  );

  // All regular routes use the Angular engine
  server.get('*', (req, res, next) => {
    const { protocol, originalUrl, baseUrl, headers } = req;

    commonEngine
      .render({
        bootstrap,
        documentFilePath: indexHtml,
        url: `${protocol}://${headers.host}${originalUrl}`,
        publicPath: browserDistFolder,
        providers: [{ provide: APP_BASE_HREF, useValue: baseUrl }],
      })
      .then((html) => res.send(html))
      .catch((err) => next(err));
  });

  return server;
}

function run(): void {
  let port: string | number;

  try {
    const options = {
      key: readFileSync('./dist/angular-tour-of-heroes/server/selfsigned.key'),
      cert: readFileSync('./dist/angular-tour-of-heroes/server/selfsigned.crt'),
    };

    const httpsServer = createServer(options, app());

    port = 4443;

    httpsServer.listen(port, () => {
      console.log(`Node Express server listening on https://localhost:${port}`);
    });
  } catch (error: any) {
    console.error(`Couldn't start https server, reason: \n\r ${error.message}`);

    port = process.env['PORT'] || 4000;
    const server = app();
    server.listen(port, () => {
      console.log(`Node Express server listening on http://localhost:${port}`);
    });
  }
}

run();
