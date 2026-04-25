---
description: "Convert inline logger calls to LoggerMessage-generated partial methods, per the C# conventions."
agent: "agent"
---

# Refactor to LoggerMessage

Replace inline `logger.LogX(...)` calls with `LoggerMessage`-generated `partial` methods, as required by [csharp.instructions.md](../instructions/csharp.instructions.md).

## Input

A file or project path. If unspecified, default to the active editor file.

## Steps

### 1. Inventory

Find every `ILogger.Log*(...)` call site (`LogTrace`, `LogDebug`, `LogInformation`, `LogWarning`, `LogError`, `LogCritical`). Group by containing class.

### 2. Plan

For each class with hits, present a table before editing:

| Method | Level | EventId | Message template | Args |

Pick stable, sequential `EventId` values per class (start at 1 within the class). Confirm with the user when message templates need to be reworded for clarity.

### 3. Convert the Class

- Mark the class `partial`.
- Add a nested `static partial class Log` (or top-level partial methods, per existing project style — match what's already in the codebase).
- For each call site, generate:
  ```csharp
  [LoggerMessage(EventId = N, Level = LogLevel.X, Message = "...")]
  static partial void <Verb><Subject>(ILogger logger, <args>);
  ```
- Replace each call site with the new method invocation.
- Method names: `PascalCase`, verb-led, third-person (`OrderProcessed`, not `LogOrderProcessed` or `OrderWasProcessed`).

### 4. Handle Edge Cases

- **Exceptions**: `Exception` parameter goes first; do not include it in the message template.
- **Scopes** (`logger.BeginScope`): leave alone — `LoggerMessage` does not replace scopes.
- **Conditional logging** (`if (logger.IsEnabled(...))`): drop the guard; the generated method already checks.

### 5. Verify

```bash
dotnet build
```

Source generators must produce zero warnings. If a `[LoggerMessage]` definition is rejected, surface the analyzer message and the offending method.

### 6. Run Tests

```bash
dotnet test
```

Logger-based assertions in tests usually keep working, but message-template changes can break string-matching tests — flag any test failures explicitly.

## Guardrails

- Do not change log levels while refactoring — that's a separate decision.
- Do not invent new log statements; only convert what already exists.
- If a class is `sealed` and has external partial declarations, surface the conflict before adding `partial` to the type.
