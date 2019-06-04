import { ENABLE_DIAGNOSTICS } from '../config';

const divider = '\n-----------------------------------';

/**
 * Print canned message when the server starts
 *
 * @param {String} port The port the server is running on
 * @param {String} ip The IP the server is running on
 * @param {String} appName Human-friendly name of the app (e.g 'PDF microservice')
 */
export const startedMessage = (port: string | number, ip: string, appName: string) => {
  console.log(`\n${appName} API started`);
  console.log(`Access URLs: ${divider}`);
  console.log(`Localhost: http://localhost:${port}`);
  console.log(`      LAN: http://${ip}:${port} ${divider}`);

  if (process.env.NODE_ENV === 'development') {
    console.log(`Press 'CTRL-C' to stop\n`);
  }
}

export const sysdebug = (message: string) => {
  if (ENABLE_DIAGNOSTICS) { console.log(message); }
};

export class Stopwatch {
  private startAt: number;
  private end = 0;

  // in milliseconds
  get time(): number {
    return this.end;
  }
  get words(): string {
    return `Execution time: ${this.time}ms`;
  }
  start(): Stopwatch {
    if (ENABLE_DIAGNOSTICS) { this.startAt = Date.now(); }
    return this;
  }
  stop(): void {
    if (ENABLE_DIAGNOSTICS) { this.end = Date.now() - this.startAt; }
  }
}
