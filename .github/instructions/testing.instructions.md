---
description: "Use when creating or modifying test projects under the tests/ folder. Covers naming conventions and structural guardrails."
applyTo: "tests/**"
---
# Test Project Conventions

Build configuration is handled by `tests/Directory.Build.props` — do not restate its settings.

## Framework

- **Testing**: TUnit.
- **Mocking**: NSubstitute (analyzers auto-added when referenced).

## Naming

- Test project: `<ProjectUnderTest>.Tests` (matches InternalsVisibleTo default).
- Test classes: `<ClassUnderTest>Tests`.
- Test methods: must use `MethodName_Condition_ExpectedResult` format. Example: `Add_NegativeNumbers_ThrowsArgumentException`.

## Structure

- Follow **Arrange / Act / Assert**.
- Always propose a set of basic tests for new or changed code. A basic test set must include at minimum: one happy-path test, one test per documented edge case or boundary condition, and one test for each expected exception.
- If no edge cases are explicitly documented, use judgment to identify and cover the most likely boundary conditions (e.g. null inputs, empty collections, zero/max values) and note the assumption in a comment.
