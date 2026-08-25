# Best Practices & Production Guidelines

---

## 1. Value Object Modeling Rules

1. **Prefer Readonly Record Structs**: Leverage value semantics, structural equality, and zero heap allocation.
2. **Encapsulate Domain Arithmetic**: Provide operators (`+`, `-`, `*`) that preserve precision and enforce currency/unit matching.
3. **Use Fiscal Satellites**: Separate localized fiscal identifiers (`Rnc`, `Rut`, `Nit`, `Rfc`) into dedicated satellite packages to keep core domains lightweight.
