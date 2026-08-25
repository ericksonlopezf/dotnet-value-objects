# Reusability Matrix

> **Cross-Layer Reusability & Dependency Constraints**

---

## 1. Reusability Matrix

| Layer | Depends On | Permitted In | Prohibited In |
|---|---|---|---|
| **Domain Kernel** | `EricksonLopez.Result` | Domain, Application, Infrastructure | Must not reference persistence or framework packages |
| **Fiscal Satellites** | Domain Kernel | Domain, Application, Infrastructure | Cross-satellite references |
| **Persistence (EF Core / Dapper)** | Domain Kernel, ORM | Infrastructure | Domain entities / pure logic |
| **Analyzers / Generators** | Roslyn APIs | Build / Compile-Time | Runtime execution |
