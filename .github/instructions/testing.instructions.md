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
- Test methods: descriptive names — `MethodName_Condition_ExpectedResult` or similar.

## Structure

- Follow **Arrange / Act / Assert**.
- Always propose a set of basic tests for new or changed code.
