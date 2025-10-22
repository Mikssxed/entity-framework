# Entity Framework Project

This project demonstrates Entity Framework Core usage with performance benchmarking using BenchmarkDotNet.

## Project Structure

- `entityframework/` - Main ASP.NET Core Web API project
- `MyBoardsBenchmark/` - BenchmarkDotNet performance testing project

## Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio Code with C# Dev Kit extension (recommended)

## Setup

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd entityframework
   ```

2. **Configure database connection**

   Update the connection string in `entityframework/appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "MyBoardsConnectionString": "Server=(localdb)\\mssqllocaldb;Database=MyBoardsDb;Trusted_Connection=true;"
     }
   }
   ```

3. **Run database migrations**
   ```bash
   cd entityframework
   dotnet ef database update
   ```

## Running the Main Application

1. **Navigate to the main project**

   ```bash
   cd entityframework
   ```

2. **Run the application**

   ```bash
   dotnet run
   ```

3. **Access the API**
   - Swagger UI: `https://localhost:5001/swagger`
   - API endpoints:
     - GET `/data` - Retrieve data
     - POST `/update` - Update an epic
     - POST `/create` - Create a new user

## Running Benchmarks

1. **Navigate to the benchmark project**

   ```bash
   cd MyBoardsBenchmark
   ```

2. **Run benchmarks in Release mode** (important for accurate results)

   ```bash
   dotnet run -c Release
   ```

3. **View results**
   - Results are displayed in the terminal
   - Detailed reports are saved in `BenchmarkDotNet.Artifacts/results/`
   - Available formats: HTML, CSV, and GitHub Markdown

## Benchmark Options

```bash
# Run with specific .NET framework
dotnet run -c Release -f net9.0

# Filter specific benchmark methods
dotnet run -c Release -- --filter "*MethodName*"

# Specify custom artifacts directory
dotnet run -c Release -- --artifacts "C:\CustomPath\Results"

# Run with memory diagnoser
dotnet run -c Release -- --memory
```

## Development Commands

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test

# Add new migration
dotnet ef migrations add MigrationName --project entityframework

# Update database
dotnet ef database update --project entityframework
```

## Troubleshooting

### JSON Circular Reference Error

If you encounter circular reference errors when calling API endpoints, ensure the JSON serialization is configured to handle cycles:

```csharp
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
```

### Database Connection Issues

- Verify SQL Server is running
- Check connection string format
- Ensure database exists or run migrations

### Benchmark Accuracy

- Always run benchmarks in Release mode (`-c Release`)
- Close unnecessary applications during benchmarking
- Run on a stable system with consistent performance

## Entity Relationships

The project demonstrates various Entity Framework relationships:

- **User** ↔ **Address** (One-to-One)
- **User** → **WorkItems** (One-to-Many)
- **User** → **Comments** (One-to-Many)
- **WorkItem** ↔ **Tags** (Many-to-Many via WorkItemTag)
- **WorkItem** → **State** (Many-to-One)
