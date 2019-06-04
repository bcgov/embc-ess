import { PORT, IP } from './config';
import { expressApp } from './api/app';
import { startedMessage, sysdebug } from './lib/utils';
import { createRenderer } from './lib/pdf';

const main = async () => {
  try {
    const renderer = await createRenderer();
    const app = await expressApp(renderer);

    // Start the server.
    const server = app.listen(Number(PORT), IP, function () {
      startedMessage(PORT, IP, 'PDF microservice');
    });

    // Terminate process
    const close = async () => {
      sysdebug(`Close received, gracefully closing`);
      await renderer.close();

      sysdebug(`Successful shutdown, exiting`);
      process.exit(0);
    };

    process.on('SIGTERM', close);
    process.on('SIGINT', close);

    return server;
  }
  catch (error) {
    sysdebug(`Failed to initialize renderer. ${error}`);
    process.exit(1);
  }
};

main();
