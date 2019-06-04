import express from 'express';

import { applyMiddleware } from './middleware';
import { applyRoutes } from './routes';
import { Renderer } from '../lib/pdf';

// Create Express server
export const expressApp = async (renderer: Renderer): Promise<express.Express> => {
  const app = express();
  applyMiddleware(app);
  applyRoutes(app, renderer);
  return app;
};
