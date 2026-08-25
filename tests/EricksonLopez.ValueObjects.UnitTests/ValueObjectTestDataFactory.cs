// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Centralized test data factory providing reliable, pre-validated Value Object instances and randomized datasets for unit and integration test suites.
/// </summary>
public static class ValueObjectTestDataFactory
{
    private static readonly Random RandomSource = new(1337_42);

    /// <summary>
    /// Creates a valid <see cref="Money"/> instance with optional parameters.
    /// </summary>
    public static Money CreateValidMoney(decimal amount = 100.50m, CurrencyCode? currency = null)
    {
        return Money.Create(amount, currency ?? CurrencyCode.USD).Value;
    }

    /// <summary>
    /// Creates a valid <see cref="Email"/> instance.
    /// </summary>
    public static Email CreateValidEmail(string user = "jane.doe", string domain = "enterprise.com")
    {
        return Email.Create($"{user}@{domain}").Value;
    }

    /// <summary>
    /// Creates a valid <see cref="PhoneNumber"/> instance in E.164 / formatted international format.
    /// </summary>
    public static PhoneNumber CreateValidPhoneNumber(string raw = "+1-809-555-0199")
    {
        return PhoneNumber.Create(raw).Value;
    }

    /// <summary>
    /// Creates a valid <see cref="Percentage"/> instance.
    /// </summary>
    public static Percentage CreateValidPercentage(decimal value = 18.0m)
    {
        return Percentage.Create(value).Value;
    }

    /// <summary>
    /// Creates a valid <see cref="Range{DateOnly}"/> representing a calendar quarter or custom dates.
    /// </summary>
    public static Range<DateOnly> CreateValidDateRange(DateOnly? start = null, DateOnly? end = null)
    {
        var s = start ?? new DateOnly(2026, 1, 1);
        var e = end ?? new DateOnly(2026, 12, 31);
        return Range<DateOnly>.Create(s, e).Value;
    }

    /// <summary>
    /// Creates a valid <see cref="TimeRange"/> instance.
    /// </summary>
    public static TimeRange CreateValidTimeRange(TimeOnly? start = null, TimeOnly? end = null, bool allowOvernight = false)
    {
        var s = start ?? new TimeOnly(9, 0);
        var e = end ?? new TimeOnly(17, 0);
        return TimeRange.Create(s, e, allowOvernight).Value;
    }

    /// <summary>
    /// Creates a valid <see cref="Address"/> instance using the fluent <see cref="AddressBuilder"/>.
    /// </summary>
    public static Address CreateValidAddress(Action<AddressBuilder>? configure = null)
    {
        var builder = new AddressBuilder();
        configure?.Invoke(builder);
        return builder.Build();
    }

    /// <summary>
    /// Creates a valid <see cref="FullName"/> instance using the fluent <see cref="FullNameBuilder"/>.
    /// </summary>
    public static FullName CreateValidFullName(Action<FullNameBuilder>? configure = null)
    {
        var builder = new FullNameBuilder();
        configure?.Invoke(builder);
        return builder.Build();
    }
}
