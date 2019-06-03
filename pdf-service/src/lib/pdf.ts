import puppeteer from 'puppeteer';
import { sysdebug, Stopwatch } from './utils';

// TODO: remove after refactor + testing
export const printPDF = async (html: string) => {
  sysdebug('[legacy] Launching puppeteer');
  const args = ['--no-sandbox', '--disable-setuid-sandbox', '--disable-dev-shm-usage'];
  const browser = await puppeteer.launch({ headless: true, args });
  const page = await browser.newPage();
  await page.setContent(html, { waitUntil: 'networkidle0' });
  const buffer = await page.pdf({ format: 'Letter', printBackground: true });
  sysdebug('[legacy] Puppeteer generated pdf');
  await browser.close();
  sysdebug('[legacy] Close puppeteer');
  return buffer;
};

export class Renderer {
  private browser: puppeteer.Browser;

  constructor(browser: puppeteer.Browser) {
    this.browser = browser;
  }

  private async createPage(html: string, options: puppeteer.NavigationOptions = {}) {
    if (!this.browser) { throw new Error('Chrome instance is required'); }

    const { timeout, waitUntil } = options;
    const page = await this.browser.newPage();
    await page.setContent(html, {
      timeout: Number(timeout) || 30 * 1000,
      waitUntil: waitUntil || 'networkidle0',
    });
    return page;
  }

  async pdf(html: string, options: puppeteer.NavigationOptions = {}) {
    let page: puppeteer.Page = null;
    try {
      page = await this.createPage(html, options);
      const buffer = await page.pdf({ format: 'Letter', printBackground: true });
      return buffer;
    } finally {
      if (page) {
        await page.close();
      }
    }
  }

  async close() {
    sysdebug(`Shutting down Chrome`);
    if (this.browser) {
      await this.browser.close();
      this.browser = null;
    }
  }
}

const launchChrome = async (): Promise<puppeteer.Browser> => {
  const timer = new Stopwatch();
  timer.start();

  const args = ['--no-sandbox', '--disable-setuid-sandbox', '--disable-dev-shm-usage'];
  sysdebug(`\nLaunching Chrome with args: ${JSON.stringify(args)}`);
  const browser = await puppeteer.launch({ headless: true, args });
  const chromeVersion = await browser.version();
  timer.stop();
  sysdebug(`Chrome ${chromeVersion} launched ${timer.time}ms\n`);

  return browser;
};

export const createRenderer = async () => {
  const chrome = await launchChrome();
  return new Renderer(chrome);
};
