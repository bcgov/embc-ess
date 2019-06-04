import { Request, Response, NextFunction, Express } from 'express';
import bodyParser from 'body-parser';
import { MAX_PAYLOAD_SIZE, ENABLE_CORS } from '../config';
import { sysdebug } from '../lib/utils';

export const applyMiddleware = (app: Express) => {
  if (ENABLE_CORS) {
    sysdebug('Applying CORS middleware.')
    app.use(function handleCors(req: Request, res: Response, next: NextFunction) {
      res.setHeader('Access-Control-Allow-Origin', '*');
      res.setHeader('Access-Control-Allow-Methods', 'GET, POST');
      res.setHeader('Access-Control-Allow-Headers', 'X-Requested-With,Content-Type,Authorization,responseType');
      res.setHeader('Access-Control-Allow-Credentials', 'true');
      next();
    });
  }

  // Limit to 10mb if HTML has e.g. inline images
  app.use(bodyParser.text({ limit: MAX_PAYLOAD_SIZE, type: 'text/html' }));
}
