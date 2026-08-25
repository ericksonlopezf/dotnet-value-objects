# RFC-0004: Decoupled Persistence Converters for EF Core 10 and Dapper

> **Status:** Approved  
> **Authors:** Erickson Lopez (<ericksonlopezf@gmail.com>)  
> **Created:** 2026-08-23  
> **Target Release:** v1.0.0  

---

## 1. Summary

This RFC defines the persistence adapter strategy: separating Entity Framework Core 10 `ValueConverter` mappings and Dapper `SqlMapper.TypeHandler` registrations into independent packages (`EricksonLopez.ValueObjects.EntityFrameworkCore` and `EricksonLopez.ValueObjects.Dapper`), keeping domain kernel assemblies 100% free from ORM dependencies.

---

## 2. Motivation

In Clean Architecture and Domain-Driven Design, the Domain layer must remain pure and free from infrastructure or persistence dependencies. Packaging persistence adapters alongside domain models violates Clean Architecture boundaries and inflates deployment footprint for non-EF applications.

---

## 3. Specification

1. **EF Core 10**: Exposes `modelBuilder.ConfigureDomainValueObjects()` that scans and automatically registers `ValueConverter<TModel, TProvider>` for all domain primitives without runtime reflection.
2. **Dapper**: Exposes `DapperValueObjectRegistry.RegisterAll()` that registers strongly typed `SqlMapper.TypeHandler<T>` instances for all core and fiscal types.

---

## 4. Decision

Approved and verified with automated integration tests against SQLite and relational in-memory providers.
