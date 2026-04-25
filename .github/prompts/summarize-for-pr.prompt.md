---
description: "Create a temporary markdown file summarising all branch changes vs. main/master for PR review"
agent: "agent"
---

# Summarize Branch Changes for PR

Create a temporary markdown file in the repository root called `BRANCH_CHANGES.md` that summarises all changes on the current branch compared to the master/main/default branch. This file is intended to aid PR review and should be deleted before merging.

## Input

No explicit input required — the summary is derived from git history and the current Copilot session context.

## Steps

### 1. Gather Git Context

- Determine the current branch name (`git branch --show-current`).
- Find the merge base with `origin/master`.
- Collect the commit log between the merge base and HEAD (`git log --oneline`).
- Collect the diff stat (`git diff --stat`), file change list (`--diff-filter`), and numstat.

### 2. Distil Session Requests (if applicable)

If Copilot session context is available, extract the key user requests that drove the changes and present them as a bullet-point list at the top of the file under a **Copilot Session Requests** heading.

### 3. Produce the Markdown File

Create `BRANCH_CHANGES.md` in the repository root with the following sections:

#### Header

- Branch name, base branch, and current date.

#### Copilot Session Requests

- Bullet list of the user prompts/requests from the current session that led to changes (omit if no session context).

#### Commits

- Table with columns: Hash, Message, Author, Date.

#### Files Changed

- Table with columns: File, Change type (Added/Modified/Deleted), Insertions, Deletions.
- Summary line: _N files changed, X insertions, Y deletions._

#### Change Details

- One sub-heading per changed file (or logical group of related files).
- Numbered list of the distinct changes made, written for a reviewer who is unfamiliar with the recent context.

#### Pending Session Work (if applicable)

- Any in-progress or uncommitted work from the current Copilot session that has not yet been committed.

## Guidelines

- Keep descriptions concise but specific — a reviewer should understand _what_ changed and _why_ without reading the diff.
- Use git data as the source of truth; supplement with session context only for intent and motivation.
- Do not include the full diff content — summarise it.
- Mark the file clearly as temporary (mention it should be deleted before merge).
