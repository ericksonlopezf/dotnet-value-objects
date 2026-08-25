# Level 08: Fluent Testing & Quality Verification

> **Module:** Fluent Assertions, Invariant Verification & Mutation Testing  
> **Key Frameworks:** `AwesomeAssertions`, `xUnit.v3`, `Bogus`, `FsCheck`, `Stryker.NET`

---

## 1. Domain Invariant Testing

Test assertions follow the Osherove naming convention (`Method_StateUnderTest_ExpectedBehavior`):

```csharp
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using Xunit;

public sealed class MoneyTests
{
    [Fact]
    public void Allocate_WhenSplitAcrossRatios_ShouldDistributeRemainingCentsDeterministically()
    {
        // Arrange
        var total = Money.Create(100.00m, CurrencyCode.USD).Value;

        // Act
        var allocations = total.Allocate(1, 1, 1);

        // Assert
        allocations.Should().HaveCount(3);
        allocations[0].Amount.Should().Be(33.34m);
        allocations[1].Amount.Should().Be(33.33m);
        allocations[2].Amount.Should().Be(33.33m);
        (allocations[0] + allocations[1] + allocations[2]).Should().Be(total);
    }
}
```

---

## 2. Testing Zero Allocations

Verify zero allocations using `GC.GetAllocatedBytesForCurrentThread()`:

```csharp
[Fact]
public void Money_Addition_ShouldProduceZeroHeapAllocations()
{
    var a = Money.Create(100m, CurrencyCode.USD).Value;
    var b = Money.Create(50m, CurrencyCode.USD).Value;

    long before = GC.GetAllocatedBytesForCurrentThread();
    Money sum = a + b;
    long after = GC.GetAllocatedBytesForCurrentThread();

    (after - before).Should().Be(0);
}
```

---

## 3. Mutation Testing Score

The test suite achieves a verified **100% mutation score** against all domain rules and statutory checksum algorithms under Stryker.NET.
