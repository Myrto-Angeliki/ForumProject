# ForumProject

A forum application backend built with .NET 10, following Clean Architecture principles and exposing a REST API. The solution separates concerns across Domain, Application, Infrastructure, and API layers.

## Project Structure

The solution is organized into the following projects:

```
ForumProject/
├── src/
│   ├── ForumProject.Api             # Presentation layer — REST endpoints/controllers, startup configuration
│   ├── ForumProject.Application     # Application layer — use cases, business logic, DTOs, interfaces
│   ├── ForumProject.Domain          # Domain layer — core entities, enums, domain logic
│   ├── ForumProject.Infrastructure  # Infrastructure layer — data access via Dapper and stored procedures, external services
│   └── ForumProject.Seeder          # Utility project for seeding the database with initial/sample data
├── tests/
│   └── ForumProject.Tests           # Automated tests for the solution
└── ForumProject.slnx                # Solution file
```

This layout follows the standard Clean Architecture dependency flow:

```
Api  ->  Application  ->  Domain
              ^
       Infrastructure
```

- Domain has no dependencies on other layers — it contains the core business entities and rules.
- Application depends only on Domain and defines interfaces implemented by Infrastructure.
- Infrastructure implements Application's interfaces (e.g. persistence via SQL Server using Dapper and stored procedures, external integrations).
- Api wires everything together and exposes the application as a REST API.


## Backend Documentation
| Endpoint           | Method  | Action            |
| --------           | :------ | ----------------- |
| /api/auth/register | POST    | Create a new user |
| /api/auth/login    | POST    | Login an existing user |
| /api/auth/password | PUT     | Change the password of an authorized user |

## Prerequisites

- .NET 10 SDK
- SQL Server (local instance, Docker container, or remote)

## Getting Started

1. Clone the repository
   ```bash
   git clone https://github.com/Myrto-Angeliki/ForumProject.git
   cd ForumProject
   ```

2. Restore dependencies
   ```bash
   dotnet restore ForumProject.slnx
   ```

3. Configure application settings

   Update the SQL Server connection string and any other required settings in `src/ForumProject.Api/appsettings.json`.

4. Set up the database

   Run `InitialSchema.sql` against your SQL Server database to create the schema, then execute the stored procedure scripts found in `src/ForumProject.Infrastructure/Database/StoredProcedures`.

5. Seed the database (optional)

   The seeder reads its configuration from a hardcoded path to `appsettings.json`. Before running it, open `src/ForumProject.Seeder` and update the path passed to `AddJsonFile(...)` to point to your local copy of `src/ForumProject.Api/appsettings.json`:
   ```csharp
   IConfiguration config = new ConfigurationBuilder()
       .AddJsonFile("C:\\path\\to\\your\\ForumProject\\src\\ForumProject.Api\\appsettings.json")
       .Build();
   ```
   Then run:
   ```bash
   dotnet run --project src/ForumProject.Seeder
   ```

6. Run the API
   ```bash
   dotnet run --project src/ForumProject.Api
   ```

## Running Tests

```bash
dotnet test tests/ForumProject.Tests
```

## Built With

- .NET 10 / ASP.NET Core Web API
- SQL Server (accessed via Dapper, with queries executed through stored procedures)
- Clean Architecture — solution layout and dependency management

## License

MIT License

## Contributing

Contributions, issues, and feature requests are welcome. Feel free to open an issue or submit a pull request.

---

Note: This project is a work in progress and has not been fully tested. Some aspects of the setup or functionality may not work as expected — issues and corrections are welcome.



