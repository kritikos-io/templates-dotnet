---
name: docker-build
description: "Build, test, publish, or pack .NET projects using the multi-stage Docker pipeline. Use when: building Docker images, running containerized tests, creating migration bundles, publishing to containers, or troubleshooting Dockerfile stages."
argument-hint: "Describe what you want to build or publish (e.g., 'publish WebApplication1 as web app')"
---
# Docker Build Skill

## When to Use

- Build and run a .NET project in Docker
- Run tests inside a container
- Create a NuGet package via Docker
- Create an EF Core migration bundle
- Troubleshoot multi-stage Docker build issues

## Prerequisites

- Docker Desktop or Docker Engine running
- Repository root as build context

## Common Workflows

### Build and run a web application

```shell
docker compose -f docker/compose.sample.yaml build sample-api \
  --build-arg PUBLISHED_PROJECT=<ProjectName>
docker compose -f docker/compose.sample.yaml up sample-api
```

### Build self-contained application

```shell
docker build -f docker/Dockerfile \
  --target final \
  --build-arg RUNTIME_BASE=self-contained \
  --build-arg PUBLISHED_PROJECT=<ProjectName> \
  -t myapp .
```

### Run tests in container

```shell
docker build -f docker/Dockerfile --target test .
```

### Lint OpenAPI documents

```shell
docker build -f docker/Dockerfile --target openapi-lint .
```

### Pack NuGet packages

```shell
docker build -f docker/Dockerfile --target pack --output artifacts/nuget .
```

### Create EF Core migration bundle

```shell
docker build -f docker/Dockerfile --target migrations \
  --build-arg PUBLISHED_PROJECT=<StartupProject> \
  --build-arg DATABASE_PROJECT=<EFProject> \
  --build-arg DBCONTEXT=<OptionalContextName> \
  -t migrations .
```

## Stage Reference

See [Dockerfile](../../docker/Dockerfile) for the full multi-stage pipeline. Key stages and their targets:

| `--target` | Produces |
|------------|----------|
| `test` | Runs `dotnet test` — exits non-zero on failure |
| `openapi-lint` | Spectral lint on generated OpenAPI JSON |
| `publish` | Published app in `/app/publish` |
| `pack` | NuGet `.nupkg` files in `/app/publish` |
| `migration-bundle` | Standalone EF migration binary |
| `final` | Production runtime image |

## Troubleshooting

- **Restore failures**: Check `nuget.config` package sources and ensure `packages.lock.json` files are committed.
- **GitVersion errors**: The `gitversion` stage needs `.git/` — ensure the build context includes it.
- **Missing OpenAPI docs**: The `openapi-lint` stage only runs Spectral if `*.json` files exist in `artifacts/OpenApi/`.
