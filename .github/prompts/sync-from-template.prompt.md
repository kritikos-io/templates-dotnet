---
description: "Sync the current repository with upstream changes from kritikos-io/templates-dotnet, with conflict-resolution guidance."
agent: "agent"
---

# Sync From Template

Pull updates from the upstream `kritikos-io/templates-dotnet` repository, following the workflow documented in the root `README.md`.

## Steps

### 1. Verify the `template` Remote

```bash
git remote -v | grep template
```

If absent, add it and warn the user that the first sync requires `--allow-unrelated-histories`:

```bash
git remote add template https://github.com/kritikos-io/templates-dotnet
git fetch --all
```

### 2. Check Working Tree

`git status` must be clean. If it isn't, stop and ask the user to commit or stash before continuing.

### 3. Pull

- **First-time sync** (template remote was just added):
  ```bash
  git merge template/main --allow-unrelated-histories
  ```
- **Subsequent syncs**:
  ```bash
  git pull template main
  ```

### 4. Resolve Conflicts

Common conflict zones, with resolution guidance:

| File / area | Strategy |
| --- | --- |
| `Solution.slnx`, `Solution.code-workspace` | Keep local — these are project-specific. Re-apply any new entries from upstream manually. |
| `Directory.Build.props`, `Directory.Build.targets`, `Directory.Packages.props` | Take upstream as the base, re-apply project-specific overrides on top. |
| `.github/instructions/`, `.github/skills/`, `.github/prompts/` | Take upstream; project-specific guidance should live in a *separate* file, never as edits to template-shipped files. |
| `README.md`, `LICENSE.md`, `icon.png` | Keep local — these are project-specific. |
| `docker/Dockerfile` | Take upstream; project-specific compose lives in `docker/compose.*.yaml`. |

When a conflict needs human judgement, list it with file path + decision options rather than guessing.

### 5. Verify

```bash
dotnet restore --force-evaluate
dotnet build
dotnet test
```

If lock files moved, commit `packages.lock.json` updates.

### 6. Commit

Use a single merge commit; do not squash. Subject:

```
🔀 Syncs with kritikos-io/templates-dotnet
```

(Per [copilot-commit-message-instructions.md](../copilot-commit-message-instructions.md): gitmoji, third-person imperative.)
