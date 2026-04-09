# AGENTS.md

## Repo overview

Early-stage .NET SDK for [Opik](https://github.com/comet-ml/opik). Single git commit; no tests, CI, or README yet.

## Structure

```
OpikSimplSdk/
  OpikSimplSdk.sln
  OpikSimplSdk.Core/   # Core abstractions (namespace: OpikSimplSdk.Core)
  OpikSimplSdk.Http/   # HTTP client layer (namespace: OpikSimplSdk.Http)
```

- Solution root is `OpikSimplSdk/`, not the repo root.
- Both projects target **net10.0** with `Nullable` and `ImplicitUsings` enabled.
- `OpikSimplSdk.Http` declares `IOpikClient` (currently empty interface).
- `OpikSimplSdk.Core/Class1.cs` is a scaffold placeholder — rename/replace it.

## Commands

All commands run from `OpikSimplSdk/` (where the `.sln` lives):

```bash
# Build entire solution
dotnet build OpikSimplSdk.sln

# Build a single project
dotnet build OpikSimplSdk.Core/OpikSimplSdk.Core.csproj

# Restore packages
dotnet restore OpikSimplSdk.sln
```

No test projects exist yet. When added, run with:
```bash
dotnet test OpikSimplSdk.sln
```

## Quirks

- `obj/` directories are **committed** (not ignored) — they contain NuGet restore artifacts. Don't delete them unless you re-run `dotnet restore`.
- `.idea/` is ignored via `.gitignore`; the repo was created with JetBrains Rider.
- No `Directory.Build.props` or `Directory.Packages.props` yet — each `.csproj` manages its own properties.
- `net10.0` requires .NET SDK 10. Verify with `dotnet --version` before building.
