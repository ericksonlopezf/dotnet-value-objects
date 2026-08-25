# Level 06 — Roslyn Source Generators & NativeAOT Compilation

In Level 06, we generate custom value objects using `EricksonLopez.ValueObjects.Generators`.

---

## 1. Incremental Value Object Generators

```csharp
using EricksonLopez.ValueObjects;

[ValueObject]
public readonly partial struct Dimensions
{
    public double Length { get; }
    public double Width { get; }
    public double Height { get; }
}
```

The generator emits structural equality, formatters, and NativeAOT type converters at compile time.
