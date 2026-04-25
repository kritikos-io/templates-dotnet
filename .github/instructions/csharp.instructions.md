---
description: "Use when writing or modifying C# source code. Covers non-obvious defaults not caught by analyzers."
applyTo: "**/*.cs"
---
# C# Conventions

Code style is enforced by `.editorconfig`, `.globalconfig`, and StyleCop analyzers — do not restate their rules.

## Non-Obvious Defaults

- XML doc comments required on public API in `src/` (suppressed in `tests/` and `samples/`).
- Avoid `null!` (null-forgiving operator) — fix the nullability flow instead of suppressing the warning.

## Patterns

- Prefer **records** for DTOs, value objects, and other immutable data carriers.
- Use `LoggerMessage`-generated `partial` methods on a `partial class` — never log inline (e.g., `logger.LogInformation(…)`).

