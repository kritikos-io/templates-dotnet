---
description: "Use when editing files under docs/adr/. Enforces ADR immutability and supersession rules."
applyTo: "docs/adr/**"
---
# ADR Editing Rules

ADRs are append-only history. To create a new ADR, use the `new-adr` skill.

- **Accepted ADRs are immutable.** Do not change `Context`, `Decision Drivers`, `Considered Options`, or `Decision Outcome` after acceptance — write a new ADR that supersedes instead.
- **Permitted edits to accepted ADRs**: typo/grammar fixes, adding links to related ADRs, and updating the `status` field as part of supersession or deprecation.
- **Superseding**: set the predecessor's frontmatter to `status: superseded by ADR-NNNN`, update its `date`, and add a back-link in its body. Never delete the predecessor.
- **Filenames are permanent.** Do not renumber or rename existing ADRs even if numbers have gaps.
