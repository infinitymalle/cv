# --- Build stage: compile the React app to static files ---
FROM node:24-alpine AS build
WORKDIR /src
COPY package.json package-lock.json ./
RUN npm ci
COPY . .
RUN npm run build

# --- Runtime stage: Caddy serves the static files and proxies /api to the backend ---
FROM caddy:2-alpine
COPY --from=build /src/dist /srv
