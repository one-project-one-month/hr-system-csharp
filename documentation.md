# Database Scaffolding Commands

Use the following EF Core scaffold commands to generate database contexts and entities.

## 📁 Run from Directory
All scaffold commands should be executed from:
```
hr-system-csharp\HRSystem.Csharp.Database>
```

## 🗄️ SQL Server Scaffolding

### Localhost MSSQL
```
dotnet ef dbcontext scaffold "Server=.;Database=HRSystem;User ID=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o AppDbContextModels -c AppDbContext -f
```