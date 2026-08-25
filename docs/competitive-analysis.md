# Competitive Analysis

> **Market Evaluation vs Alternative Value Object & Money Solutions in .NET**

---

## 1. Feature Matrix vs Alternatives

| Capability | EricksonLopez.ValueObjects | NodaMoney | ValueOf | StronglyTypedId |
|---|:---:|:---:|:---:|:---:|
| **Zero Heap Allocations on Arithmetic** | ✅ **0 Bytes** | ⚠️ Partial | ❌ Heap Allocated | ❌ Heap Allocated |
| **Martin Fowler Lossless Allocation** | ✅ Built-in | ❌ Basic Split | ❌ None | ❌ None |
| **6 Official LATAM Fiscal Satellites** | ✅ Dedicated | ❌ None | ❌ None | ❌ None |
| **Compile-Time Roslyn Analyzers** | ✅ Rules `ELVO001`–`ELVO003` | ❌ None | ❌ None | ❌ None |
| **Pre-Built EF Core 10 Auto-Mapping** | ✅ `ConfigureDomainValueObjects` | ⚠️ Custom | ⚠️ Custom | ⚠️ Custom |
| **100% NativeAOT / Trimming Ready** | ✅ Zero Reflection | ⚠️ Partial | ❌ Reflection | ⚠️ Partial |
| **Railway-Oriented `Result<T>` Flow** | ✅ Built-in | ❌ Exceptions | ❌ Exceptions | ❌ Exceptions |
