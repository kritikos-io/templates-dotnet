---
name: new-adr
description: 'Create a new Architectural Decision Record (ADR) capturing a justified design choice that addresses an architecturally significant functional or non-functional requirement. Use when: creating a new ADR, documenting an architectural choice, recording a design decision, superseding a previous decision, capturing rationale for a framework/library/pattern selection.'
argument-hint: 'Brief description of the decision (e.g., "adopt Minimal APIs over MVC controllers")'
---
# New Architectural Decision Record

You are assisting the user in producing a well-structured, comprehensive ADR. Ask clarifying questions, offer suggestions and examples, and ensure the record follows the standard format with all required sections filled.

Reference: <https://adr.github.io>.

## When to Use

- A design choice has a **measurable effect on architecture or quality attributes** (security, performance, scalability, maintainability, cost, DX).
- The decision is **non-obvious**, has **trade-offs**, or **supersedes** prior guidance.
- A reviewer or future maintainer would reasonably ask *"why did we do it this way?"*.

**Not an ADR**: cosmetic refactors, bug fixes, style preferences, or decisions with no viable alternatives. Use a commit message or code comment instead.

## Location & Filename

- **Folder**: `docs/adr/`
- **Filename**: `NNNN-kebab-title.md` (zero-padded, four digits, sequential; e.g., `0001-use-minimal-apis.md`).
- **Index** (optional): `docs/adr/README.md` listing all ADRs with status.

Determine the next `NNNN` by scanning existing files and incrementing the highest number. Never reuse numbers, even for retracted ADRs.

## Required Sections

Every ADR must include these MADR sections, in order:

1. **Context and Problem Statement** — the situation, goals, and constraints that frame the decision; pose the problem as a question when helpful.
2. **Decision Drivers** — bullet list of forces, concerns, and quality attributes that matter.
3. **Considered Options** — bullet list of viable alternatives.
4. **Decision Outcome** — the chosen option, justification, nested **Consequences** (good/bad/follow-up), and optional **Confirmation** subsection.

MADR-optional sections (include when they add value):

- **Pros and Cons of the Options** — per-option Good/Neutral/Bad bullets. Prefix with the comparison table below when more than two options compete.
- **More Information** — links, revisit notes, related ADRs.

### Comparison Table (project addition)

When using **Pros and Cons of the Options**, lead with a Yes/No/Maybe matrix scoring each option against each driver. This is a project convention on top of MADR, not part of the spec, but it forces explicit comparison and exposes weak options.

```markdown
| Driver     | Option A | Option B | Option C |
| ---------- | -------- | -------- | -------- |
| {driver 1} | Yes      | Maybe    | No       |
| {driver 2} | No       | Yes      | Yes      |
```

## Status Lifecycle

`proposed` → `accepted` → (`deprecated` | `superseded by ADR-NNNN`) · `rejected` is terminal from `proposed`.

- New ADRs start as `proposed` unless the user confirms acceptance.
- When superseding, **update the old ADR's status** to `superseded by ADR-NNNN` and add a back-link. Never rewrite history of an accepted ADR.
- `date` reflects the last status change.

## MADR Template

This skill follows the [MADR 4.0](https://adr.github.io/madr/) (Markdown Any Decision Records) format with one project addition (see *Comparison Table* above). Copy [MADR.md](MADR.md) to `docs/adr/NNNN-kebab-title.md` and fill in the placeholders. The folder `docs/adr/` is a project convention; MADR's own recommendation is `docs/decisions/`.

## Procedure

1. **Confirm significance.** Check against the "When to Use" criteria. If it doesn't qualify, suggest a lighter alternative (code comment, PR description, CHANGELOG entry) and stop.
2. **Assign a number.** List `docs/adr/` and pick the next `NNNN`.
3. **Interview the user — one section at a time.** Ask focused questions for the current section, offer suggestions/examples where useful, then move on. Do not dump every question at once.
4. **Show progress** after each section using a status table with these markers:
   - ✔️ completed
   - 🟡 in progress
   - ➖ not started

   ```markdown
   | Section                       | Status |
   | ----------------------------- | ------ |
   | Context and Problem Statement | ✔️     |
   | Decision Drivers              | 🟡     |
   | Considered Options            | ➖     |
   | Decision Outcome              | ➖     |
   | Consequences                  | ➖     |
   | Pros and Cons (optional)      | ➖     |
   ```

5. **Draft the ADR** as sections are filled. Keep the title short and decision-oriented (`"Use X for Y"` or `"Adopt X over Y"`). Status = `proposed` unless the user confirms acceptance.
6. **Handle supersession.** If this ADR replaces another, update the predecessor's frontmatter `status` to `superseded by ADR-NNNN` and add a cross-link in its body.
7. **Update index.** If `docs/adr/README.md` exists, append the new entry with status; otherwise offer to create one.
8. **Save & confirm.** Write the file to `docs/adr/NNNN-kebab-title.md`, validate frontmatter YAML and filename, and confirm the saved path back to the user.

## Quality Bar

- **Title states the decision**, not the topic. ✅ `Use Minimal APIs for new endpoints` · ❌ `API style`.
- **Options are real alternatives** that were genuinely considered — not strawmen.
- **Consequences include the downsides** we accept. An ADR with only positives is incomplete.
- **Links > prose** for context already documented elsewhere (issues, RFCs, benchmarks).
- **Immutable once accepted.** Further changes happen via a new ADR that supersedes.
