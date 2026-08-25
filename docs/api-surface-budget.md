# API Surface & Memory Budget

> **Budget Policy:** Strict constraints on type size, public members, and memory allocations.

---

## 1. Struct Memory Footprint Budget

| Type | Maximum Size | Alignment | GC Allocation Target |
|---|---|---|---|
| `CurrencyCode` | 8 bytes | 8-byte aligned | **0 B** |
| `Money` | 24 bytes (16B decimal + 8B ref) | 8-byte aligned | **0 B** |
| `Range<T>` | 2 * sizeof(T) | Native struct alignment | **0 B** |
| `BusinessDate` | 4 bytes (`DateOnly`) | 4-byte aligned | **0 B** |
| `Percentage` | 16 bytes (`decimal`) | 8-byte aligned | **0 B** |
| `TaxRate` | 16 bytes (`decimal`) | 8-byte aligned | **0 B** |

---

## 2. Public Member Count Budget

To maintain clean IntelliSense and prevent cognitive overload, individual Value Object types enforce a budget of:
- **Maximum 25 public members** per concrete struct.
- Single responsibility: parsing, formatting, domain invariants, and mathematical operations.

---

## 3. NativeAOT & Trimming Budgets

- **IL2026 Warnings (RequiresUnreferencedCode):** 0 permitted (`TreatWarningsAsErrors=true`).
- **IL3050 Warnings (RequiresDynamicCode):** 0 permitted (`TreatWarningsAsErrors=true`).
- **Startup Latency:** < 5 ms in published NativeAOT container images.
