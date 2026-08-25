# Architectural Differentiation Strategy

> **Core Differentiators of `EricksonLopez.ValueObjects`**

---

## 1. Zero-Allocation Stack Foundations
Unlike libraries relying on class records or boxed structs, scalar Value Objects are pure `readonly record struct` value types. They incur **0 bytes of managed heap allocation** on hot paths.

## 2. Integrated Fowler Lossless Allocation
Built-in mathematical support for Martin Fowler's proportional allocation algorithm distributes remainder pennies without loss, solving a common financial calculation defect in e-commerce and billing systems.

## 3. Dedicated Multi-Country Fiscal Satellites
Pre-packaged, zero-dependency statutory libraries validate official tax IDs and electronic invoice structures for 6 Latin American nations (Dominican Republic, Chile, Colombia, Mexico, Peru, Argentina).

## 4. Live IDE Roslyn Diagnostic Analyzers
Three specialized Roslyn analyzers (`ELVO001`, `ELVO002`, `ELVO003`) enforce Domain-Driven Design invariants directly inside the developer's IDE with automated code fixes.

## 5. 100% NativeAOT & Trimming First
Zero reliance on dynamic reflection, runtime code emission, or un-trimmable BCL converters guarantees minimal container binary sizes and instantaneous serverless startup.
