console.log();
console.log(`** Using angular proxy to remote API server: ${process.env.API_URL} **`);

const proxyConfig = {
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
      if (process.env.SM_TOKEN) {
        req.headers.cookie = 'sm.token=' + process.env.SM_TOKEN;
      }
    }
  }
}

module.exports = proxyConfig;
