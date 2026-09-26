# CV

A personal CV site for things built during my Computer Science and Engineering studies.
It's built so it can grow into something else without starting over.

- **Backend:** C# / ASP.NET Core (.NET 10), layered ("Clean Architecture")
- **Frontend:** React + TypeScript (Vite)
- **Hosting:** Docker + Caddy (automatic HTTPS), runs anywhere Docker runs

## How it fits together

```
Browser ──HTTPS──► Caddy (web container)
                     ├── /        → React app (static files)
                     └── /api/*   → ASP.NET Core API (internal network only)
                                        │
                                        ▼
                                   content/*.json
```

### Backend layers

```
backend/src/
├─ Cv.Domain/          Core types (Project). Depends on nothing.
├─ Cv.Application/     Logic + interfaces (IProjectService, IProjectRepository). Depends on Domain.
├─ Cv.Infrastructure/  How data is stored (JsonProjectRepository). Implements Application's interfaces.
└─ Cv.Api/             HTTP: endpoints, security middleware, wiring (Program.cs).
```

Dependencies point **inward**: `Api → Application → Domain`, and `Infrastructure → Application`.
The logic never knows *where* data comes from, so storage can change without touching it.
For example, moving to a database means adding a new `IProjectRepository` implementation and
changing one line in `Cv.Infrastructure/DependencyInjection.cs`.

### Frontend layout

```
frontend/src/
├─ app/                 App shell / layout
├─ api/                 Typed API client; schema.d.ts is GENERATED from the backend
└─ features/projects/   Everything for one feature: data fetching, hook, components, tests
```

New sections of the site (courses, experience, ...) get their own folder under `features/`.

### The API contract

Building the backend writes `backend/openapi/Cv.Api.json`, a description of every endpoint.
`npm run gen:api` turns that into TypeScript types, so if a field changes in C#, the frontend
fails to compile until it's updated. Both files are committed, so API changes show up in diffs.

## Running locally (development)

Requirements: .NET 10 SDK, Node 24.

```sh
# Terminal 1: API on http://localhost:5080
cd backend
dotnet run --project src/Cv.Api

# Terminal 2: site on http://localhost:5173 (proxies /api to the backend)
cd frontend
npm install
npm run dev
```

In development the API document is at http://localhost:5080/openapi/v1.json.

## Tests

```sh
cd backend  && dotnet test     # unit tests (Application) + integration tests (real API in memory)
cd frontend && npm test        # component tests (Vitest + Testing Library)
```

### CI (GitHub Actions)

Every push and pull request runs `.github/workflows/ci.yml`:

1. **Backend:** build + all tests, and a check that the committed API contract is up to date
2. **Frontend:** a check that the generated types match the contract, then typecheck, tests and build
3. **Docker:** both images build (only if 1 and 2 pass)

Dependabot (`.github/dependabot.yml`) opens weekly PRs for dependency updates, and CI runs on each one.

## Editing content

Edit `content/projects.json`. No code changes or rebuild needed, since the API reads the file on each request.

## Adding a new feature (e.g. "courses")

1. **Domain:** add `Cv.Domain/Courses/Course.cs`
2. **Application:** add `ICourseRepository`, `CourseDto`, `ICourseService` + `CourseService`; register in `DependencyInjection.cs`
3. **Infrastructure:** add `JsonCourseRepository`; register it
4. **Api:** add `Endpoints/CourseEndpoints.cs`; map it in `Program.cs`
5. **Tests:** unit test the service with a fake repository; add integration tests
6. `dotnet build`, then `npm run gen:api` in `frontend/`
7. **Frontend:** add `features/courses/` and use it in `App.tsx`

## Deploying (Docker)

```sh
cp .env.example .env          # set SITE_ADDRESS to your domain, or keep localhost
docker compose up --build -d
```

Only Caddy (ports 80/443) is exposed. The API is only reachable through Caddy on an internal Docker network.
Moving to another host means copying the repo and `.env` and running the same command.

### Hosting from home

Don't port-forward 80/443 on your router. Use a **Cloudflare Tunnel** (`cloudflared`) instead:
it makes an *outgoing* connection to Cloudflare, so there are no open ports and your home IP stays hidden.
Point the tunnel at `http://localhost:80`.

## Security measures

| Where | What |
|---|---|
| Caddy | HTTPS (auto certificates), HSTS, strict Content-Security-Policy, anti-framing, no `Server` header |
| API | Read-only (GET only), per-IP rate limiting, security headers, small request-size limit, input validation (slugs), only `http(s)` links reach the browser, errors as ProblemDetails without internals |
| Containers | API runs as non-root in a "chiseled" image (no shell), read-only filesystem, all Linux capabilities dropped, content mounted read-only |
| Repo | `.env` is gitignored; no secrets in code |

Keep dependencies up to date: `dotnet list package --outdated` and `npm outdated` / `npm audit`.

## Notes on tooling choices

- **TypeScript 5.9, not 7:** `openapi-typescript` needs the TypeScript 5 compiler API. Upgrade once it supports 7.
- **xUnit v3 on Microsoft Testing Platform:** enabled in `backend/global.json`; required by .NET 10's `dotnet test`.
- **Central package versions:** all NuGet versions live in `backend/Directory.Packages.props`.
