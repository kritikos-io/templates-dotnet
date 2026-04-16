---
description: "Use when editing Dockerfile, compose files, or Docker-related configuration. Covers guardrails for the multi-stage build and runtime image selection."
applyTo: "docker/**,compose*.yaml"
---
# Docker Conventions

`docker/Dockerfile` is a multi-stage pipeline — stage names and purposes are visible in the file itself.

## Guardrails

- **Build context** is always the repo root, not `docker/`.
- **`COPY --link`** is used intentionally for layer cache — do not replace with plain `COPY`.
- **Cache mounts** (`--mount=type=cache,target=/root/.nuget/packages`) must stay on restore, build, test, and publish steps.
- **Stage dependency chain** must be preserved (e.g., `build` → `restore`, `test` → `build`, `publish` → `test`).
- New stages should follow the existing pattern: `FROM <parent> AS <name>`.

## Build Args

- `RUNTIME_BASE`: `web` (ASP.NET), `app` (console), or `self-contained`
- `CONFIGURATION`: `Release` (default) or `Debug`
- `PUBLISHED_PROJECT`: Project name to publish (without path or extension)
- `DATABASE_PROJECT`: EF Core project for migration bundles
- `DBCONTEXT`: Optional DbContext name for migrations

## Image Selection

| Use Case | `RUNTIME_BASE` | Base Image |
|----------|----------------|------------|
| ASP.NET web apps | `web` | `aspnet:*-alpine-composite-extra` |
| Console / worker | `app` | `runtime:*-alpine-extra` |
| Self-contained | `self-contained` | `runtime-deps:*-alpine-extra` |
