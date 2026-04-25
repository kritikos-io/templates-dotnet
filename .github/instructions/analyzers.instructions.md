---
description: "Use when editing analyzer or code-style configuration: .editorconfig, .globalconfig, stylecop.json, .DotSettings."
applyTo: ".editorconfig,.globalconfig,stylecop.json,*.DotSettings"
---
# Analyzer & Style Configuration

These files are the source of truth for code style and analyzer severity. Changes have wide blast radius.

> [!IMPORTANT]
> **Ask for explicit permission before editing.** State the rule(s), the new severity, and the rationale; wait for approval. Do not touch these files as part of unrelated work. The stated rationale belongs in the PR description.

## Guardrails

- **Do not relax a rule to silence a single occurrence.** Fix the code, or suppress narrowly with a justification comment in the offending file.
- New rules start at the **strictest justifiable severity**; downgrade later only with evidence.
- Never disable an entire analyzer category.

## File Roles

| File | Purpose |
| --- | --- |
| `.editorconfig` | Per-language formatting, naming rules, and analyzer severities scoped by file glob. |
| `.globalconfig` | Repo-wide analyzer severities not needing file-pattern scoping. |
| `stylecop.json` | StyleCop-specific settings (file headers, ordering, documentation). |
| `Solution.sln.DotSettings` | ReSharper/Rider mirror — **derived**, follows the others; do not lead changes here. |

## Coordination

- **A rule has one source of truth** across `.editorconfig`, `.globalconfig`, and `stylecop.json` — never duplicate.
- When a rule must appear in multiple files for IDE parity (e.g., mirrored to `Solution.sln.DotSettings`), severities **must match exactly**. Drift is a bug.
- StyleCop rules: configuration in `stylecop.json`, severity in `.editorconfig`.
