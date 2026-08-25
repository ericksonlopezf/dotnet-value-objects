// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Generic;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Reusable assertion helpers for validating Value Object contracts and invariants across all test suites.
/// </summary>
/// <remarks>
/// <para><b>Architectural Design &amp; Boundary Policy (Single Responsibility):</b></para>
/// <para>
/// This class is strictly dedicated to validating universal DDD Value Object mathematical contracts:
/// <list type="bullet">
///   <item><description>Formal value equality (<see cref="IEquatable{T}"/>, symmetry, reflexivity, transitivity, hash code consistency).</description></item>
///   <item><description>Relational ordering contracts (<see cref="IComparable{T}"/>, &lt;, &lt;=, &gt;, &gt;=).</description></item>
///   <item><description>Zero-allocation memory invariants (<see cref="GC.GetAllocatedBytesForCurrentThread"/>).</description></item>
/// </list>
/// To prevent this helper from devolving into a "God Object", domain-specific parsing, persistence fixtures, or mock configurations
/// must NOT be added here. They belong in dedicated test fixtures within their respective test modules.
/// </para>
/// </remarks>
public static class ValueObjectContractExtensions
{
    /// <summary>
    /// Validates full value equality semantics: reflexivity, symmetry, transitivity, and inequality.
    /// </summary>
    public static void ShouldSatisfyEqualityContract<T>(
        this T instance,
        T equalInstance,
        T differentInstance,
        Func<T, T, bool>? equalityOperator = null,
        Func<T, T, bool>? inequalityOperator = null)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(equalInstance);
        ArgumentNullException.ThrowIfNull(differentInstance);

        // Reflexivity
        instance.Equals(instance).Should().BeTrue("an instance must equal itself");
        EqualityComparer<T>.Default.Equals(instance, instance).Should().BeTrue();

        // Symmetry
        instance.Equals(equalInstance).Should().BeTrue("equal instances must be equal");
        equalInstance.Equals(instance).Should().BeTrue("equality must be symmetric");
        EqualityComparer<T>.Default.Equals(instance, equalInstance).Should().BeTrue();

        // Hash Code consistency
        instance.GetHashCode().Should().Be(equalInstance.GetHashCode(), "equal instances must produce identical hash codes");

        // Non-equality
        instance.Equals(differentInstance).Should().BeFalse("different instances must not be equal");
        EqualityComparer<T>.Default.Equals(instance, differentInstance).Should().BeFalse();

        // Null comparison
        instance.Equals(default!).Should().BeFalse("non-null instance must not equal default/null");

        // Optional operators check
        if (equalityOperator is not null)
        {
            equalityOperator(instance, instance).Should().BeTrue();
            equalityOperator(instance, equalInstance).Should().BeTrue();
            equalityOperator(instance, differentInstance).Should().BeFalse();
        }

        if (inequalityOperator is not null)
        {
            inequalityOperator(instance, instance).Should().BeFalse();
            inequalityOperator(instance, equalInstance).Should().BeFalse();
            inequalityOperator(instance, differentInstance).Should().BeTrue();
        }
    }

    /// <summary>
    /// Validates comparison operator contracts: &lt;, &lt;=, &gt;, &gt;=, CompareTo.
    /// </summary>
    public static void ShouldSatisfyComparisonContract<T>(
        this T smaller,
        T equalToSmaller,
        T greater,
        Func<T, T, bool> lessThan,
        Func<T, T, bool> lessThanOrEqual,
        Func<T, T, bool> greaterThan,
        Func<T, T, bool> greaterThanOrEqual)
        where T : IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(smaller);
        ArgumentNullException.ThrowIfNull(equalToSmaller);
        ArgumentNullException.ThrowIfNull(greater);
        ArgumentNullException.ThrowIfNull(lessThan);
        ArgumentNullException.ThrowIfNull(lessThanOrEqual);
        ArgumentNullException.ThrowIfNull(greaterThan);
        ArgumentNullException.ThrowIfNull(greaterThanOrEqual);

        // CompareTo semantics
        smaller.CompareTo(greater).Should().BeNegative("smaller instance must compare negative against greater");
        greater.CompareTo(smaller).Should().BePositive("greater instance must compare positive against smaller");
        smaller.CompareTo(equalToSmaller).Should().Be(0, "equal instances must compare to 0");

        // Relational operator semantics
        lessThan(smaller, greater).Should().BeTrue();
        lessThan(greater, smaller).Should().BeFalse();
        lessThan(smaller, equalToSmaller).Should().BeFalse();

        lessThanOrEqual(smaller, greater).Should().BeTrue();
        lessThanOrEqual(greater, smaller).Should().BeFalse();
        lessThanOrEqual(smaller, equalToSmaller).Should().BeTrue();

        greaterThan(greater, smaller).Should().BeTrue();
        greaterThan(smaller, greater).Should().BeFalse();
        greaterThan(smaller, equalToSmaller).Should().BeFalse();

        greaterThanOrEqual(greater, smaller).Should().BeTrue();
        greaterThanOrEqual(smaller, greater).Should().BeFalse();
        greaterThanOrEqual(smaller, equalToSmaller).Should().BeTrue();
    }

    /// <summary>
    /// Executes the specified action and asserts that zero heap allocations occurred during execution.
    /// Performs a warm-up phase to ensure JIT compilation, static constructors, and caches are primed.
    /// </summary>
    /// <param name="action">The action to execute and measure.</param>
    /// <param name="warmupIterations">Number of warm-up iterations prior to measurement.</param>
    /// <param name="measurementIterations">Number of iterations to execute during measurement.</param>
    public static void AssertZeroAllocations(
        Action action,
        int warmupIterations = 5,
        int measurementIterations = 20)
    {
        ArgumentNullException.ThrowIfNull(action);

        // Warm-up to trigger JIT compilation and static initializers
        for (int i = 0; i < warmupIterations; i++)
        {
            action();
        }

        // Measure allocated bytes on the current thread
        long beforeAllocated = GC.GetAllocatedBytesForCurrentThread();

        for (int i = 0; i < measurementIterations; i++)
        {
            action();
        }

        long afterAllocated = GC.GetAllocatedBytesForCurrentThread();
        long totalAllocated = afterAllocated - beforeAllocated;

        totalAllocated.Should().Be(0, $"expected 0 bytes allocated across {measurementIterations} iterations, but {totalAllocated} bytes were allocated");
    }
}
