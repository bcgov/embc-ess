import puppeteer from 'puppeteer';

export default async function printPDF(html: string) {
  console.log('Launching puppeteer')
  const browser = await puppeteer.launch({
    headless: true,
    args: ['--no-sandbox', '--disable-setuid-sandbox', '--disable-dev-shm-usage']
  });
  const page = await browser.newPage();
  await page.setContent(html, { waitUntil: 'networkidle0' });
  const pdf = await page.pdf({ format: 'Letter' });
  console.log('Puppeteer generated pdf');
  await browser.close();
  console.log('Close puppeteer')
  return pdf;
};
