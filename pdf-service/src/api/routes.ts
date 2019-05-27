import { Request, Response, Express } from 'express';
import printPDF from './pdf';

export default function applyRoutes(app: Express) {

  /**
   * POST /pdf
   * PDF rendering.
   */
  app.post('/pdf', async function (req: Request, res: Response) {
    const { filename = 'unnamed.pdf' } = req.query;
    const html = req.body;
    const pdf = await printPDF(html);
    res.setHeader('Content-Type', 'application/pdf');
    res.setHeader('Content-Length', pdf.length);
    res.setHeader('Content-Disposition', `inline;filename=${filename}`);
    res.send(pdf);
  });

  /**
   * GET /health
   * Server health check.
   */
  app.get('/health', async function (req: Request, res: Response) {
    res.send(`PDF microservice lives! - ${new Date().toISOString()}`);
  });
}
