# Backend & API Instructions

## Architecture
- **CQRS + MediatR** pattern: Features use `IRequest`/`IRequestHandler` with Command/Query separation
- **Autofac** for dependency injection (each feature has its own `AutofacModule.cs`)
- **Feature folders**: `Core/FMG.Integrations.Applications/Features/{Domain}/{FeatureName}/`
- **Infrastructure implementations**: `InfrastructureLayer/FMG.Integrations.Infrastructures/Features/{Domain}/{FeatureName}/`
- **Apply Aggregate Root pattern in Microservices

## API Versioning
- Versioned via `[ApiVersion("X.0")]` attribute
- Controllers: `Apps/FMG.Contact.Api/Controllers/v{X}/`
- Models: `Apps/FMG.Contact.Api/Models/v{X}/`
- Current target version: **V5**

## V5 API Patterns (Reference: GroupsController V5)
- Use `ProblemDetails` / `ValidationProblemDetails` for error responses (RFC 7807)
- Use XML doc comments (`/// <summary>`) on controllers and models
- Use `init` setters for request/response model properties
- Use `ILogTrace` for structured logging
- Use `ToActionResult` extension for `Result<T>` pattern
- Use proper HTTP status codes: `201 Created`, `202 Accepted`, `204 NoContent`
- Use `[ProducesResponseType]` with `ProblemDetails` instead of `BaseResponse`
- Namespace: `FMG.Contact.Api.Models.v5`

## Naming Conventions
- Request models: `{Action}{Entity}Request.cs` (e.g., `CreateContactRequest.cs`)
- Response models: `{Action}{Entity}Response.cs` (e.g., `CreateContactResponse.cs`)
- Feature classes: `Feature.cs` inside the feature folder with nested `Command`/`Query`, `Handler`, and response classes
- Keep property names consistent with existing Feature.Command properties

## Feature Structure Rules
- **Never use try-catch in controllers** — use `Result<T, AppError>` pattern and `ToActionResult()` extension for error handling
- **Always implement interfaces on Command/Query** — use `IRequest<Result<TResponse, AppError>>, IFeatureContract, IFmgRequestV3` pattern (refer to existing features)
- **Keep Handler inside Feature class** — Handler must be a nested class within `Feature`, not a separate file in Infrastructure layer
- **Use service builders instead of creating clients directly** — use `ISfApiClientBuilder<ISfApiClient>` pattern instead of `IGenericRefitClientFactory` + manual client creation (reference: `Features/CrmGroups/GetCrmGroups/Feature.cs`)

## Error Handling (V5)
```csharp
return Problem(
    statusCode: (int)HttpStatusCode.BadRequest,
    title: "Operation Not Allowed",
    detail: ex.Message,
    instance: $"{HttpContext.Request.Method} {HttpContext.Request.Path}",
    type: $"/contact/errors/{(int)errorCode}"
);
```

## Coding Conventions
- **Never use DataAnnotations for validation** — do not use `[Required]`, `[StringLength]`, `[Range]`, etc. from `System.ComponentModel.DataAnnotations`. Use FluentValidation with `AbstractValidator<T>` instead.
- **Never throw `System.Exception`** — use specific exception types (`InvalidOperationException`, `ArgumentException`, or domain-specific exceptions from `FMG.Integrations.Domains.Exceptions`)
- **Define constants for repeated string literals** — if a literal (e.g., `"company"`) is used 3+ times, extract it to a `private const string`
- **Use format providers for date/time parsing** — always pass `CultureInfo.InvariantCulture` and `DateTimeStyles` to `DateTime.TryParse`/`DateTime.Parse`
- **Simplify loops with LINQ** — prefer `.Select()`, `.Where()`, `.ToDictionary()` over manual `foreach` + `if` + `Add` patterns
- **Use concrete types over interfaces when possible** — if a parameter is always a concrete type (e.g., `ContactBase`), prefer it over the interface (`IContactBase`) for improved performance
- **Mark methods `static` when they don't access instance data** — private/internal methods that don't use `this` should be `static`
- **Remove always-true/false conditions** — if a condition is trivially always true (e.g., checking `.Count > 0` on a list initialized with items), simplify the code to remove the dead branch
- **Never use anonymous types for data transfer** — always create explicit classes (e.g., `CrmContactCreatedEvent`) instead of anonymous objects like `new { Property = value }`. This applies to event data, DTOs, and any object passed between methods. Place event classes in `Share/Events/` folder.

## Unit Tests
- Test projects are in `UnitTests/`
- Refit interfaces (e.g., `IContactApiV3.cs`, `IContactApiV5.cs`) define the API contract
- When adding V5 endpoints, add corresponding Refit interface methods

---

## Handler Refactoring Steps

When refactoring a Feature Handler, follow these steps in order:

### Step 1: Define the flow
Write out the numbered step sequence the handler should follow. Example:
1. Load context (DB call)
2. Validate context
3. Build external client
4. Load shared data needed by multiple steps (e.g., synced IDs)
5. Primary operation (search/create/update)
6. Conditional secondary operation (reuse same method from step 5)

### Step 2: Identify repeated patterns
Look for the same sequence of calls appearing more than once in the handler (e.g., getFields → search → fetchGroups → markSynced). These are candidates for a single service method.

### Step 3: Extract to service method
- Create one method on the **service interface** (in `Applications/`) that encapsulates the repeated sequence
- Implement it in the **infrastructure service** (in `Infrastructures/`), composing the existing private/internal methods
- The new method should accept all data it needs (shared state like `syncedCampaignIds`) as parameters — do not re-fetch inside

### Step 4: Simplify the handler
- Replace the repeated multi-step blocks with single calls to the new service method
- Remove any helper methods from the handler that are now inside the service (e.g., `MarkCampaignSynced`)
- The handler should read as a linear sequence of high-level steps

### Step 5: Validate
- Build the full solution (`dotnet build`)
- Run related unit tests
- Verify no method is called more than once per request (per Workflow step 6)

### Key principles
- **Handler = orchestration only** — it decides _what_ to do and _in what order_, not _how_
- **Service = encapsulated operations** — combines fetch + transform + side-effect marking into one cohesive method
- **Shared data fetched once, passed down** — data needed by multiple steps (e.g., `syncedCampaignIds`) is loaded early and passed as parameters, never re-fetched
- **Each block is self-contained** — search + groups + sync marking happen together per object type, not split across the handler

---

## Feature Creation Conventions

### 1. Namespace

```
FMG.Integrations.Applications.Features.{Domain}.{FeatureName}
```

**Examples:**
- `FMG.Integrations.Applications.Features.Contacts.SearchCrmContacts`
- `FMG.Integrations.Applications.Features.CrmGroups.GetCrmGroups`
- `FMG.Integrations.Applications.Features.Groups.CreateGroup`

### 2. File Structure

Each feature is a single `Feature.cs` file containing all nested classes:

```
Core/FMG.Integrations.Applications/Features/{Domain}/{FeatureName}/
└── Feature.cs   ← Contains Query/Command, Handler, DTOs, Validator
```

### 3. Naming Conventions

| Element | Pattern | Example |
|---------|---------|---------|
| **Folder** | `{Verb}{Entity}` or `{Entity}{Verb}` | `GetCrmGroups`, `CreateGroup`, `SearchCrmContacts` |
| **Query class** (read) | `Query` | `public class Query` |
| **Command class** (write) | `Command` | `public class Command` |
| **Handler class** | `Handler` | `public class Handler` |
| **Response** (Query) | `{Entity}` or `QueryResult` | `CrmGroup`, `QueryResult` |
| **Response** (Command) | `CommandResponse` | `public class CommandResponse` |
| **Validator** | `QueryValidator` / `CommandValidator` | `public class QueryValidator : AbstractValidator<Query>` |

### 4. Inheritance & Interfaces

#### Query (Read Operations)
```csharp
public class Query : IRequest<Result<TResponse, AppError>>, IFeatureContract, IFmgRequestV3
{
    public int PartyId { get; set; }
    public Guid CorrelationId { get; set; }
    // ... other properties
}
```

#### Command (Write Operations)
```csharp
// Option A: With Result<T, AppError> (preferred for V5)
public class Command : IRequest<Result<CommandResponse, AppError>>, IFeatureContract, IFmgRequestV3
{
    public int PartyId { get; set; }
    public Guid CorrelationId { get; set; }
    // ... other properties
}

// Option B: Simple response (legacy)
public class Command : IRequest<CommandResponse>, IFeatureContract, IFmgRequestV3
{
    public int PartyId { get; set; }
    public Guid CorrelationId { get; set; }
    // ... other properties
}

// Option C: No response (side-effect only)
public class Command : IRequest<Result<Unit, AppError>>, IFeatureContract, IFmgRequestV3
```

**Required interfaces:**
- `IRequest<TResponse>` — MediatR request marker
- `IFeatureContract` — Project marker interface
- `IFmgRequestV3` — Requires non-nullable `PartyId` and `CorrelationId` (used by logging behavior)
- `IResponseWithAppError` — (Optional) For features using `Result<T, AppError>` with error pipeline

### 5. Return Type Patterns

| Scenario | Return Type |
|----------|-------------|
| **Query with data** | `Result<List<TDto>, AppError>` or `Result<TDto, AppError>` |
| **Command with response** | `Result<CommandResponse, AppError>` |
| **Command without response** | `Result<Unit, AppError>` |
| **Legacy (avoid in V5)** | `TResponse` (throws exceptions) |

### 6. Error Handling

**Do NOT use try-catch in handlers.** Use `Result<T, AppError>` pattern:

```csharp
// Return failure with specific AppError
if (syncSetting == null)
    return new CrmSyncSetting.SyncSettingNotFound();

// Return failure with generic FeatureError
if (string.IsNullOrWhiteSpace(request.Keyword))
    return Result.Failure<QueryResult, AppError>(
        new FeatureError(EnumAppErrorCode.MissingRequiredFields, "At least one search keyword is required."));

// Chain errors from other services
var result = await _service.DoSomething();
if (result.IsFailure) return result.Error;

// Only throw NotSupportedException for truly unsupported branches
default:
    throw new NotSupportedException($"IntegrationType {syncSetting.IntegrationType} is not supported");
```

**Creating custom AppError:**

```csharp
// In the entity or feature file
public class SyncSettingNotFound : AppError
{
    public SyncSettingNotFound() : base(EnumAppErrorCode.SyncSettingNotFound) { }
}
```

### 7. Logging

Inject `ILogTrace _log` and use structured logging:

```csharp
// Log with named placeholders
_log.Info("IntegrationType: {Value}", syncSetting?.IntegrationType);
_log.Info("SearchCrmContacts - PartyId: {PartyId}, Type: {Type}", request.PartyId, request.Type);

// Push properties for correlation
_log.PushProperty("IntegrationType", integrationType);

// Log warnings
_log.Warning("This GroupId {Value} belongs to CRM and cannot be updated", groupId);

// Log errors
_log.Error(exception);
_log.Error(appError);
```

### 8. Event Collection Scope (for trackable operations)

```csharp
private readonly IEventCollector _eventCollector;

public async Task<Result<TResponse, AppError>> Handle(Query request, CancellationToken ct)
{
    await using var scope = _eventCollector.BeginScope(request.PartyId, request.CorrelationId, "FeatureName");
    // ... handler logic
}
```

### 9. Validation (FluentValidation)

```csharp
public class QueryValidator : AbstractValidator<Query>
{
    public QueryValidator()
    {
        RuleFor(f => f.Type)
            .NotNull()
            .WithName(nameof(Query.Type))
            .WithMessage("The Type field is required.");
    }
}
```

