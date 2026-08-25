---
name: Feature Request
about: Suggest an idea or new Value Object for this framework
title: '[FEATURE] '
labels: 'enhancement'
assignees: 'ericksonlopez'
---

## Feature Summary

<!-- A clear and concise description of what feature or Value Object you are proposing. -->

## Target Package

<!-- E.g., EricksonLopez.ValueObjects, EricksonLopez.ValueObjects.Fiscal.Chile, etc. -->

## Motivation & Use Case

<!-- What problem does this solve? Is there a regulatory mandate or multi-system corporate use case? -->

## Proposed Design & Invariants

<!-- Proposed C# type signature, invariants, formatting rules, regex, or Fowler algorithm. -->

```csharp
// Example API proposal
```

## Architectural Considerations

- **Representation**: (`readonly record struct` vs `sealed record : StringValueObject<TSelf>` vs `sealed record : ValueObject`)
- **Zero-Allocation**: How does this design prevent Gen0 allocations?
- **Native AOT**: Is it free of runtime reflection?
