---
name: new-release
description: 'Cut a new release: tag the appropriate commit, generate the changelog, and produce build artifacts. Use when: releasing a new version, tagging a release, publishing NuGet packages, preparing release artifacts.'
argument-hint: 'Branch or commit to release from (default: current main) and the kind of release (stable, prerelease, hotfix)'
---

# New Release

Versioning is **GitVersion-driven** (`GitVersion.yml`). Do not hand-pick versions — let GitVersion compute them from branch and tag history.

## Versioning

The root `GitVersion.yml` uses the `TrunkBased/preview1` workflow. Its `Mainline` strategy is what makes stable branches increment per commit rather than holding one version until the next tag.

| Branch | Label | Resulting version |
| --- | --- | --- |
| `main`, `release/*` | — | `MAJOR.MINOR.PATCH` (stable; **patch increments on every commit**) |
| `feature/*` | branch name | `…-<branch>.N` (pre-release; merging bumps **minor**) |
| `hotfix/*` | branch name | `…-<branch>.N` (pre-release; merging bumps **patch**) |

- Tags use the prefix `v` (regex `[vV]?` also accepts `V` or none).
- Each `src/*` project is versioned **independently**: only changes under that project's own directory, `Directory.Packages.props`, or `GitVersion.yml` increment its version (see `GenerateProjectGitVersionConfig` in `src/Directory.Build.props`, which renders the root `GitVersion.yml` into a per-project copy). A change to one library does not bump a sibling's version.
- All five packages still share one tag namespace, so a tag rebases every project's `MAJOR.MINOR` at once. Only the patch/pre-release counters diverge per project.
- Increments are suppressed on already-tagged commits, so the tagged commit builds exactly the tagged version.

## Changelog

Release notes are generated at pack time and embedded as `PackageReleaseNotes`. The `GenerateChangelogFromGit` target writes `git log` subjects to `$(ChangeLogPath)`, and `CreateReleaseNotesFromFile` reads that file into the nuspec. Both run before `GenerateNuspec`, and generation is skipped when the repository root has no `.git` directory.

The commit range is resolved in this order, first match wins:

1. `$(ChangeLogFromRef)..HEAD` — when `ChangeLogFromRef` is set.
2. `<tag>..HEAD` — from `git describe --tags --abbrev=0`.
3. `<sha>..HEAD` — the last first-parent merge whose subject matches `$(ChangeLogUpstreamMergePattern)`.
4. `<sha>..HEAD` — the last first-parent merge.
5. No range; the log is capped at `$(ChangeLogCommitLimit)` commits.

Commits are filtered with `--no-merges` and scoped to the project's own directory, plus the directories of its `ProjectReference`s unless `ChangeLogIncludeReferences` is `false`. This mirrors the per-project versioning rule above: a sibling library's commits do not appear in this package's notes.

| Property | Default | Purpose |
| --- | --- | --- |
| `ChangeLogFromRef` | *(empty)* | Explicit range start; overrides the fallback chain |
| `ChangeLogPath` | `$(BaseIntermediateOutputPath)changes.log` | Where the log is written and read from |
| `ChangeLogCommitLimit` | `100` | Cap used only when no range resolves |
| `ChangeLogUpstreamMergePattern` | `from .*/` | Subject matcher for the upstream-merge fallback |
| `ChangeLogIncludeReferences` | `true` | Include `ProjectReference` directories in scoping |
| `GenerateChangelogFromGit` | *(unset)* | Set to `false` to skip generation entirely |

> [!IMPORTANT]
> Step 2 uses the **nearest reachable** tag, not the newest. Packing from a commit that already carries a release tag therefore yields only the commits added since it — often a single entry. Pass `ChangeLogFromRef` to widen the range:
>
> ```shell
> dotnet pack -c Release -p:ChangeLogFromRef=v4.0.0
> ```

To ship hand-written notes instead, disable generation and point `ChangeLogPath` at your own file. Generation must be disabled, otherwise the target overwrites it:

```shell
dotnet pack -c Release -p:GenerateChangelogFromGit=false -p:ChangeLogPath=notes.md
```

Confirm the resolved range in the build output before publishing:

```text
Generated changelog at <path> using range v4.0.0..HEAD.
```

## Procedure

1. **Sync.** `git fetch --all --tags && git status` — confirm clean.
2. **Compute.** `dotnet build src\<Project>\<Project>.csproj -getProperty:Version` — verify the value matches expectations for that specific project (there is no single repo-wide version).
3. **Pack.** `dotnet pack -c Release` (or use the [docker-build](../docker-build/SKILL.md) `pack` target). Artifacts land in `artifacts/nuget/`. Check the logged changelog range and pass `ChangeLogFromRef` if it is narrower than the release warrants.
4. **Tag.** `git tag -a v<version> -m "Release v<version>" && git push origin v<version>`.
5. **Publish** packages to the configured feed.
6. **Create the GitHub release** linked to the tag, using the generated changelog as notes.

For hotfixes: branch from the latest stable tag (`git switch -c hotfix/<name> v<latest-stable>`), follow the procedure, then merge back into `main`. There is no `develop` branch to reconcile with — this repo is trunk-based.

## Guardrails

- **Tags are immutable** once pushed; never move or delete them.
- **Never skip a `PATCH`** — ship a fix forward.
- **Do not edit `GitVersion.yml` or a generated `src/*/GitVersion.yml` to force a version.** Address the underlying cause (missing tag, branch base, path filter, commit messages). Generated per-project files are gitignored build artifacts, re-derived from the root `GitVersion.yml` on every build — never hand-edit them.
