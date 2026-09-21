// Copyright © Erickson Lopez. MIT License.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a monetary amount denominated in a specific ISO 4217 currency.
/// </summary>
/// <remarks>
/// Enforces same-currency arithmetic and canonical rounding at domain boundaries.
/// <para>
/// <b>Caution on Value Type Defaults:</b> As a value type (<c>readonly record struct</c>), C# permits
/// uninitialized instances via <c>default(Money)</c>. In an uninitialized instance, <see cref="Currency"/>
/// is unassigned, which violates domain invariants. Always create instances via <see cref="Create(decimal, CurrencyCode, int?)"/>,
/// <see cref="Zero(CurrencyCode)"/>, or designated domain factories.
/// </para>
/// </remarks>
public readonly record struct Money : IValueObject<Money>, IComparable<Money>, IComparable, IFormattable, ISpanFormattable, IParsable<Money>, ISpanParsable<Money>
{
    private const decimal MaxAbsoluteAmount = 999_999_999_999_999.999999m;

    /// <summary>
    /// Gets the numerical monetary amount.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Gets the ISO 4217 currency code.
    /// </summary>
    public CurrencyCode Currency { get; }

    /// <summary>
    /// Gets a value indicating whether this <see cref="Money"/> instance was initialized through a factory or constructor rather than <c>default(Money)</c>.
    /// </summary>
    public bool IsInitialized => Currency.IsInitialized;

    /// <summary>
    /// Gets a zero-value <see cref="Money"/> instance denominated in US Dollars (USD).
    /// </summary>
    public static readonly Money ZeroUsd = new(0m, CurrencyCode.USD);

    internal Money(decimal amount, CurrencyCode currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Creates a <see cref="Money"/> instance rounded to the specified decimal places using banker's rounding.
    /// </summary>
    /// <param name="amount">The numeric monetary amount.</param>
    /// <param name="currency">The currency code value object.</param>
    /// <param name="decimals">The optional number of decimal places to round to. If omitted, uses the currency's standard decimal places.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the created monetary value, or a validation failure.</returns>
    public static Result<Money> Create(decimal amount, CurrencyCode currency, int? decimals = null)
    {
        if (!currency.IsInitialized)
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.InvalidCurrency", "Currency code must be initialized."));
        }

        if (Math.Abs(amount) > MaxAbsoluteAmount)
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.AmountOutOfRange", "Money amount is outside the supported range."));
        }

        if (!NumericValidation.IsScaleAtMost(amount, 6))
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.TooManyDecimals", "Money amount supports at most 6 decimal places."));
        }

        var rounded = Math.Round(amount, decimals ?? currency.DecimalPlaces, MidpointRounding.ToEven);
        return Result<Money>.Success(new Money(rounded, currency));
    }

    /// <summary>
    /// Creates a <see cref="Money"/> instance using a string currency code.
    /// </summary>
    /// <param name="amount">The numeric monetary amount.</param>
    /// <param name="currency">The ISO 4217 three-letter currency string.</param>
    /// <param name="decimals">The optional number of decimal places to round to.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the created monetary value, or a validation failure.</returns>
    public static Result<Money> Create(decimal amount, string currency, int? decimals = null)
    {
        var currencyResult = CurrencyCode.Create(currency);
        if (currencyResult.IsFailure)
        {
            return Result<Money>.Failure(currencyResult.Error);
        }

        return Create(amount, currencyResult.Value, decimals);
    }

    /// <summary>
    /// Creates a <see cref="Money"/> instance that requires a non-negative amount (greater than or equal to zero).
    /// </summary>
    /// <param name="amount">The non-negative numeric amount.</param>
    /// <param name="currency">The currency code value object.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the created monetary value, or a validation failure.</returns>
    public static Result<Money> CreateNonNegative(decimal amount, CurrencyCode currency)
    {
        if (amount < 0m)
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.NegativeAmount", "Money amount cannot be negative."));
        }

        return Create(amount, currency);
    }

    /// <summary>
    /// Creates a zero-value <see cref="Money"/> instance in the specified currency.
    /// </summary>
    /// <param name="currency">The target currency code.</param>
    /// <returns>A zero-value monetary amount in the specified currency.</returns>
    public static Money Zero(CurrencyCode currency)
    {
        if (!currency.IsInitialized)
        {
            throw new DomainException("Cannot create Money with uninitialized currency.");
        }

        return new(0m, currency);
    }

    /// <summary>
    /// Adds another monetary amount to this instance, ensuring both amounts share the same currency.
    /// </summary>
    /// <param name="other">The monetary amount to add.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the sum, or a currency mismatch error.</returns>
    public Result<Money> Add(Money other)
    {
        if (string.IsNullOrEmpty(Currency.Value) || string.IsNullOrEmpty(other.Currency.Value))
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.UninitializedCurrency", "Cannot operate on Money with uninitialized currency."));
        }

        if (Currency != other.Currency)
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.CurrencyMismatch", $"Cannot add '{other.Currency}' to '{Currency}'."));
        }

        decimal newAmount = Amount + other.Amount;
        if (Math.Abs(newAmount) > MaxAbsoluteAmount)
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.AmountOutOfRange", "Money amount is outside the supported range."));
        }

        return new Money(newAmount, Currency);
    }

    /// <summary>
    /// Subtracts another monetary amount from this instance, ensuring both amounts share the same currency.
    /// </summary>
    /// <param name="other">The monetary amount to subtract.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the difference, or a currency mismatch error.</returns>
    public Result<Money> Subtract(Money other)
    {
        if (string.IsNullOrEmpty(Currency.Value) || string.IsNullOrEmpty(other.Currency.Value))
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.UninitializedCurrency", "Cannot operate on Money with uninitialized currency."));
        }

        if (Currency != other.Currency)
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.CurrencyMismatch", $"Cannot subtract '{other.Currency}' from '{Currency}'."));
        }

        decimal newAmount = Amount - other.Amount;
        if (Math.Abs(newAmount) > MaxAbsoluteAmount)
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.AmountOutOfRange", "Money amount is outside the supported range."));
        }

        return new Money(newAmount, Currency);
    }

    /// <summary>
    /// Attempts to multiply the monetary amount by a scalar factor using banker's rounding without throwing exceptions.
    /// </summary>
    /// <param name="factor">The multiplier factor.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the product, or a validation error if out of range or uninitialized.</returns>
    public Result<Money> TryMultiply(decimal factor)
    {
        if (!Currency.IsInitialized)
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.UninitializedCurrency", "Cannot operate on Money with uninitialized currency."));
        }

        try
        {
            decimal newAmount = Math.Round(Amount * factor, Currency.DecimalPlaces, MidpointRounding.ToEven);
            if (Math.Abs(newAmount) > MaxAbsoluteAmount)
            {
                return Result<Money>.Failure(Error.Validation(
                    "Money.AmountOutOfRange", "Money amount is outside the supported range."));
            }

            return new Money(newAmount, Currency);
        }
        catch (OverflowException)
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.AmountOutOfRange", "Money amount is outside the supported range."));
        }
    }

    /// <summary>
    /// Multiplies the monetary amount by a scalar factor using banker's rounding.
    /// </summary>
    /// <param name="factor">The multiplier factor.</param>
    /// <returns>The product rounded to the currency's standard decimal places.</returns>
    /// <exception cref="DomainException">Currency is uninitialized or amount is outside supported range.</exception>
    public Money Multiply(decimal factor)
    {
        var result = TryMultiply(factor);
        if (result.IsFailure)
        {
            throw new DomainException(result.Error.Description);
        }

        return result.Value;
    }

    /// <summary>
    /// Attempts to apply a percentage to this monetary amount using banker's rounding without throwing exceptions.
    /// </summary>
    /// <param name="percentage">The percentage to apply.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the calculated share, or a validation error.</returns>
    public Result<Money> TryApplyPercentage(Percentage percentage)
    {
        if (!Currency.IsInitialized)
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.UninitializedCurrency", "Cannot operate on Money with uninitialized currency."));
        }

        try
        {
            decimal newAmount = Math.Round(Amount * percentage.Fraction, Currency.DecimalPlaces, MidpointRounding.ToEven);
            if (Math.Abs(newAmount) > MaxAbsoluteAmount)
            {
                return Result<Money>.Failure(Error.Validation(
                    "Money.AmountOutOfRange", "Money amount is outside the supported range."));
            }

            return new Money(newAmount, Currency);
        }
        catch (OverflowException)
        {
            return Result<Money>.Failure(Error.Validation(
                "Money.AmountOutOfRange", "Money amount is outside the supported range."));
        }
    }

    /// <summary>
    /// Applies a percentage to this monetary amount using banker's rounding.
    /// </summary>
    /// <param name="percentage">The percentage to apply.</param>
    /// <returns>The calculated monetary share.</returns>
    /// <exception cref="DomainException">Currency is uninitialized or amount is outside supported range.</exception>
    public Money ApplyPercentage(Percentage percentage)
    {
        var result = TryApplyPercentage(percentage);
        if (result.IsFailure)
        {
            throw new DomainException(result.Error.Description);
        }

        return result.Value;
    }

    /// <summary>
    /// Returns the negated monetary amount.
    /// </summary>
    /// <returns>A new <see cref="Money"/> instance with the negated amount.</returns>
    /// <exception cref="DomainException">Currency is uninitialized.</exception>
    public Money Negate()
    {
        if (!Currency.IsInitialized)
        {
            throw new DomainException("Cannot operate on Money with uninitialized currency.");
        }

        return new(-Amount, Currency);
    }

    /// <summary>
    /// Returns the absolute value of this monetary amount.
    /// </summary>
    /// <returns>A new <see cref="Money"/> instance with a non-negative amount.</returns>
    /// <exception cref="DomainException">Currency is uninitialized.</exception>
    public Money Abs()
    {
        if (!Currency.IsInitialized)
        {
            throw new DomainException("Cannot operate on Money with uninitialized currency.");
        }

        return new(Math.Abs(Amount), Currency);
    }

    /// <summary>
    /// Allocates the monetary amount proportionally across the specified integer ratios without fractional currency loss.
    /// </summary>
    /// <remarks>
    /// <para>Implements Martin Fowler's proportional allocation algorithm. Each share is truncated to the
    /// currency's standard decimal precision (as defined by <see cref="CurrencyCode.DecimalPlaces"/>), and any
    /// indivisible remainder is distributed one minimum unit at a time to the earliest shares.</para>
    /// <para>Returns an empty array when <paramref name="ratios"/> is empty.</para>
    /// <para>The minimum distribution unit respects the currency's decimal precision:
    /// 1 for zero-decimal currencies (e.g. JPY), 0.01 for two-decimal currencies (e.g. USD),
    /// 0.001 for three-decimal currencies (e.g. KWD).</para>
    /// </remarks>
    /// <param name="ratios">The positive integer ratios used to divide the amount.</param>
    /// <returns>
    /// An array of <see cref="Money"/> instances representing each allocated portion, one per ratio.
    /// Returns an empty array if <paramref name="ratios"/> is empty.
    /// </returns>
    /// <exception cref="DomainException">Currency is uninitialized.</exception>
    /// <exception cref="ArgumentException"><paramref name="ratios"/> contains a value less than or equal to zero</exception>
    public Money[] Allocate(params ReadOnlySpan<int> ratios)
    {
        if (!Currency.IsInitialized)
        {
            throw new DomainException("Cannot operate on Money with uninitialized currency.");
        }

        if (ratios.Length == 0)
        {
            return [];
        }

        int totalRatio = 0;
        for (int i = 0; i < ratios.Length; i++)
        {
            if (ratios[i] <= 0)
            {
                throw new ArgumentException("Ratios must be strictly positive.", nameof(ratios));
            }
            totalRatio += ratios[i];
        }

        int decimalPlaces = Currency.DecimalPlaces;
        decimal scaleFactor = (decimal)Math.Pow(10, decimalPlaces);
        decimal minUnit = scaleFactor == 0m ? 1m : 1m / scaleFactor;

        var results = new Money[ratios.Length];
        decimal remainder = Amount;

        for (int i = 0; i < ratios.Length; i++)
        {
            decimal share = Math.Truncate(Amount * ratios[i] / totalRatio * scaleFactor) / scaleFactor;
            results[i] = new Money(share, Currency);
            remainder -= share;
        }

        // Distribute remainder using modulo arithmetic to prevent CPU exhaustion on massive ratios (O(N) instead of O(Remainder))
        decimal step = Amount >= 0m ? minUnit : -minUnit;
        decimal totalStepsDecimal = Math.Abs(remainder / minUnit);
        int totalSteps = (int)Math.Round(totalStepsDecimal);

        if (totalSteps > 0)
        {
            int fullPasses = totalSteps / results.Length;
            int leftover = totalSteps % results.Length;

            if (fullPasses > 0)
            {
                decimal passAmount = fullPasses * step;
                for (int i = 0; i < results.Length; i++)
                {
                    results[i] = new Money(results[i].Amount + passAmount, Currency);
                }
            }

            for (int i = 0; i < leftover; i++)
            {
                results[i] = new Money(results[i].Amount + step, Currency);
            }
        }

        return results;
    }

    /// <summary>
    /// Splits the monetary amount into equal parts, distributing any remainder pennies across initial parts.
    /// </summary>
    /// <param name="parts">The positive number of parts to divide into.</param>
    /// <returns>An array of <see cref="Money"/> instances representing the divided parts.</returns>
    /// <exception cref="DomainException"><paramref name="parts"/> is less than or equal to zero</exception>
    public Money[] Distribute(int parts)
    {
        DomainException.ThrowIf(parts <= 0, $"Cannot distribute Money into {parts} parts.");

        Span<int> ratios = parts <= 128 ? stackalloc int[parts] : new int[parts];
        ratios.Fill(1);
        return Allocate(ratios);
    }

    /// <summary>
    /// Gets a value indicating whether the monetary amount is zero.
    /// </summary>
    public bool IsZero => Amount == 0m;

    /// <summary>
    /// Gets a value indicating whether the monetary amount is strictly positive.
    /// </summary>
    public bool IsPositive => Amount > 0m;

    /// <summary>
    /// Gets a value indicating whether the monetary amount is strictly negative.
    /// </summary>
    public bool IsNegative => Amount < 0m;

    /// <summary>
    /// Determines whether this amount is strictly greater than another amount in the same currency.
    /// </summary>
    /// <param name="other">The monetary amount to compare with.</param>
    /// <returns><see langword="true"/> if this amount is greater; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DomainException"><paramref name="other"/> has a different currency</exception>
    public bool IsGreaterThan(Money other)
    {
        EnsureSameCurrency(other);
        return Amount > other.Amount;
    }

    /// <summary>
    /// Determines whether this amount is greater than or equal to another amount in the same currency.
    /// </summary>
    /// <param name="other">The monetary amount to compare with.</param>
    /// <returns><see langword="true"/> if this amount is greater than or equal; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DomainException"><paramref name="other"/> has a different currency</exception>
    public bool IsGreaterThanOrEqual(Money other)
    {
        EnsureSameCurrency(other);
        return Amount >= other.Amount;
    }

    /// <summary>
    /// Determines whether this amount is strictly less than another amount in the same currency.
    /// </summary>
    /// <param name="other">The monetary amount to compare with.</param>
    /// <returns><see langword="true"/> if this amount is less; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DomainException"><paramref name="other"/> has a different currency</exception>
    public bool IsLessThan(Money other)
    {
        EnsureSameCurrency(other);
        return Amount < other.Amount;
    }

    /// <summary>
    /// Determines whether this amount is less than or equal to another amount in the same currency.
    /// </summary>
    /// <param name="other">The monetary amount to compare with.</param>
    /// <returns><see langword="true"/> if this amount is less than or equal; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DomainException"><paramref name="other"/> has a different currency</exception>
    public bool IsLessThanOrEqual(Money other)
    {
        EnsureSameCurrency(other);
        return Amount <= other.Amount;
    }

    /// <summary>
    /// Compares this monetary amount to another amount denominated in the same currency.
    /// </summary>
    /// <param name="other">The monetary amount to compare against.</param>
    /// <returns>A value indicating the relative order of the amounts being compared.</returns>
    /// <exception cref="DomainException"><paramref name="other"/> has a different currency</exception>
    public int CompareTo(Money other)
    {
        bool thisUninit = string.IsNullOrEmpty(Currency.Value);
        bool otherUninit = string.IsNullOrEmpty(other.Currency.Value);

        if (thisUninit && otherUninit)
        {
            return Amount.CompareTo(other.Amount);
        }

        if (thisUninit) return -1;
        if (otherUninit) return 1;

        EnsureSameCurrency(other);
        return Amount.CompareTo(other.Amount);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="Money"/></exception>
    public int CompareTo(object? obj) =>
        obj is Money other ? CompareTo(other) : throw new ArgumentException("Object is not a Money instance.", nameof(obj));

    /// <summary>
    /// Determines whether the left monetary amount is less than the right monetary amount.
    /// </summary>
    /// <param name="a">The first monetary amount to compare.</param>
    /// <param name="b">The second monetary amount to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="a"/> is less than <paramref name="b"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DomainException">The currencies of <paramref name="a"/> and <paramref name="b"/> do not match</exception>
    public static bool operator <(Money a, Money b) => a.CompareTo(b) < 0;

    /// <summary>
    /// Determines whether the left monetary amount is greater than the right monetary amount.
    /// </summary>
    /// <param name="a">The first monetary amount to compare.</param>
    /// <param name="b">The second monetary amount to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="a"/> is greater than <paramref name="b"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DomainException">The currencies of <paramref name="a"/> and <paramref name="b"/> do not match</exception>
    public static bool operator >(Money a, Money b) => a.CompareTo(b) > 0;

    /// <summary>
    /// Determines whether the left monetary amount is less than or equal to the right monetary amount.
    /// </summary>
    /// <param name="a">The first monetary amount to compare.</param>
    /// <param name="b">The second monetary amount to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="a"/> is less than or equal to <paramref name="b"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DomainException">The currencies of <paramref name="a"/> and <paramref name="b"/> do not match</exception>
    public static bool operator <=(Money a, Money b) => a.CompareTo(b) <= 0;

    /// <summary>
    /// Determines whether the left monetary amount is greater than or equal to the right monetary amount.
    /// </summary>
    /// <param name="a">The first monetary amount to compare.</param>
    /// <param name="b">The second monetary amount to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="a"/> is greater than or equal to <paramref name="b"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DomainException">The currencies of <paramref name="a"/> and <paramref name="b"/> do not match</exception>
    public static bool operator >=(Money a, Money b) => a.CompareTo(b) >= 0;

    /// <summary>
    /// Rounds the monetary amount to the specified number of decimal places using banker's rounding (to nearest even).
    /// </summary>
    /// <param name="decimals">The optional decimal places to round to. If omitted, uses the currency's standard decimal places.</param>
    /// <returns>A new <see cref="Money"/> instance with the rounded amount.</returns>
    public Money Round(int? decimals = null) =>
        new(Math.Round(Amount, decimals ?? Currency.DecimalPlaces, MidpointRounding.ToEven), Currency);

    /// <summary>
    /// Rounds the monetary amount to the specified number of decimal places using commercial rounding (away from zero).
    /// </summary>
    /// <param name="decimals">The optional decimal places to round to. If omitted, uses the currency's standard decimal places.</param>
    /// <returns>A new <see cref="Money"/> instance with the rounded amount.</returns>
    public Money RoundCommercial(int? decimals = null) =>
        new(Math.Round(Amount, decimals ?? Currency.DecimalPlaces, MidpointRounding.AwayFromZero), Currency);

    /// <summary>
    /// Adds two monetary amounts having the same currency.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Add(Money)"/> for defensive workflows returning <see cref="Result{T}"/>.
    /// The <c>+</c> operator is designed for ergonomic expressions where invariants are assumed to hold;
    /// a currency mismatch is treated as an unrecoverable domain error throwing <see cref="DomainException"/>.
    /// </remarks>
    /// <param name="a">The first monetary amount.</param>
    /// <param name="b">The second monetary amount.</param>
    /// <returns>The resulting sum.</returns>
    /// <exception cref="DomainException">The currencies of <paramref name="a"/> and <paramref name="b"/> do not match</exception>
    public static Money operator +(Money a, Money b)
    {
        var res = a.Add(b);
        if (res.IsFailure) throw new DomainException(res.Error.Description);
        return res.Value;
    }

    /// <summary>
    /// Subtracts the second monetary amount from the first when both have the same currency.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Subtract(Money)"/> for defensive workflows returning <see cref="Result{T}"/>.
    /// The <c>-</c> operator is designed for ergonomic expressions where invariants are assumed to hold;
    /// a currency mismatch is treated as an unrecoverable domain error throwing <see cref="DomainException"/>.
    /// </remarks>
    /// <param name="a">The first monetary amount.</param>
    /// <param name="b">The second monetary amount to subtract.</param>
    /// <returns>The resulting difference.</returns>
    /// <exception cref="DomainException">The currencies of <paramref name="a"/> and <paramref name="b"/> do not match</exception>
    public static Money operator -(Money a, Money b)
    {
        var res = a.Subtract(b);
        if (res.IsFailure) throw new DomainException(res.Error.Description);
        return res.Value;
    }

    /// <summary>
    /// Multiplies a monetary amount by a decimal scalar factor.
    /// </summary>
    /// <param name="a">The monetary amount.</param>
    /// <param name="b">The scalar multiplier.</param>
    /// <returns>The product rounded to the currency's standard decimal places.</returns>
    public static Money operator *(Money a, decimal b) => a.Multiply(b);

    /// <summary>
    /// Multiplies a decimal scalar factor by a monetary amount.
    /// </summary>
    /// <param name="a">The scalar multiplier.</param>
    /// <param name="b">The monetary amount.</param>
    /// <returns>The product rounded to the currency's standard decimal places.</returns>
    public static Money operator *(decimal a, Money b) => b.Multiply(a);

    /// <summary>
    /// Negates the specified monetary amount.
    /// </summary>
    /// <param name="a">The monetary amount to negate.</param>
    /// <returns>The negated monetary amount.</returns>
    public static Money operator -(Money a) => a.Negate();

    /// <inheritdoc/>
    public override string ToString() => ToString(null, null);

    /// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        string numFormat = string.IsNullOrEmpty(format) ? $"N{Currency.DecimalPlaces}" : format;
        string formattedAmount = Amount.ToString(numFormat, formatProvider ?? CultureInfo.InvariantCulture);
        return string.IsNullOrEmpty(Currency.Value)
            ? formattedAmount
            : $"{formattedAmount} {Currency}";
    }

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        ReadOnlySpan<char> numFormat = format.IsEmpty
            ? (Currency.DecimalPlaces switch
            {
                0 => "N0",
                1 => "N1",
                2 => "N2",
                3 => "N3",
                4 => "N4",
                _ => "N2"
            })
            : format;
        if (!Amount.TryFormat(destination, out int amountChars, numFormat, provider ?? CultureInfo.InvariantCulture))
        {
            charsWritten = 0;
            return false;
        }

        if (string.IsNullOrEmpty(Currency.Value))
        {
            charsWritten = amountChars;
            return true;
        }

        string currencyStr = Currency.ToString();
        int needed = amountChars + 1 + currencyStr.Length;
        if (destination.Length < needed)
        {
            charsWritten = 0;
            return false;
        }

        destination[amountChars] = ' ';
        currencyStr.AsSpan().CopyTo(destination[(amountChars + 1)..]);
        charsWritten = needed;
        return true;
    }

    /// <inheritdoc/>
    public static Money Parse(string s, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        return TryParse(s.AsSpan(), provider, out var result)
            ? result
            : throw new FormatException($"Cannot parse '{s}' as Money.");
    }

    /// <inheritdoc/>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Money result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc/>
    public static Money Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Cannot parse '{s.ToString()}' as Money.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Money result)
    {
        result = default;
        ReadOnlySpan<char> trimmed = s.Trim();
        if (trimmed.IsEmpty)
        {
            return false;
        }

        int firstSpace = trimmed.IndexOf(' ');
        int lastSpace = trimmed.LastIndexOf(' ');

        if (firstSpace < 0)
        {
            return false;
        }

        // Case 1: "100.50 USD" -> amount at start, currency at end
        ReadOnlySpan<char> candidateAmount = trimmed[..lastSpace].Trim();
        ReadOnlySpan<char> candidateCurrency = trimmed[(lastSpace + 1)..].Trim();

        if (CurrencyCode.TryParse(candidateCurrency, provider, out var curr1) &&
            decimal.TryParse(candidateAmount, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, provider ?? CultureInfo.InvariantCulture, out decimal amt1))
        {
            var res = Create(amt1, curr1);
            if (res.IsSuccess)
            {
                result = res.Value;
                return true;
            }
        }

        // Case 2: "USD 100.50" -> currency at start, amount at end
        candidateCurrency = trimmed[..firstSpace].Trim();
        candidateAmount = trimmed[(firstSpace + 1)..].Trim();

        if (CurrencyCode.TryParse(candidateCurrency, provider, out var curr2) &&
            decimal.TryParse(candidateAmount, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, provider ?? CultureInfo.InvariantCulture, out decimal amt2))
        {
            var res = Create(amt2, curr2);
            if (res.IsSuccess)
            {
                result = res.Value;
                return true;
            }
        }

        return false;
    }

    private void EnsureSameCurrency(Money other)
    {
        DomainException.ThrowIf(
            string.IsNullOrEmpty(Currency.Value) || string.IsNullOrEmpty(other.Currency.Value),
            "Cannot operate on Money with uninitialized currency.");

        DomainException.ThrowIf(
            Currency != other.Currency,
            $"Cannot operate on Money with different currencies: '{Currency}' vs '{other.Currency}'.");
    }
}

