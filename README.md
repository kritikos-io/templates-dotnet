# Templates - Dotnet

A starting point for new .NET projects, based on opinionated rules.

1. In order to be able to update your repository with the latest changes, you can use the following command **after creating** your repo:

```bash
git remote add template https://github.com/kritikos-io/templates-dotnet
git fetch --all
git merge template/main --allow-unrelated-histories
```

2. Do this as soon as possible, as the unrelated histories flag will lead to a few conflicts that you will need to resolve manually.
3. Afterwards, you can pull future changes using

```bash
git pull template main
```

4. Rename the solution and project files, replacing 'Solution' to match your project name.
   1. Solution.sln
   2. Solution.sln.DotSettings
   3. Solution.code-workspace

> Keep in mind that until the dotnet toolset handles generating new projects correctly, you will need to edit new csproj files and remove Version attributes from PackageReference entries. For more details consult [Central Package Management].

> Provided props files allow compiled models with EF Core 9+, to use them install `Microsoft.EntityFrameworkCore.Tasks` on all projects containing DbContext classes. (Not yet suited for production use, consult [Entity Framework Core MSBuild integration]).

[Central Package Management]: https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management
[Entity Framework Core MSBuild integration]: https://learn.microsoft.com/en-us/ef/core/cli/msbuild
