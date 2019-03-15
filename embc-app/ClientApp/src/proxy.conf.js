
console.log(`Using proxy to remote API server: ${process.env.API_URL}`);

const PROXY_CONFIG = {
  '/api': {
    'target': process.env.API_URL,
    'secure': false,
    'changeOrigin': true,
    'bypass': function (req, res, proxyOptions) {
      if (!process.env.API_URL) {
        return false;
      }
    }
  }
}

module.exports = PROXY_CONFIG;
