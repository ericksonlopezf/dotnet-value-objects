# Pull Request & Code Review Standards

> **Checklist for Maintainers & Contributors**

---

## 1. Code Review Requirements

Every PR submitted to `EricksonLopez.ValueObjects` must satisfy:

1. **Architecture & Immutability**: Structs must be `readonly record struct`. No mutable setters (`ELVO003`).
2. **Factory Validation**: Constructors must be private (`ELVO001`). `Create` factory must return `Result<T>` (`ELVO002`).
3. **Zero Allocations**: Operations on scalar types must produce 0 heap allocations.
4. **Statutory Accuracy**: Fiscal check algorithms must match official tax specifications.
5. **Quality Gates**: 0 compiler warnings, 100% tests passing, mutation score ≥ 95%.
6. **Documentation**: XML comments on all public members; kebab-case docs in `/docs/`.
