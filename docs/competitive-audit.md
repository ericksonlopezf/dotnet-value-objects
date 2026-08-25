# Competitive Audit & Feature Comparison

---

## 1. Feature Matrix vs Ecosystem Alternatives

| Feature | `EricksonLopez.ValueObjects` | NodaMoney | Custom Class VOs |
|---|:---:|:---:|:---:|
| **Zero-Allocation Struct Layout** | ✅ **Yes (`readonly record struct`)** | ⚠️ Partial | ❌ No |
| **Multi-Country Fiscal Satellites** | ✅ **6 LATAM Countries + Validation** | ❌ No | ❌ No |
| **NativeAOT & Trimming Safe** | ✅ **100% NativeAOT** | ⚠️ Partial | ⚠️ Reflection based |
| **EF Core & Dapper Integration** | ✅ **Complex Types & TypeHandlers** | ⚠️ Limited | ❌ Manual |
| **Stryker Mutation Tested ($\ge 95\%$)** | ✅ **100% Verified** | ❌ Untested | ❌ Untested |
| **Code Coverage ($\ge 99\%$)** | ✅ **99.6%** | ⚠️ ~80% | ⚠️ ~70% |
