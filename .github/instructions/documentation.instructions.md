---
description: "README.md authoring: structure, callouts, diagrams, and assets."
applyTo: "**/README.md"
---
# README Style

## Structure

Follow this ordering when sections apply; omit those that don't:

1. **Title** (`# <ProjectName>`) and one-sentence purpose.
1. **Getting Started** — minimal commands to consume or run the project.
1. **Features** / **Capabilities** — bulleted highlights.
1. **Configuration** — build args, environment variables, MSBuild properties.
1. **Usage Examples** — code or CLI snippets.
1. **Recommendations** / **Caveats** — known limitations, in callouts.
1. **Reference link definitions** at the bottom.

## Conventions

- Do not hard-wrap prose; one paragraph per line.
- Ordered lists: write every item as `1.` and let the renderer auto-number.
- Use reference-style links for external URLs in prose, with definitions at the bottom of the file. Inline links are fine in tables and for in-repo relative paths.
- Keep code examples minimal — strip boilerplate unless it is the point.

## Callouts

Use [GitHub-flavoured alerts](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax#alerts) for emphasis; reserve plain `>` blockquotes for quoted text.

| Alert | Use for |
| --- | --- |
| `> [!NOTE]` | Neutral context the reader should know |
| `> [!TIP]` | Optional best-practice |
| `> [!IMPORTANT]` | Required for correct usage |
| `> [!WARNING]` | Risk of breakage or data loss |
| `> [!CAUTION]` | Security or destructive-action risk |

## Mermaid Diagrams

- Match diagram type to intent: `flowchart` (processes), `sequenceDiagram` (interactions over time), `classDiagram` (type relationships), `erDiagram` (data models).
- Keep diagrams under ~15 nodes; split larger ones.
- No custom styling — defaults render in both light and dark modes.

## Assets

- Repo-wide assets in `/docs/assets/`; project-scoped assets in the project's own `docs/assets/`.
- Prefer **SVG** for diagrams and logos; PNG only for raster screenshots.
