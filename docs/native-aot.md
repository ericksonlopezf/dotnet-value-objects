# Native AOT & Trimming Specification

`EricksonLopez.ValueObjects` is engineered for the **Native AOT** and trimming era in .NET 10+.

---

## 1. Core Guarantees

1. `<IsAotCompatible>true</IsAotCompatible>` enabled across all core domain, fiscal, and persistence libraries (configured in `Directory.Build.props`).
2. `<IsTrimmable>true</IsTrimmable>` enabled across all libraries.
3. Zero runtime reflection in core validation pipelines and constructors.
4. 100% NativeAOT smoke test pass rate (`EricksonLopez.ValueObjects.NativeAotTests` — 31 tests).

---

## 2. Zero Dynamic Reflection

All runtime reflection has been eliminated from core paths:

- Value Object creation and normalization use direct static factory calls and compiled delegates.
- Sensitive data masking is declared statically or via virtual property evaluation in `SingleValueObject<TSelf, TValue>`.
- `System.Text.Json` converters use explicit type mappings or compile-time Source Generators (no `MakeGenericType`, no `TypeDescriptor`, no dynamic IL).

### Prohibited Patterns

| Pattern | Status | Reason |
|---|:---:|---|
| `Activator.CreateInstance<T>()` | ❌ Prohibited | Dynamic IL; incompatible with NativeAOT |
| `typeof(T).GetProperties()` | ❌ Prohibited | Reflection; trimmer removes unused members |
| `MakeGenericType(...)` | ❌ Prohibited | Dynamic IL |
| `Newtonsoft.Json` | ❌ Prohibited | Dynamic reflection-based serialization |
| `System.Text.Json` with AOT-safe converters | ✅ Allowed | Explicit type mappings; source generator compatible |
| `static readonly` compiled expressions | ✅ Allowed | Evaluated at build time, not runtime |

---

## 3. Serialization Rules

- Exclusively uses `System.Text.Json` NativeAOT-compatible converters from `EricksonLopez.ValueObjects.Serialization.Json`.
- `Newtonsoft.Json` is prohibited by architectural decision (ADR-004) to ensure zero dynamic IL generation and trimming safety.
- All `JsonConverter<T>` implementations use generic type constraints resolved at compile-time.

---

## 4. Running NativeAOT Smoke Tests

```bash
dotnet run --project tests/EricksonLopez.ValueObjects.NativeAotTests/EricksonLopez.ValueObjects.NativeAotTests.csproj --configuration Release
```

The smoke test project publishes the test binary as a NativeAOT executable and verifies all 31 representative value object scenarios complete without `TypeLoadException`, `MissingMethodException`, or trimmer warnings.

---

## 5. Compatibility Matrix

| Package | `IsAotCompatible` | `IsTrimmable` | Dynamic Reflection |
|---|:---:|:---:|:---:|
| `EricksonLopez.ValueObjects` | ✅ `true` | ✅ `true` | **Zero (0)** |
| `EricksonLopez.ValueObjects.DomainPrimitives` | ✅ `true` | ✅ `true` | **Zero (0)** |
| `EricksonLopez.ValueObjects.Serialization.Json` | ✅ `true` | ✅ `true` | **Zero (0)** |
| `EricksonLopez.ValueObjects.Dapper` | ✅ `true` | ✅ `true` | **Zero (0)** |
| `EricksonLopez.ValueObjects.EntityFrameworkCore` | ✅ `true` | ✅ `true` | **Zero (0)** |
| `EricksonLopez.ValueObjects.Fiscal.*` (All 6) | ✅ `true` | ✅ `true` | **Zero (0)** |
| `EricksonLopez.ValueObjects.Analyzers` | N/A (Build-only) | N/A | N/A |
| `EricksonLopez.ValueObjects.Generators` | N/A (Build-only) | N/A | N/A |
