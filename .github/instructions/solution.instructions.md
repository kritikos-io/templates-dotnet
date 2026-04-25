---
description: "Use when adding, removing, or moving projects, or when editing the solution file or VS Code workspace."
applyTo: "*.slnx,*.sln,Solution.code-workspace"
---
# Solution Conventions

`Solution.slnx` (XML) is the canonical solution format. The legacy `.sln` is not used; do not regenerate it.

## Adding & Removing Entries

Any project, configuration, or asset file added to, removed from, or renamed in a tracked location **must** be reflected in `Solution.slnx` in the same commit. New top-level folders also require an entry in `Solution.code-workspace`.

Place entries in the virtual folder matching the on-disk location:

| On-disk location | Virtual folder |
| --- | --- |
| `src/**`, `samples/**`, `tests/**` (projects + their `Directory.Build.props`) | `/src/`, `/samples/`, `/tests/` |
| Repo-root config files (`.editorconfig`, `Directory.*.props`, `GitVersion.yml`, etc.) | `/Configuration/` |
| Repo-root assets (`README.md`, `LICENSE.md`, `icon.png`, logos) | `/Assets/` |

Reserved virtual folder names must not be renamed.

## Renames

- Update the `.slnx` entry, the on-disk path, and any `ProjectReference` paths in dependent projects in the same commit.
- Never leave dangling `<File Path="…" />` or project entries.

After any `.slnx` change, run `dotnet build` to confirm all referenced projects load.
