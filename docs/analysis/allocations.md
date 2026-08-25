# Allocation & Memory Profile Analysis

---

## 1. Zero-Allocation Value Objects

| Value Object | Standard Class Wrapper | `EricksonLopez.ValueObjects` | Improvement |
|---|---|---|---|
| `Money` Arithmetic | 32 B | **0 B** (`readonly record struct`) | **100% Zero Allocation** |
| Fiscal Satellites (`Rnc`, `Rut`, `Nit`, `Rfc`) | 40 B | **0 B** (`readonly record struct`) | **100% Zero Allocation** |
| `GeoCoordinate` Distance Calculation | 24 B | **0 B** (`readonly record struct`) | **100% Zero Allocation** |
| JSON Serialization (STJ) | Dynamic reflection buffer | **0 B (Direct Token Write)** | **Zero Allocation** |
