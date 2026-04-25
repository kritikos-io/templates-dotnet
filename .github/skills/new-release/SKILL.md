---
name: new-release
description: 'Cut a new release: tag the appropriate commit, generate the changelog, and produce build artifacts. Use when: releasing a new version, tagging a release, publishing NuGet packages, preparing release artifacts.'
argument-hint: 'Branch or commit to release from (default: current main) and the kind of release (stable, prerelease, hotfix)'
---

# New Release

Versioning is **GitVersion-driven** (`GitVersion.yml`). Do not hand-pick versions — let GitVersion compute them from branch and tag history.

## Versioning

| Branch | Label | Resulting version |
| --- | --- | --- |
| `main` | — | `MAJOR.MINOR.PATCH` (stable) |
| `develop` / `dev` / `development` | `beta` | `MAJOR.MINOR.PATCH-beta.N` |
| `release/*`, `hotfix/*`, `feature/*` | branch name | `…-<branch>.N` |

- Tags use the prefix `v` (regex `[vV]?` also accepts `V` or none).
- Only changes under `src/`, `Directory.Packages.props`, or `GitVersion.yml` increment the version.
- Increments are suppressed for already-merged branches and already-tagged commits.

## Procedure

1. **Sync.** `git fetch --all --tags && git status` — confirm clean.
2. **Compute.** `dotnet build -getProperty:Version` — verify the value matches expectations.
3. **Pack.** `dotnet pack -c Release` (or use the [docker-build](../docker-build/SKILL.md) `pack` target). Artifacts land in `artifacts/nuget/`.
4. **Tag.** `git tag -a v<version> -m "Release v<version>" && git push origin v<version>`.
5. **Publish** packages to the configured feed.
6. **Create the GitHub release** linked to the tag, using the generated changelog as notes.

For hotfixes: branch from the latest stable tag (`git switch -c hotfix/<name> v<latest-stable>`), follow the procedure, then back-merge into `main` and `develop`.

## Guardrails

- **Tags are immutable** once pushed; never move or delete them.
- **Never skip a `PATCH`** — ship a fix forward.
- **Do not edit `GitVersion.yml` to force a version.** Address the underlying cause (missing tag, branch base, path filter, commit messages).
