import { UserManager, WebStorageStateStore } from "oidc-client-ts";

const oidcConfig = {
  authority: "https://localhost:7130/auth",
  client_id: "react_client",
  redirect_uri: "https://localhost:3001/callback",
  post_logout_redirect_uri: "https://localhost:3001/dashboard",
  response_type: "code",
  scope: "openid profile email offline_access",
  stateStore: new WebStorageStateStore({ store: window.localStorage }),
  userStore: new WebStorageStateStore({ store: window.localStorage }),
  code_challenge_method: 'S256',
};

export const userManager = new UserManager(oidcConfig);