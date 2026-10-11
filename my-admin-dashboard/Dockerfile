FROM node:22-alpine AS build
WORKDIR /app
RUN corepack enable && corepack prepare pnpm@10.8.1 --activate
COPY package.json pnpm-lock.yaml ./
RUN pnpm install --frozen-lockfile
COPY . .
ARG VITE_API_BASE_URL=http://localhost:7130/api
ARG VITE_AUTHORITY_URL=http://localhost:7130
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL VITE_AUTHORITY_URL=$VITE_AUTHORITY_URL
ENV VITE_REDIRECT_URI=http://localhost:3000/callback VITE_POST_LOGOUT_REDIRECT_URI=http://localhost:3000/
ENV VITE_CLIENT_ID=react_client VITE_SCOPE="openid profile email offline_access ecommerce_api" VITE_RESPONSE_TYPE=code VITE_CODE_CHALLENGE_METHOD=S256
RUN pnpm build
FROM nginx:1.28-alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
HEALTHCHECK --interval=15s --timeout=3s CMD wget -q -O /dev/null http://127.0.0.1/ || exit 1
