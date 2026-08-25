# Engineering Guidelines & Decision Tree

## 1. Value Object Creation Decision Tree

```
Does the domain concept have business invariants or validation rules?
├── YES → Is it used across 2 or more Bounded Contexts?
│   ├── YES → Does a suitable VO already exist in EricksonLopez.ValueObjects?
│   │   ├── YES → REUSE the existing VO ✅
│   │   └── NO  → CREATE a new corporate VO in EricksonLopez.ValueObjects ✅
│   └── NO  → Does it contain domain-specific logic (regex, checksums, algorithms)?
│       ├── YES → CREATE a local VO within the bounded context ⚠️
│       └── NO  → Evaluate if a primitive or DomainPrimitive strong-ID is sufficient 🤔
└── NO  → Is it an arbitrary string/number without business constraints?
    ├── YES → DO NOT create a Value Object. Use a primitive type ❌
```

---

## 2. Naming Standards

1. **Type Names**: Use `PascalCase` nouns without generic prefixes (`Email`, `Money`, `Nit`, `Rfc`).
2. **Factory Methods**: Exclusively use `Create(...)` or `TryCreate(...)`. Do not create non-standard factory methods like `Build()` or `From()`.
3. **File Names**: Must match the type name exactly (`Email.cs` for `Email`).
4. **Single Type per File**: Non-negotiable rule. Nested classes or multiple types in one file are forbidden.

---

## 3. Immutability & Structural Equality Rules

1. **Constructors**: All constructors must be `private` or `protected` (enforced by `ELVO001`).
2. **Properties**: Properties must be read-only (`{ get; }` or `{ get; init; }`). Setters with public mutation are prohibited (enforced by `ELVO003`).
3. **Equality**:
   - `readonly record struct` provides automatic compiler-generated value equality.
   - `sealed record : StringValueObject<TSelf>` inherits normalized string equality, `IComparable`, and case-insensitive comparison options.
   - `sealed record : ValueObject` must implement `GetEqualityComponents()`.

---

## 4. Error Code Conventions

Validation failures returned in `Result<T>.Failure(Error.Validation(...))` must follow the structured pattern:

```
[ValueObjectName].[InvariantBroken]
```

**Examples:**
- `Email.NullOrEmpty`
- `Email.InvalidFormat`
- `Money.CurrencyMismatch`
- `Percentage.OutOfRange`
- `Rnc.InvalidChecksum`
- `Rut.InvalidVerificationDigit`
- `Cuit.InvalidLength`
