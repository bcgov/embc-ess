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
    console.log('Request to print ' + filename);
    const pdf = await printPDF(html);
    console.log(`Sending back PDF of length ${pdf.length}`);
    res.setHeader('Content-Type', 'application/pdf');
    res.setHeader('Content-Length', pdf.length);
    res.setHeader('Content-Disposition', `inline;filename=${filename}`);
    res.send(pdf);
    console.log(`Done`)
  });

  /**
   * GET /health
   * Server health check.
   */
  app.get('/health', async function (req: Request, res: Response) {
    res.send(`PDF microservice lives! - ${new Date().toISOString()}`);
  });
}
