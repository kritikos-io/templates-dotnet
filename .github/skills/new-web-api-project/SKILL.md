---
name: new-web-api-project
description: 'Scaffold a new ASP.NET web project (API or worker service) with correct structure, launch settings, and Docker integration. Use when: creating a new web API, adding an ASP.NET project, setting up a new web service under src/.'
argument-hint: 'Brief description of the web project (e.g., "REST API for user management", "background worker for email processing")'
---

# New Web Project

## When to Use

- Creating a new ASP.NET web API or worker service under `src/`
- Adding a web project that will be deployed via Docker

## Procedure

1. Ask the user for the project's purpose and type (API or worker).
2. Create the project under `src/{ProjectName}/` using the appropriate SDK (`Microsoft.NET.Sdk.Web` or `Microsoft.NET.Sdk.Worker`).
4. **Single target framework** — web projects target only the current LTS (no multi-targeting).
5. Create `Properties/launchSettings.json` with `http` and `https` profiles:
   - Pick ports that **do not conflict** with existing projects — check all `src/*/Properties/launchSettings.json` files first.
   - Set `launchBrowser: false`.
   - Set `ASPNETCORE_ENVIRONMENT: Development`.
6. For APIs: use **Minimal APIs** with OpenAPI support (`Microsoft.AspNetCore.OpenApi`). The OpenAPI document generation and Spectral linting are handled automatically by the build infrastructure.
7. Create a matching test project under `tests/{ProjectName}.Tests/`.
8. Add both projects to `Solution.slnx`.
9. Add a compose service entry to `compose.yaml` (or create one from `docker/compose.sample.yaml`):
   - Set `RUNTIME_BASE: web` for APIs, `RUNTIME_BASE: app` for workers.
   - Set `PUBLISHED_PROJECT` to the project name.
   - Build context is the repo root with `dockerfile: docker/Dockerfile`.
10. Verify the build compiles cleanly.

## Launch Settings Template

```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:{HTTP_PORT}",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "https://localhost:{HTTPS_PORT};http://localhost:{HTTP_PORT}",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```
