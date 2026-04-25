---
description: "Convert an MVC controller to Minimal API endpoints following the project's ASP.NET conventions."
agent: "agent"
---

# Convert Controller to Minimal API

Refactor an MVC controller into Minimal API endpoints aligned with [aspnet-api.instructions.md](../instructions/aspnet-api.instructions.md).

## Input

Path to the controller file (e.g., `src/MyApi/Controllers/OrdersController.cs`). Ask if not provided.

## Steps

### 1. Analyse the Controller

For each action method, capture:

- HTTP verb and route template.
- Parameter binding sources (`[FromRoute]`, `[FromQuery]`, `[FromBody]`, `[FromServices]`).
- Return type and possible status codes.
- Filters / attributes (`[Authorize]`, `[ProducesResponseType]`, etc.).
- Async signature and `CancellationToken` usage.

### 2. Produce a Migration Plan

Present a table to the user before writing any code:

| Action | New endpoint | Notes (auth, filters, response types) |

Ask for confirmation or adjustments.

### 3. Generate the Endpoint File

Create `<Resource>Endpoints.cs` next to the controller, following the conventions:

- Static class with a `MapXEndpoints(this IEndpointRouteBuilder)` extension.
- `MapGroup("/api/v{apiVersion}/{resource}")` with shared `.WithTags(...)`.
- Use **`TypedResults`** for every return path.
- Use **`[AsParameters]`** for complex parameter sets.
- Use **`CancellationToken`** on every async lambda.
- Errors return **`ProblemDetails`** via `TypedResults.Problem(...)`.
- Endpoint lambdas stay thin; delegate to injected services.

### 4. Wire Up Registration

In `Program.cs`, replace `MapControllers()` (or supplement it) with the new `MapXEndpoints()` call. If the controller was the last one, also remove `AddControllers()`.

### 5. Migrate Tests

For each test targeting the controller, port it to use `WebApplicationFactory` against the new endpoint. Keep the file name and test names; update the arrange/act sections only.

### 6. Delete the Controller

Only after the new endpoints + tests pass. Run `dotnet build` and `dotnet test` before deletion.

### 7. Update README

If the project's `README.md` documented the old controller routes, update it in the same commit (per [copilot-instructions.md](../copilot-instructions.md)).

## Guardrails

- Do not change behaviour while converting — refactor first, behaviour changes go in a separate commit.
- If a controller uses filters or model binders without a Minimal API equivalent, surface the gap and ask before inventing one.
