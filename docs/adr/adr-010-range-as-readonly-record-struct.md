# ADR-010: Range<T> as Zero-Allocation readonly record struct

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Core Abstractions & Mathematical Interval Types

## Context and Problem Statement

Implementing continuous intervals `Range<T>` as a reference class (`sealed record`) forces heap allocations for every temporal check or interval calculation.

## Decision

1. **Convert `Range<T>` to Struct:** Change `Range<T>` to `public readonly record struct Range<T>` where `T : IComparable<T>`.
2. **Value Equality & Interval Logic:** Provide instance methods for `Contains`, `Overlaps`, and `Intersects`.

## Consequences

- **Positive:** Zero heap allocations during interval intersections, range checks, and high-frequency scheduling logic.
- **Negative:** `Range<T>` is a value type and cannot be null unless wrapped in `Nullable<Range<T>>`.
