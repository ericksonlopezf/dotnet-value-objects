# Serialization & NativeAOT Design

> **High-Performance, Zero-Reflection `System.Text.Json` Converters**

---

## 1. System.Text.Json Converter Architecture

`EricksonLopez.ValueObjects.Serialization.Json` provides specialized `JsonConverter<T>` types:
- Single-value objects (`Email`, `PhoneNumber`, `Rnc`, `Rut`) serialize directly as string tokens (`"value"`).
- `Money` serializes as a composite JSON object: `{"amount": 100.50, "currency": "USD"}`.
- `Range<T>` serializes as: `{"start": ..., "end": ...}`.

---

## 2. Zero Reflection & Trim Safety

All converters use direct property access and factory methods (`Create`) rather than dynamic reflection. This guarantees 100% compatibility with NativeAOT compiler trimming (`EnableTrimAnalyzer=true`).
