import express from 'express';

import applyMiddleware from './middleware';
import applyRoutes from './routes';

// Create Express server
const app = express();

// Apply configuration: middleware + routes
applyMiddleware(app);
applyRoutes(app);

export default app;
