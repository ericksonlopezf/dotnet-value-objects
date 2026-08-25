# Migration Guide

> **Master Migration Hub for `EricksonLopez.ValueObjects`**

---

## 1. Migration Paths

- [**Migrating from NodaMoney**](https://github.com/ericksonlopezf/dotnet-value-objects/blob/main/docs/migration/from-nodamoney.md) — Converting monetary arithmetic, currency representations, and split operations.
- [**Migrating from Raw Primitives**](https://github.com/ericksonlopezf/dotnet-value-objects/blob/main/docs/migration/from-raw-primitives.md) — Eliminating Primitive Obsession across application layers and entity models.

---

## 2. General Upgrade Checklist

1. Replace public constructors with `Type.Create(...)` factory calls.
2. Check `Result.IsSuccess` before accessing `.Value`.
3. Use `ConfigureDomainValueObjects()` in EF Core `OnModelCreating`.
4. Register Dapper type handlers using `DapperValueObjectRegistry.RegisterAll()`.
