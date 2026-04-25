---
name: ef-migration
description: 'Add, remove, or apply Entity Framework Core migrations against a project under src/. Use when: creating a new migration, reverting the last migration, generating a migration bundle, applying migrations to a database.'
argument-hint: 'Describe the schema change (e.g., "add audit columns to Orders") and the target DbContext if multiple exist'
---

# EF Core Migration

## Prerequisites

- The EF project (containing the `DbContext`) is referenced by — or is itself — a runnable startup project.
- Requires `dotnet ef`: `dotnet tool restore` (if a manifest exists) or `dotnet tool install -g dotnet-ef`.

## Conventions

- **Migration name**: `PascalCase`, imperative, third person, ≤ 60 chars (e.g., `AddsOrderAuditColumns`, not `audit_changes` or `Migration1`).
- **Output folder**: default `Migrations/` inside the EF project. Do not relocate.
- **One DbContext per command** — pass `--context` whenever the project has more than one.
- **Never edit a migration after it has been applied to a shared environment.** Add a corrective migration instead.
- **Commit migrations together with the model change** that produced them.

## Procedure

The **EF project** contains the `DbContext`; the **startup project** is the runnable project EF executes against (usually a web/worker referencing the EF project).

1. **Add the migration:**
   ```bash
   dotnet ef migrations add <PascalCaseName> \
     --project src/<EFProject> \
     --startup-project src/<StartupProject> \
     --context <DbContextName>
   ```
2. **Inspect** the generated `*.cs` and `*.Designer.cs` files for unintended changes (unrelated columns, dropped indices).
3. **If wrong, remove before committing:**
   ```bash
   dotnet ef migrations remove --project src/<EFProject> --startup-project src/<StartupProject>
   ```

For a deployable migration binary, use the [docker-build](../docker-build/SKILL.md) skill's `migrations` target.

## Generating an SQL Script

```bash
dotnet ef migrations script <FromMigration> <ToMigration> \
  --project src/<EFProject> \
  --startup-project src/<StartupProject> \
  --idempotent \
  --output artifacts/migrations.sql
```

Use `--idempotent` whenever the script may run against a database in an unknown state.

## Troubleshooting

- **"Unable to create a 'DbContext'…"** — pass `--startup-project` explicitly; auto-detection picks the wrong project in multi-project solutions.
- **Multiple DbContexts found** — pass `--context <Name>`.
