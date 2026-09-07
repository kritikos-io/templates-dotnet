---
description: "Use when writing or modifying C# source code. Covers non-obvious defaults not caught by analyzers."
applyTo: "**/*.cs"
---
# C# Conventions

Code style is enforced by `.editorconfig`, `.globalconfig`, and StyleCop analyzers — do not restate their rules.

## Non-Obvious Defaults

- XML doc comments required on all `public` and `protected` members in `src/` (suppressed in `tests/` and `samples/`).
- Avoid `null!` (null-forgiving operator) — fix the nullability flow instead of suppressing the warning.

## Comments

Applies to `src/`. Tests keep the guidance in the testing instructions.

- **No comments inside member bodies.** Anything worth saying goes in an XML doc comment on the member that says it.
- Document `private` and `internal` members — the analyzers only *require* docs on exposed elements, they do not object to more. Put the "why" in `<remarks>`.
- If the explanation is about one statement rather than the whole member, extract that statement into a named private method and document that instead.
- For logic inside lambdas or local functions where an XML doc comment is not applicable, extract the logic into a named private method and document that method instead.
- Never state what the code already shows, and never narrate history — a past bug, a change, or an answer to a reviewer belongs in the commit message, where it cannot go stale.
- Sole exception: the justification required beside a narrow analyzer suppression, which has nowhere else to live.

## Patterns

- Prefer **records** for DTOs, value objects, and other immutable data carriers.
- Use `LoggerMessage`-generated `partial` methods on a `partial class` — never log inline (e.g., `logger.LogInformation(…)`). Do not make existing types partial without explicit user sign-off. Do not fall back to inline logging.
- If a type cannot be made partial (no user sign-off obtained), do not modify its logging calls — surface the conflict to the user and wait for instruction rather than silently falling back to inline logging. Present at least these options: (1) make the type `partial`; (2) move the `LoggerMessage` methods to a separate type — group reusable messages in one shared class, and keep the rest in a per-type class.
