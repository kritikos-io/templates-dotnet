# Templates - Dotnet

A starting point for new .NET projects, based on opinionated rules.

## Getting started

1. In order to be able to update your repository with the latest changes, you can use the following command **after creating** your repo:
   ```bash
   git remote add template https://github.com/kritikos-io/templates-dotnet
   git fetch --all
   git merge template/main --allow-unrelated-histories
   ```
1. Do this as soon as possible, as the unrelated histories flag will lead to a few conflicts that you will need to resolve manually.
1. Afterwards, you can pull future changes using
   ```bash
   git pull template main
   ```
1. Rename the solution and project files, replacing 'Solution' to match your project name.
   1. Solution.slnx
   1. Solution.sln.DotSettings
   1. Solution.code-workspace

## Features

Apart from a robust configuration, this template specifically includes:

- Git history to semantic versioning integration using GitVersion
- Artifacts layout, to avoid bin/obj folders all over the place. These are placed in the `artifacts` folder at the solution level, and are further organized by project and configuration. Can also be overridden either at the project level or by a Directory.Build.props above the repository root to redirect them to a different location.
- Central package management, to avoid version conflicts and make it easier to update dependencies.
- Additional targets that enable change log generated from git history, and SBOM generation.

## Docker

A multi-stage Dockerfile is provided in `docker/` with multiple targets for different use cases. Sample usage is provided in [`compose.sample.yaml`](./docker/compose.sample.yaml).

The `RUNTIME_BASE` build arg controls the final image base:

| Value | Base image | Use case |
|---|---|---|
| `web` (default) | `aspnet` | ASP.NET web applications |
| `app` | `runtime` | Console applications |
| `self-contained` | `runtime-deps` | Self-contained deployments |

### OpenAPI Linting

OpenAPI documents generated at build time are validated using [Spectral]. Configure rules in `.spectral.yaml` at the repository root.

## Recommendations

> Keep in mind that until the dotnet toolset handles generating new projects correctly, you will need to edit new *proj files and remove Version attributes from PackageReference entries. For more details consult [Central Package Management].

> Provided props files allow compiled models with EF Core 9+, to use them install `Microsoft.EntityFrameworkCore.Tasks` on all projects containing DbContext classes. (Not yet suited for production use, consult [Entity Framework Core MSBuild integration]).

[Central Package Management]: https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management
[Entity Framework Core MSBuild integration]: https://learn.microsoft.com/en-us/ef/core/cli/msbuild
[Spectral]: https://github.com/stoplightio/spectral
