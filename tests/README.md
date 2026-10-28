# Test suites

| Folder | Purpose | Run |
| --- | --- | --- |
| `UnitTests` | Fast isolated xUnit tests | `dotnet test <project>.csproj` |
| `IntegrationTests` | HTTP/API tests with an in-memory database | `dotnet test <project>.csproj` |
| `E2ETests` | Playwright tests against the Docker stack or the local UI | Run the scripts in each E2E project |

## E2E prerequisites

Both E2E projects use the root `.env.e2e.test` file to start Docker Compose:

```powershell
cd tests\E2ETests\AdminDashboard.E2E
pnpm test:e2e:stack
pnpm test:e2e

cd ..\MvcApp.E2E
pnpm test:e2e
```

Run the dashboard UI-only test without Docker:

```powershell
cd tests\E2ETests\AdminDashboard.E2E
pnpm test:e2e:ui
```
