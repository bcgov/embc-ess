import { Request, Response, Express } from 'express';
import { Renderer, printPDF } from '../lib/pdf';
import { Stopwatch, sysdebug } from '../lib/utils';

export const applyRoutes = (app: Express, renderer?: Renderer) => {

  /**
   * POST /pdf
   * PDF rendering.
   */
  app.post('/pdf', async function (req: Request, res: Response) {
    const timer = new Stopwatch();
    const html = req.body;
    let pdf: Buffer;
    let { filename = 'unnamed.pdf', v = 2 } = req.query;
    sysdebug(`[/pdf] Request to print ${filename}`);

    // measure execution time
    timer.start();

    if (!renderer) {
      sysdebug(`Renderer not found, using legacy PDF generation`);
      v = 1;
    }

    if (Number(v) == 1) {
      sysdebug(`Using legacy PDF mode`);
      pdf = await printPDF(html);  // legacy
    } else {
      pdf = await renderer.pdf(html);
    }

    sysdebug(`Sending back PDF of length ${pdf.length}`);

    res.setHeader('Content-Type', 'application/pdf');
    res.setHeader('Content-Length', pdf.length);
    res.setHeader('Content-Disposition', `inline; filename=${filename}`);
    res.send(pdf);

    timer.stop();

    sysdebug(timer.words);
    sysdebug(`[/pdf] Done`);
  });

  /**
   * GET /health
   * Server health check.
   */
  app.get('/health', async function (req: Request, res: Response) {
    res.send(`PDF microservice lives! - ${new Date().toISOString()}`);
  });
};
