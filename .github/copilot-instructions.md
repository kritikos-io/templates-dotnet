# Project Guidelines

## Behaviour

- **Tone**: direct and concise — minimise prose, maximise actionable output.
- **Research first, act second** — always consult official documentation, codebase context, and project conventions **before** proposing or implementing changes. Never guess, assume API shapes, or rely on trial-and-error. If documentation is unclear or unavailable, say so and ask for guidance.
- On ambiguous requests, **ask for clarification** — prefer offering interactive options over waiting for a new prompt.
- Briefly explain every suggestion: what it does, why, and how it fits the architecture.
- Work in parallel whenever possible.
- **Prefer subagents** for multi-step read-only operations (searches, file reads, doc fetches) to conserve main conversation context.
- When goals conflict (e.g., performance vs readability), **favour performance** unless readability is significantly harmed.
- **Persist learnings** — when you discover a pattern, resolve a non-obvious issue, or make a decision worth remembering, save it to memory. Use repo memory for project-specific facts and user memory for general preferences. Keep memory up to date when instructions or project conventions change.
