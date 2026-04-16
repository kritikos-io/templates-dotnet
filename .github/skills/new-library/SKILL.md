---
name: new-library
description: 'Scaffold a new reusable .NET library with correct naming, multi-targeting, and structure. Use when: creating a new library project, adding a reusable package, setting up a shared library under src/.'
argument-hint: 'Brief description of the library purpose (e.g., "hashing utilities", "EF Core auditing")'
---

# New Library

## When to Use

- Creating a new reusable, packable library under `src/`
- Adding a shared component meant for NuGet distribution

## Naming Convention

Libraries use a `{Domain}.{Feature}` pattern. The `Kritikos.` root namespace prefix is applied automatically by `Directory.Build.props`.

| Domain prefix | When to use | Example |
|---|---|---|
| `AspNetCore.{Feature}` | Depends on `Microsoft.AspNetCore.App` | `AspNetCore.ProblemDetails` |
| `Extensions.{Feature}` | Framework-agnostic, depends only on `Microsoft.Extensions.*` | `Extensions.Hashing` |
| `EntityFrameworkCore.{Feature}` | Depends on EF Core | `EntityFrameworkCore.Auditing` |
| *(no prefix)* | Pure .NET with no framework dependency | `Primitives` |

**Always propose 2–4 name options** to the user and allow a custom choice.

## Multi-Targeting

Target **both current LTS versions** plus optionally **one STS version** if newer than the latest LTS:

| Scenario | Targets | Rationale |
|---|---|---|
| .NET 10 is current LTS | `net8.0;net10.0` | Skip .NET 9 (STS, superseded) |
| .NET 11 (STS) ships | `net8.0;net10.0;net11.0` | 11 > 10, include it |
| .NET 12 (LTS) ships | `net10.0;net12.0` | .NET 8 drops; 11 superseded |

Libraries with **no framework dependency** (no domain prefix) should also target `netstandard2.0`/`netstandard2.1` when APIs and dependencies allow. Framework-dependent libraries skip .NET Standard.

## Procedure

1. Ask the user for the library's purpose.
2. Propose name options using the naming table above.
3. Determine the correct multi-targeting based on current LTS/STS and framework dependencies.
4. Create the project under `src/{LibraryName}/`.
5. Do **not** specify `Version` on `PackageReference` — add versions in `Directory.Packages.props`.
6. Create a matching test project under `tests/{LibraryName}.Tests/`.
7. Add both projects to `Solution.slnx`.
8. Verify the build compiles cleanly.
