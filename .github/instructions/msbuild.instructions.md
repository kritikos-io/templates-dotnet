---
description: "Use when editing MSBuild files (.csproj, .props, .targets). Covers central package management, artifacts layout, and property inheritance."
applyTo: "**/*.{csproj,props,targets}"
---
# MSBuild Conventions

## Central Package Management

All package versions are declared in `Directory.Packages.props` at the repo root. In `.csproj` files:

```xml
<!-- Correct -->
<PackageReference Include="Newtonsoft.Json" />

<!-- Wrong — never specify Version -->
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

To add or update a version, edit `Directory.Packages.props`:

```xml
<PackageVersion Include="Newtonsoft.Json" Version="13.0.3" />
```

## Properties Set Centrally (Do Not Override)

These are configured in the root `Directory.Build.props` — do **not** set them in individual projects:

`LangVersion`, `Nullable`, `ImplicitUsings`.

## InternalsVisibleTo

Handled automatically via `<InternalsVisibleToSuffix>` in the root props. Default suffixes are `.Tests` and `Tests`. To add custom visibility:

```xml
<ItemGroup>
  <InternalsVisibleTo Include="MyProject.IntegrationTests" />
</ItemGroup>
```
