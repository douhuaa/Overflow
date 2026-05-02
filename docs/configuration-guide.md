# AppHost Configuration Strategy

This document describes how configuration is layered in `Overflow.AppHost` and how to manage
defaults, local overrides, and secrets.

---

## Configuration Layers

### 1. Non-sensitive configuration — Options objects

Ports, image tags, virtual hosts, and endpoint names are **non-sensitive** and are expressed as
typed Options classes bound from `appsettings.json`.

| Options class        | Config section             | Description                                |
|----------------------|----------------------------|--------------------------------------------|
| `IdentityOptions`    | `Infrastructure:Identity`  | Keycloak host port, internal port, vhost   |
| `PostgresOptions`    | `Infrastructure:Postgres`  | Postgres port, pgAdmin port & image tag    |
| `TypesenseOptions`   | `Infrastructure:Typesense` | Typesense port, image tag, endpoint name   |
| `RabbitMqOptions`    | `Infrastructure:RabbitMq`  | RabbitMQ management port                   |

These values are read in `AppHost.cs` via `IConfiguration.GetSection(...).Bind(...)` and passed
into `AddInfrastructure(options)` at startup.

**Default values** are declared in two places:
- As C# property initializers in the Options class (fallback when no config is present)
- In `appsettings.json` under the `"Infrastructure"` section (canonical documented defaults)

**Overriding defaults** — use any standard .NET configuration source:
- `appsettings.Development.json` for local developer overrides
- Environment variables (e.g. `Infrastructure__Typesense__Port=9999`)
- Any other `IConfiguration` provider

### 2. Sensitive configuration — Aspire Parameters

API keys, passwords, connection strings, and other secrets are **never** stored in code or
`appsettings.json`. They are expressed as Aspire external parameters.

| Parameter name      | Secret | Used by               |
|---------------------|--------|-----------------------|
| `typesense-api-key` | ✅ yes | Typesense, SearchService |

Parameter names are defined in `SecretParameterNames`.

### 3. Stable internal identifiers — Constants

Resource logical names, environment-variable keys, and Aspire parameter names that are
**compile-time stable** live in dedicated constant classes:

| Class                   | Contents                                                |
|-------------------------|---------------------------------------------------------|
| `AppHostResourceNames`  | Aspire resource names (`"keycloak"`, `"postgres"`, …)  |
| `SecretParameterNames`  | Aspire secret parameter names (`"typesense-api-key"`)  |
| `EnvironmentVariableNames` | In-process env-var keys (`TYPESENSE_API_KEY`, …)    |
| `AppHostConstants`      | Gateway/frontend/proxy ports and virtual hosts          |

---

## Local Development — Providing Secrets

Use [.NET User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) to supply
parameter values locally.

```bash
# In the Overflow.AppHost directory:
dotnet user-secrets set "Parameters:typesense-api-key" "your-local-api-key"
```

Aspire reads parameters from `Parameters:<name>` in the configuration hierarchy.

---

## Cloud / Production — Providing Secrets

Connect your deployment environment to a secret store (e.g., Azure Key Vault) and map each
parameter to its secret path. Do **not** commit secrets to the repository.

---

## Adding New Infrastructure Resources

When adding a new resource, decide which category its config belongs to:

| Config type                        | Where it lives                              |
|------------------------------------|---------------------------------------------|
| Port, image tag, virtual host, etc.| Add a property to the relevant Options class and `appsettings.json` |
| API key, password, connection string | `builder.AddParameter("...", secret: true)` + `SecretParameterNames` |
| Resource logical name              | `AppHostResourceNames`                       |

**Convention**:
- Do not add new sensitive values to `AppHostConstants` or `appsettings.json`.
- Do not hard-code ports or image tags as `static const` — put them in Options.
