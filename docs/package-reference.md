# Package Reference & Dependency Hierarchy

---

## 1. NuGet Packages

| Package | Description | Dependencies |
|---|---|---|
| [`EricksonLopez.ValueObjects`](https://nuget.org/packages/EricksonLopez.ValueObjects) | Core value objects (`Money`, `Address`, `GeoCoordinate`) | None (BCL Only) |
| [`EricksonLopez.ValueObjects.Fiscal.DominicanRepublic`](https://nuget.org/packages/EricksonLopez.ValueObjects.Fiscal.DominicanRepublic) | DR fiscal identifiers (`Rnc`, `Cedula`, `Ncf`) | `ValueObjects` |
| [`EricksonLopez.ValueObjects.Fiscal.Chile`](https://nuget.org/packages/EricksonLopez.ValueObjects.Fiscal.Chile) | Chile fiscal identifiers (`Rut`) | `ValueObjects` |
| [`EricksonLopez.ValueObjects.Fiscal.Colombia`](https://nuget.org/packages/EricksonLopez.ValueObjects.Fiscal.Colombia) | Colombia fiscal identifiers (`Nit`) | `ValueObjects` |
| [`EricksonLopez.ValueObjects.Fiscal.Mexico`](https://nuget.org/packages/EricksonLopez.ValueObjects.Fiscal.Mexico) | Mexico fiscal identifiers (`Rfc`, `Curp`) | `ValueObjects` |
| [`EricksonLopez.ValueObjects.Fiscal.Peru`](https://nuget.org/packages/EricksonLopez.ValueObjects.Fiscal.Peru) | Peru fiscal identifiers (`Ruc`, `Dni`) | `ValueObjects` |
| [`EricksonLopez.ValueObjects.Fiscal.Argentina`](https://nuget.org/packages/EricksonLopez.ValueObjects.Fiscal.Argentina) | Argentina fiscal identifiers (`Cuit`, `Cuil`) | `ValueObjects` |
| [`EricksonLopez.ValueObjects.EntityFrameworkCore`](https://nuget.org/packages/EricksonLopez.ValueObjects.EntityFrameworkCore) | EF Core complex type conversions | `ValueObjects`, `EF Core` |
| [`EricksonLopez.ValueObjects.Dapper`](https://nuget.org/packages/EricksonLopez.ValueObjects.Dapper) | Dapper type handlers | `ValueObjects`, `Dapper` |
| [`EricksonLopez.ValueObjects.Serialization.Json`](https://nuget.org/packages/EricksonLopez.ValueObjects.Serialization.Json) | System.Text.Json converters | `ValueObjects` |
| [`EricksonLopez.ValueObjects.Generators`](https://nuget.org/packages/EricksonLopez.ValueObjects.Generators) | Incremental Roslyn source generator | Roslyn 4.8 |
| [`EricksonLopez.ValueObjects.Analyzers`](https://nuget.org/packages/EricksonLopez.ValueObjects.Analyzers) | Roslyn code analyzers | Roslyn 4.8 |
| [`EricksonLopez.ValueObjects.DomainPrimitives`](https://nuget.org/packages/EricksonLopez.ValueObjects.DomainPrimitives) | Domain primitives synergy bridges | `ValueObjects` |
