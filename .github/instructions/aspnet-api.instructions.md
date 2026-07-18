---
description: "Use when writing API endpoints, routing, middleware, request/response handling, OpenAPI configuration, or HTTP pipeline setup in ASP.NET. Covers Minimal API conventions, error handling, and endpoint organisation."
---
# ASP.NET API Conventions

## Routing

- All endpoints live under `/api/v{apiVersion}/{resource}`.
- Default to `/api/v1` for projects without explicit API versioning.

## Endpoints

- Prefer **Minimal APIs**; use Controllers only when the endpoint group requires action filters, model binding customisation, or exceeds 10 endpoints sharing significant cross-cutting logic.
- Group related endpoints using `MapGroup()` with a shared prefix and tag.
- Use **typed results** (`TypedResults.Ok()`, `TypedResults.NotFound()`) over untyped `Results` for OpenAPI metadata inference.
- Return **`ProblemDetails`** (RFC 9457) for all error responses — configure via `AddProblemDetails()`.

## Authorisation

- Apply `.RequireAuthorization()` at the group level for protected resources.
- Reference policy names through `const string` fields on a shared static class (e.g., `.RequireAuthorization(AuthorizationPolicies.RequireAdminRole)`) instead of inline string literals.
- Mark public endpoints explicitly with `.AllowAnonymous()`.

## Request Handling

- Bind request data with **`[AsParameters]`** for complex parameter sets.
- Validate input at the endpoint boundary — use **filters** or **`IValidatableObject`** to fail fast.
- Use **`CancellationToken`** on all async endpoints.

## Endpoint Organisation

- One file per resource or feature area (e.g., `UserEndpoints.cs`, `OrderEndpoints.cs`).
- Register via extension methods on `IEndpointRouteBuilder`: `MapUserEndpoints()`.
- Keep endpoint lambdas thin — delegate business logic to injected services.

## OpenAPI

- Annotate endpoints with `.WithTags()` for grouping in the generated spec.
