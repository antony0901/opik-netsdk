# AGENTS.md

## Repo overview

.NET SDK for [Opik](https://github.com/comet-ml/opik). M1–M3 milestones are complete; 119 tests pass. M4 (Attachments, Check, Workspaces) remains.

## Structure

```
OpikSimplSdk/
  OpikSimplSdk.sln
  EXAMPLES.md
  OpikSimplSdk.Core/          # Core abstractions (namespace: OpikSimplSdk.Core)
    Common/
      Enums.cs                # FeedbackScoreSource, SpanType, FeedbackDefinitionType, etc.
      OpikClientConfig.cs     # BaseUrl, ApiKey, WorkspaceName
      Optional.cs             # Optional<T> + JSON converter for omit/null semantics
      RequestOptions.cs       # Timeout, ChunkSize
    Clients/
      ClientInterfaces.cs     # All IXxxClient interfaces
    Models/
      FeedbackDefinitions.cs  # Discriminated-union feedback types
      Placeholders.cs         # Stub request/response model types
    Class1.cs                 # Assembly marker (harmless, do not delete)
  OpikSimplSdk.Http/          # HTTP client layer (namespace: OpikSimplSdk.Http)
    Infrastructure/
      AuthHeaderMode.cs       # Enum: AuthorizationBearer | CometSdkApiKey
      IOpikHttpTransport.cs
      OpikHttpTransport.cs    # Auth, base URL, timeout, streaming
      OpikJson.cs             # Shared JsonSerializerOptions
    Clients/
      ClientBase.cs           # Query-string helpers shared by all clients
      TracesClient.cs
      SpansClient.cs
      DatasetsClient.cs
      ExperimentsClient.cs    # Includes 4 MB bulk guard
      ProjectsClient.cs
      PromptsClient.cs
      FeedbackDefinitionsClient.cs
    IOpikClient.cs            # Root interface exposing all sub-clients
    OpikClient.cs             # Concrete implementation with lazy sub-client init
  OpikSimplSdk.Tests/         # xUnit test project (119 tests, all passing)
    TestInfrastructure/
    CoreTypesTests.cs
    OpikHttpTransportTests.cs
    OpikClientTests.cs
    TracesClientTests.cs
    SpansClientTests.cs
    DatasetsClientTests.cs
    ExperimentsClientTests.cs
    ProjectsClientTests.cs
    PromptsClientTests.cs
    FeedbackDefinitionsClientTests.cs
```

- Solution root is `OpikSimplSdk/`, not the repo root.
- All projects target **net10.0** with `Nullable` and `ImplicitUsings` enabled.
- `IOpikClient` (in `OpikSimplSdk.Http`) is the root entry point exposing all sub-clients as properties.
- Interfaces live in `OpikSimplSdk.Core`; HTTP implementations live in `OpikSimplSdk.Http`.
- `Attachments`, `Check`, and `Workspaces` clients are **not yet implemented** (M4).

## Commands

All commands run from `OpikSimplSdk/` (where the `.sln` lives):

```bash
# Build entire solution
dotnet build OpikSimplSdk.sln

# Build a single project
dotnet build OpikSimplSdk.Core/OpikSimplSdk.Core.csproj

# Restore packages
dotnet restore OpikSimplSdk.sln

# Run tests
dotnet test OpikSimplSdk.sln
```

## Quirks

- `obj/` directories are **committed** (not ignored) — they contain NuGet restore artifacts. Don't delete them unless you re-run `dotnet restore`.
- `.idea/` is ignored via `.gitignore`; the repo was created with JetBrains Rider.
- No `Directory.Build.props` or `Directory.Packages.props` yet — each `.csproj` manages its own properties.
- `net10.0` requires .NET SDK 10. Verify with `dotnet --version` before building.
- `Class1.cs` in `OpikSimplSdk.Core` is kept as an assembly marker; it is harmless but should not be deleted.
- Model types in `Placeholders.cs` are stubs — they compile but have no real properties yet.
