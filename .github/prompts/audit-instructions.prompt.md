---
description: "Audit .github/instructions/ and .github/skills/ for duplication, stale references, and unscoped or misscoped files."
agent: "agent"
---

# Audit Instructions & Skills

Walk every file under `.github/instructions/` and `.github/skills/` and produce a prioritised report of issues. Make no edits unless the user approves them after reading the report.

## Steps

### 1. Inventory

List every `*.instructions.md` and every `**/SKILL.md`, plus `copilot-instructions.md` and any `*.prompt.md`.

### 2. Per-File Checks

For each instructions file:

- `applyTo` glob exists and matches actual files in the repo (no dead globs).
- `description` accurately reflects the content (no stale TOCs, no over-promises).
- Content is project-specific — flag anything restating universal Markdown / C# / formatter rules, or duplicating the agent runtime (e.g., "work in parallel", "save to memory").
- File defers to enforcement layers (`.editorconfig`, analyzers, `Directory.Build.props`) instead of restating their rules.
- Cross-references point at files that exist.

For each skill:

- Frontmatter `name` matches the directory name.
- `description` starts with the action and includes "Use when:".
- Procedure steps are actionable, not narration.
- Commands actually work against this repo's tool versions and config files.
- Cross-skill references use relative paths.

### 3. Cross-File Checks

- Same rule defined in multiple files with different wording or severity → flag.
- Topics not covered by any file but enforced via PR comments / convention → flag as candidate new file.
- `applyTo` overlap that could cause conflicting guidance on a single file → flag.

### 4. Report

Produce a table sorted by priority (high / medium / low):

| Priority | File | Issue | Suggested fix |

End with two summary counts: total files inspected, total issues found.

## Guardrails

- Read-only by default. Apply fixes only if the user explicitly approves them.
- When suggesting a fix, quote the exact lines to change so the user can decide without re-reading the file.
