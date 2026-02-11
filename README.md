# Nedo ASP.NET Core Boilerplate

A production-ready ASP.NET Core 9.0 boilerplate for document management systems, following Clean Architecture principles with PostgreSQL database.

## Features

- ✅ **Clean Architecture** - Single-project structure with clear layer separation
- ✅ **PostgreSQL Database** - Entity Framework Core with code-first migrations
- ✅ **Sindika Naming Conventions** - Database tables and columns follow established patterns
- ✅ **Soft Delete** - All entities support soft deletion with audit trails
- ✅ **RESTful API** - Complete CRUD operations with Swagger documentation
- ✅ **Global Exception Handling** - Centralized error handling middleware
- ✅ **Structured Logging** - Serilog with console and file outputs
- ✅ **Object Mapping** - Mapster for efficient DTO transformations

## Project Structure

```
nedo.asp.boilerplate/
├── Domain/              # Core business entities
├── Application/         # Business logic, DTOs, interfaces
├── Infrastructure/      # Data access, repositories
├── API/                 # Controllers, models, middleware
├── Extensions/          # DI configuration
└── docker-compose.yml   # PostgreSQL container
```

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for PostgreSQL)
- [EF Core CLI Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

```bash
dotnet tool install --global dotnet-ef
```

## Getting Started

### 1. Start PostgreSQL

```bash
docker-compose up -d
```

### 2. Create and Apply Database Migration

```bash
# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update
```

### 3. Run the Application

```bash
dotnet run
```

The API will be available at:
- **Swagger UI**: https://localhost:5001 (or http://localhost:5000)
- **API Base**: https://localhost:5001/api

## Database Schema

### Tables

- **dbs000_project** - Project management
- **dbs000_document** - Document storage and tracking

### Naming Conventions

**Tables**: `dbs000_{entity}` (singular, lowercase)
**Columns**: `{entity}_{property}` (entity-prefixed, lowercase)

Examples:
- `project_id`, `project_name`, `project_code`
- `document_id`, `document_title`, `document_projectid`

All tables include audit trail columns:
- `{entity}_isactive` - Soft delete flag
- `{entity}_createddate`, `{entity}_updateddate`, `{entity}_deleteddate`
- `{entity}_createdby`, `{entity}_updatedby`, `{entity}_deletedby`

## API Endpoints

### Projects

- `GET /api/projects` - Get all projects
- `GET /api/projects/{id}` - Get project by ID
- `POST /api/projects` - Create new project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Soft delete project

### Documents

- `GET /api/documents` - Get all documents
- `GET /api/documents/{id}` - Get document by ID
- `GET /api/documents/project/{projectId}` - Get documents by project
- `POST /api/documents` - Create new document
- `PUT /api/documents/{id}` - Update document
- `DELETE /api/documents/{id}` - Soft delete document

## Configuration

### Connection String

Update `appsettings.json` if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=nedo_documents;Username=postgres;Password=postgres;Port=5432"
  }
}
```

### CORS

CORS is configured to allow all origins in development. Update `Program.cs` for production:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://yourdomain.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

## Development Guidelines

### Adding New Entities

1. Create entity in `Domain/Entities/` implementing `IBaseEntity`
2. Add DbSet in `ApplicationDbContext`
3. Create DTOs in `Application/DTOs/{Entity}/`
4. Create repository interface and implementation
5. Create service interface and implementation
6. Create controller in `API/Controllers/`
7. Create migration: `dotnet ef migrations add Create{Entity}Table`

### Migration Naming Convention

Format: `{Timestamp}_{PascalCaseDescription}`

Examples:
- `InitialCreate` - First migration
- `CreateCategoryEntity` - Adding new entity
- `AddStatusToProject` - Adding field
- `AddProjectCodeIndex` - Adding index

## Testing

### Manual Testing via Swagger

1. Navigate to https://localhost:5001
2. Expand endpoint and click "Try it out"
3. Enter request data and execute
4. Verify response status and data

### Example: Create Project

```json
POST /api/projects
{
  "name": "Sample Project",
  "code": "PROJ001",
  "description": "Test project",
  "status": "draft"
}
```

## Logging

Logs are written to:
- **Console** - All log levels
- **File** - `logs/boilerplate-{date}.txt` (daily rotation)

Configure in `appsettings.json`:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    }
  }
}
```

## Technologies

- **ASP.NET Core 9.0** - Web framework
- **Entity Framework Core 9.0** - ORM
- **PostgreSQL** - Database
- **Npgsql** - PostgreSQL provider
- **Mapster** - Object mapping
- **Serilog** - Structured logging
- **Swashbuckle** - API documentation

## License

This boilerplate is provided as-is for use in Nedo Studio projects.

## Next Steps

- [ ] Add authentication (JWT/SSO)
- [ ] Add file upload functionality
- [ ] Add pagination support
- [ ] Add unit tests
- [ ] Add validation with FluentValidation
- [ ] Add caching layer
