// Run with Node 20+: node scripts/smoke-docker.mjs
// Creates one demo license for hung; never prints credentials or tokens.
import { readFileSync } from 'node:fs';
import { randomBytes, createHash } from 'node:crypto';
import assert from 'node:assert/strict';

const env = Object.fromEntries(readFileSync(new URL('../.env', import.meta.url), 'utf8')
  .split(/\r?\n/).filter(line => line && !line.startsWith('#'))
  .map(line => [line.slice(0, line.indexOf('=')), line.slice(line.indexOf('=') + 1)]));
const cookies = new Map();
async function request(url, options = {}) {
  const response = await fetch(url, {
    ...options, redirect: 'manual', signal: AbortSignal.timeout(30000),
    headers: { Cookie: [...cookies].map(([k, v]) => `${k}=${v}`).join('; '), ...options.headers },
  });
  for (const cookie of response.headers.getSetCookie()) {
    const pair = cookie.split(';', 1)[0];
    const split = pair.indexOf('=');
    cookies.set(pair.slice(0, split), pair.slice(split + 1));
  }
  return response;
}
async function form(url, data) {
  return request(url, { method: 'POST', body: new URLSearchParams(data) });
}
const gateway = 'http://localhost:7130';
for (const url of [
  'http://localhost:3000/', 'http://localhost:3000/callback',
  `${gateway}/api/products`, `${gateway}/api/productcategories`, `${gateway}/api/menus`,
  `${gateway}/account/login`, 'http://localhost:7294/',
  'http://localhost:7294/san-pham', 'http://localhost:7294/san-pham/1',
  'http://localhost:5237/License/Create', 'http://localhost:5080/',
  'http://localhost:5001/products',
]) {
  const response = await request(url);
  assert.equal(response.status, 200, `${url}: HTTP ${response.status}`);
  console.log(`PASS HTTP ${new URL(url).host}${new URL(url).pathname}`);
}

const discovery = await (await request(`${gateway}/.well-known/openid-configuration`)).json();
assert.equal(discovery.issuer, `${gateway}/`);
assert.equal(new URL(discovery.token_endpoint).origin, gateway);
const jwks = await (await request(discovery.jwks_uri)).json();
assert.ok(jwks.keys.length > 0);
const preflight = await request(discovery.token_endpoint, {
  method: 'OPTIONS', headers: {
    Origin: 'http://localhost:3000', 'Access-Control-Request-Method': 'POST',
    'Access-Control-Request-Headers': 'content-type',
  },
});
assert.equal(preflight.headers.get('access-control-allow-origin'), 'http://localhost:3000');
console.log('PASS OIDC discovery, signing keys and dashboard CORS');

const verifier = randomBytes(32).toString('base64url');
const challenge = createHash('sha256').update(verifier).digest('base64url');
const parameters = new URLSearchParams({
  client_id: 'react_client', redirect_uri: 'http://localhost:3000/callback',
  response_type: 'code', scope: 'openid profile email offline_access ecommerce_api',
  code_challenge: challenge, code_challenge_method: 'S256', state: 'docker-smoke',
});
const authorizeUrl = `${discovery.authorization_endpoint}?${parameters}`;
const authorize = await request(authorizeUrl);
assert.equal(authorize.status, 302, 'Authorization must redirect to login');
const login = await form(`${gateway}/account/login`, {
  username: 'admin', password: env.ADMIN_PASSWORD,
  returnUrl: new URL(authorizeUrl).pathname + new URL(authorizeUrl).search,
});
assert.equal(login.status, 302, 'Admin login failed');
const authorized = await request(authorizeUrl);
assert.equal(authorized.status, 302, 'Authorization code was not issued');
const callback = new URL(authorized.headers.get('location'));
assert.equal(callback.searchParams.get('state'), 'docker-smoke');
assert.ok(callback.searchParams.get('code'), `OIDC error: ${callback.searchParams.get('error')}`);
const tokenResponse = await form(discovery.token_endpoint, {
  grant_type: 'authorization_code', client_id: 'react_client',
  redirect_uri: 'http://localhost:3000/callback',
  code: callback.searchParams.get('code'), code_verifier: verifier,
});
assert.equal(tokenResponse.status, 200, 'PKCE token exchange failed');
const tokens = await tokenResponse.json();
assert.ok(tokens.access_token && tokens.id_token && tokens.refresh_token);
const refreshed = await form(discovery.token_endpoint, {
  grant_type: 'refresh_token', client_id: 'react_client', refresh_token: tokens.refresh_token,
});
assert.equal(refreshed.status, 200, 'Refresh token exchange failed');
console.log('PASS admin login, React Authorization Code/PKCE and refresh token');

const storeLogin = await request('http://localhost:7294/Authorization/Login');
assert.equal(storeLogin.status, 302, 'Storefront OIDC login route failed');
const storeAuthorize = await request(storeLogin.headers.get('location'));
assert.equal(storeAuthorize.status, 302, 'Storefront authorization failed');
const storeCallback = await request(storeAuthorize.headers.get('location'));
assert.equal(storeCallback.status, 302, 'Storefront token exchange or callback failed');
const profile = await request('http://localhost:7294/Home/Profile');
assert.equal(profile.status, 200, 'Storefront session failed');
console.log('PASS storefront OIDC callback and authenticated profile');

const created = await form('http://localhost:5237/License/Create', { User: 'hung' });
assert.equal(created.status, 200);
const license = await (await request('http://localhost:5237/License/GetLicenseInfor?user=hung')).json();
assert.ok(license.licenseKey && license.signature);
const verify = await request('http://localhost:5237/License/Verify', {
  method: 'POST', headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ licenseKey: license.licenseKey }),
});
assert.equal(verify.status, 200);
assert.equal((await verify.json()).message, 'License valid');
console.log('PASS create license and RSA verification (user hung)');
