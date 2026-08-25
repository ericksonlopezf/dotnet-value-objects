# Architectural Boundaries & Segregation

> **Domain Boundary Rules, Satellite Isolation & Zero-Contamination Guarantees**

---

## 1. Domain Boundary Principles

1. **Kernel Purity**: `EricksonLopez.ValueObjects` must never reference database engines, web frameworks (ASP.NET Core), serialization libraries (System.Text.Json), or country satellites.
2. **Satellite Independence**: Country satellites (`Fiscal.DominicanRepublic`, `Fiscal.Chile`, etc.) depend only on the core library and cannot reference each other.
3. **Persistence Decoupling**: Database adapters (`EntityFrameworkCore`, `Dapper`) depend on domain models, not vice versa.
4. **Tooling Isolation**: Source generators and Roslyn analyzers execute exclusively at compile time targeting `netstandard2.0`.
