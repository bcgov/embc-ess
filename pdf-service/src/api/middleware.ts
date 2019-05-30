import express from 'express';
import bodyParser from 'body-parser';
import path from 'path';

export default function applyMiddleware(app: express.Express) {
  console.log('Applying CORS middleware.')
  // Enable CORS
  app.use(function handleCors(req: express.Request, res: express.Response, next: express.NextFunction) {
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST');
    res.setHeader('Access-Control-Allow-Headers', 'X-Requested-With,Content-Type,Authorization,responseType');
    res.setHeader('Access-Control-Allow-Credentials', 'true');
    next();
  });

  // Limit to 10mb if HTML has e.g. inline images
  app.use(bodyParser.text({ limit: '10mb', type: 'text/html' }));
}