
console.log(`Using proxy to remote API server: ${process.env.API_URL}`);

const PROXY_CONFIG = {
  '/embcess/api': {
    'target': process.env.API_URL,
    'secure': false,
    'changeOrigin': true,
    'pathRewrite': {
      '^/embcess': ''
    },
    'bypass': function (req, res, proxyOptions) {
      if (!process.env.API_URL) {
        return false;
      }
      // append dev user login tokens (if available)
      if (process.env.DEV_USER) {
        req.headers['DEV-USER'] = process.env.DEV_USER;
      }
      if (process.env.DEV_BCSC_USER) {
        req.headers['DEV-BCSC-USER'] = process.env.DEV_BCSC_USER;
      }
    }
  }
}

module.exports = PROXY_CONFIG;
