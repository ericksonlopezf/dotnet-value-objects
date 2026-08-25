# Best Practices & Guidelines

> **Architectural Best Practices for Value Object Consumption**

---

## 1. Domain Modeling Best Practices

1. **Favor `readonly record struct`**: For scalar numeric, temporal, and monetary wrappers to eliminate GC allocation.
2. **Use Railway-Oriented `Result<T>`**: Validate at application boundaries (API controllers, event handlers) and pass valid Value Objects into domain logic.
3. **Use Fowler's `Allocate` for Distribution**: Never perform raw division on monetary amounts when splitting across accounts or partners.
4. **Rely on `[SensitiveData]` Masking**: Leverage automatic PII masking in logs rather than writing custom string sanitizers.
5. **Decouple Persistence**: Never introduce EF Core or database attributes into the core domain types.
