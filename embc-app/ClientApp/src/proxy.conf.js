const apiUrl = process.env.API_URL || 'http://localhost:49200';
console.log(`** Using angular proxy to remote API server: ${apiUrl} **`);

const proxyConfig = {
  '/api': {
    'target': apiUrl,
    'secure': true,
    'changeOrigin': true,
    'logLevel': 'debug',
  },
  '/login': {
    'target': apiUrl,
    'secure': true,
    'changeOrigin': true,
    'logLevel': 'debug',
  },
  '/signin-oidc': {
    'target': apiUrl,
    'secure': true,
    'changeOrigin': true,
    'logLevel': 'debug',
  }
}

module.exports = proxyConfig;