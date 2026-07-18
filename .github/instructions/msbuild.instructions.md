---
description: "Use when editing MSBuild files (.csproj, .props, .targets). Covers central package management, artifacts layout, and property inheritance."
applyTo: "**/*.{csproj,props,targets}"
---
# MSBuild Conventions

## Central Package Management

All package versions are declared in `Directory.Packages.props` at the repo root. In `Directory.Packages.props`, ensure the last child element of the root `<Project>` tag is `<Import Project="Directory.Packages.template.props" />`. `Directory.Packages.template.props` is **READ ONLY**, never edit it. The template provides default `<PackageVersion>` entries; any entry with the same `Include` declared earlier in `Directory.Packages.props` takes precedence over the template default, because MSBuild uses first-definition-wins semantics and the import appears last.

In `.csproj` files:

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

If the package does not exist in the template, simply add a new `<PackageVersion>` entry in `Directory.Packages.props`. No override logic is needed; the entry will be the sole definition.

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

This is in addition to the automatically generated entries; do not remove or duplicate the default suffix entries.
