# Level 05 — EF Core & Dapper Relational Persistence

In Level 05, we persist value objects using `EricksonLopez.ValueObjects.EntityFrameworkCore` and `EricksonLopez.ValueObjects.Dapper`.

---

## 1. EF Core Complex Type & Value Converters

```csharp
using Microsoft.EntityFrameworkCore;
using EricksonLopez.ValueObjects.EntityFrameworkCore;

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Invoice>()
        .ComplexProperty(i => i.Total); // Money mapped to Amount + Currency columns
}
```
