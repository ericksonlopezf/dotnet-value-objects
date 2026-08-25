# Performance Optimization Guide

> **Guidelines for Maximum Throughput and Zero Allocations**

---

## 1. High-Throughput Guidelines

1. **Pass Structs by ReadOnly Reference**: When passing `Money` or `Range<T>` through deep call stacks, use `in` modifiers where beneficial.
2. **Use Span Parsing**: Leverage `ISpanParsable<T>` overloads (`TryParse(ReadOnlySpan<char>, ...)`) to parse from request buffers without creating intermediate strings.
3. **Pre-Allocate Result Arrays**: When splitting millions of amounts, pass pre-sized arrays to allocation methods where possible.
4. **Use Dapper TypeHandlers**: Utilize pre-registered `DapperValueObjectRegistry` type handlers to avoid reflection overhead during SQL reads.
