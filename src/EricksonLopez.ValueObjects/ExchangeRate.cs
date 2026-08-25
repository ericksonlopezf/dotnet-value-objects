// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an exchange rate between two ISO 4217 currencies.
/// </summary>
/// <remarks>
/// Does not encapsulate temporal validity; temporal context should be managed by the aggregate or repository.
/// </remarks>
public readonly record struct ExchangeRate : IValueObject<ExchangeRate>
{
    /// <summary>
    /// Gets the source ISO 4217 currency code.
    /// </summary>
    public CurrencyCode FromCurrency { get; }

    /// <summary>
    /// Gets the target ISO 4217 currency code.
    /// </summary>
    public CurrencyCode ToCurrency { get; }

    /// <summary>
    /// Gets the numerical exchange conversion rate from source to target currency.
    /// </summary>
    public decimal Rate { get; }

    private ExchangeRate(CurrencyCode fromCurrency, CurrencyCode toCurrency, decimal rate)
    {
        FromCurrency = fromCurrency;
        ToCurrency = toCurrency;
        Rate = rate;
    }

    /// <summary>
    /// Creates a validated <see cref="ExchangeRate"/> instance between two distinct currencies.
    /// </summary>
    /// <param name="fromCurrency">The source currency code.</param>
    /// <param name="toCurrency">The target currency code.</param>
    /// <param name="rate">The positive exchange rate factor.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated exchange rate, or a validation failure.</returns>
    public static Result<ExchangeRate> Create(CurrencyCode fromCurrency, CurrencyCode toCurrency, decimal rate)
    {
        if (fromCurrency == toCurrency)
        {
            return Result<ExchangeRate>.Failure(Error.Validation(
                "ExchangeRate.SameCurrency", "Exchange rate must convert between different currencies."));
        }

        if (rate <= 0m)
        {
            return Result<ExchangeRate>.Failure(Error.Validation(
                "ExchangeRate.NonPositive", "Exchange rate must be greater than zero."));
        }

        if (!NumericValidation.IsScaleAtMost(rate, 12))
        {
            return Result<ExchangeRate>.Failure(Error.Validation(
                "ExchangeRate.TooManyDecimals", "Exchange rate supports at most 12 decimal places."));
        }

        return Result<ExchangeRate>.Success(new ExchangeRate(fromCurrency, toCurrency, rate));
    }

    /// <summary>
    /// Converts a <see cref="Money"/> amount from the source currency to the target currency.
    /// </summary>
    /// <param name="amount">The monetary amount in the source currency to convert.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the converted amount in the target currency, or a currency mismatch error.</returns>
    public Result<Money> Convert(Money amount)
    {
        if (amount.Currency != FromCurrency)
        {
            return Result<Money>.Failure(Error.Validation(
                "ExchangeRate.CurrencyMismatch",
                $"Cannot convert {amount.Currency} using an exchange rate for {FromCurrency}."));
        }

        return Money.Create(amount.Amount * Rate, ToCurrency);
    }

    /// <summary>
    /// Calculates the inverse exchange rate converting from the target currency back to the source currency.
    /// </summary>
    /// <returns>A successful <see cref="Result{T}"/> containing the inverted exchange rate.</returns>
    public Result<ExchangeRate> Inverse()
    {
        decimal inverseRate = Math.Round(1m / Rate, 12);
        return Create(ToCurrency, FromCurrency, inverseRate);
    }

    /// <inheritdoc/>
    public override string ToString() => $"{FromCurrency}/{ToCurrency} = {Rate.ToString(CultureInfo.InvariantCulture)}";
}


