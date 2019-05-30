import app from './api/app';

const port = process.env.PORT || process.env.OPENSHIFT_NODEJS_PORT || 3000;
const ip = process.env.IP || process.env.OPENSHIFT_NODEJS_IP || '0.0.0.0';

// Start the server.
const server = app.listen(Number(port), ip, function () {
  console.log(`Server started on port: ${port}`);
  console.log(`IP set to: ${ip}`)
});

export default server;
