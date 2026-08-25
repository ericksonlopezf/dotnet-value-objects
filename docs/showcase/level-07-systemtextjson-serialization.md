# Level 07 — JSON Serialization (System.Text.Json)

In Level 07, we serialize value objects with `EricksonLopez.ValueObjects.Serialization.Json`.

---

## 1. NativeAOT JSON Serialization

`Money`, `Address`, `GeoCoordinate`, and all fiscal satellites serialize to compact JSON payloads without reflection.
