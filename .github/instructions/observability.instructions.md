---
description: "Use when adding tracing, metrics, or feature flags: ActivitySource/Meter setup, semantic-convention tags, OTLP exporter config, OpenFeature client usage and flag-key naming."
---
# Observability & Feature Flags

## OpenTelemetry

- One **`ActivitySource`** per logical component, named `Kritikos.<Project>.<Component>`. Declare as `static readonly` on a dedicated `Telemetry` static class within the component (avoid scattering sources across every type).
- One **`Meter`** per component, alongside its `ActivitySource`. The names **may** match when both belong to the same component.
- Always wrap `StartActivity(...)` in a `using` — leaked activities skew downstream timings.
- **Activity names**:
  - When [OpenTelemetry semantic conventions](https://opentelemetry.io/docs/specs/semconv/) cover the operation (HTTP, DB, messaging, RPC), follow semconv exactly (`HTTP GET`, `db.query`, `messaging.process`).
  - For custom operations not covered by semconv: `<Verb><Object>` in PascalCase (`ProcessOrder`, `LoadConfiguration`). Avoid generic verbs like `Handle` or `Execute`.
- **Tags / attributes**: lowercase dotted, follow semantic conventions where they exist (`http.request.method`, `db.system`, `messaging.destination.name`). Project-specific tags use the `kritikos.<area>.<name>` namespace.
- **Metric instruments**: snake_case names with units in the suffix (`orders_processed_total`, `request_duration_seconds`). Always set `unit` and `description` at creation.
- **Resource attributes**: when a shared registration helper is added, it should populate `service.name` from `AssemblyName` and `service.version` from the GitVersion-computed `InformationalVersion`. Do not set them manually at call sites.
- **Exporter**: OTLP over gRPC, endpoint via `OTEL_EXPORTER_OTLP_ENDPOINT`. Do not register console exporters in `Release` builds.
- **Sampling**: parent-based with `TraceIdRatioBased` (1.0 in dev, configurable in prod). Do not set custom samplers without justification.

## OpenFeature

- Resolve flags through the configured OpenFeature client (obtained via DI, or `Api.Instance.GetClient()` if no DI integration is wired). Never call provider SDKs directly.
- **Flag keys**: `kebab-case`, prefixed by domain (`orders-allow-partial-fulfilment`, `auth-require-mfa`). Keys are stable contracts — treat renames like API breaking changes.
- Always pass `defaultValue:` — `client.GetBooleanValue("flag", defaultValue: false, context)`.
- **Evaluation context** is built once per request via the configured context provider — do not assemble ad-hoc contexts inline.
- Long-lived flags (kill switches, licensing) live in code; short-lived flags (rollouts, experiments) must have a removal date in their description and a tracking issue.
