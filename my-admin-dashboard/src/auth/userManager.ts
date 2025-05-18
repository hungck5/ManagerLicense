import { UserManager, WebStorageStateStore } from "oidc-client-ts";

const oidcConfig = {
  authority: import.meta.env.VITE_AUTHORITY_URL,
  client_id: import.meta.env.VITE_CLIENT_ID,
  redirect_uri: import.meta.env.VITE_REDIRECT_URI,
  post_logout_redirect_uri: import.meta.env.VITE_POST_LOGOUT_REDIRECT_URI,
  response_type: import.meta.env.VITE_RESPONSE_TYPE,
  scope: import.meta.env.VITE_SCOPE,
  stateStore: new WebStorageStateStore({ store: window.localStorage }),
  userStore: new WebStorageStateStore({ store: window.localStorage }),
  code_challenge_method: import.meta.env.VITE_CODE_CHALLENGE_METHOD,
};

export const userManager = new UserManager(oidcConfig);