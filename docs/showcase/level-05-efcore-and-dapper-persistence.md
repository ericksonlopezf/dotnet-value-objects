# Level 05: EF Core & Dapper Persistence

> **Module:** Relational Column Mappings and Micro-ORM Type Handlers  
> **Key Packages:** `EricksonLopez.ValueObjects.EntityFrameworkCore`, `EricksonLopez.ValueObjects.Dapper`

---

## 1. Entity Framework Core 10 Integration

The `ConfigureDomainValueObjects` extension maps all Value Objects to raw database primitive columns (`TEXT`, `DECIMAL`, `INTEGER`) without requiring manual converter boilerplate:

```csharp
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class Order
{
    public Guid Id { get; set; }
    public Email CustomerEmail { get; set; }
    public PhoneNumber CustomerPhone { get; set; }
    public Money Total { get; set; }
    public BusinessDate OrderDate { get; set; }
}

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Automatically registers converters for all Value Object types
        modelBuilder.ConfigureDomainValueObjects();
    }
}
```

---

## 2. Dapper Micro-ORM Integration

Register all type handlers once at application startup:

```csharp
using System.Data;
using Dapper;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Dapper;

// Application bootstrap (e.g. Program.cs)
DapperValueObjectRegistry.RegisterAll();

// Queries populate Value Objects directly
public async Task<Order?> GetAsync(IDbConnection db, Guid id)
{
    const string sql = "SELECT CustomerEmail, CustomerPhone, TotalAmount, OrderDate FROM Orders WHERE Id = @Id";
    return await db.QuerySingleOrDefaultAsync<Order>(sql, new { Id = id });
}
```
