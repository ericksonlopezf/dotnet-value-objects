// Copyright © Erickson Lopez. MIT License.
using System;
using System.Diagnostics;
using System.Net.Mail;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a validated RFC 5321 email address normalized to lowercase.
/// </summary>
[SensitiveData(mask: "user***@domain.com")]
[DebuggerDisplay("{" + nameof(ToString) + "()}")]
public readonly record struct Email : IValueObject<Email>, IComparable<Email>, IComparable, IParsable<Email>, ISpanParsable<Email>
{
    /// <summary>
    /// Gets the normalized email address string.
    /// </summary>
    public string Value { get; }

    private Email(string value) => Value = value;

    /// <summary>
    /// Creates a validated <see cref="Email"/> instance from an email address string.
    /// </summary>
    /// <param name="value">The raw email address string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated email, or a validation failure.</returns>
    public static Result<Email> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<Email>.Failure(Error.Validation(
                "Email.Required", "Email is required."));
        }

        string trimmed = value.Trim();

        if (trimmed.Length > 320)
        {
            return Result<Email>.Failure(Error.Validation(
                "Email.TooLong", "Email must be ≤320 characters."));
        }

        if (!MailAddress.TryCreate(trimmed, out _))
        {
            return Result<Email>.Failure(Error.Validation(
                "Email.InvalidFormat", $"Invalid email format: '{trimmed}'."));
        }

        return Result<Email>.Success(new Email(trimmed.ToLowerInvariant()));
    }

    /// <summary>
    /// Gets the local mailbox portion of the email address preceding the <c>@</c> sign.
    /// </summary>
    public string LocalPart => Value is not null && Value.Contains('@') ? Value[..Value.LastIndexOf('@')] : string.Empty;

    /// <summary>
    /// Gets the domain portion of the email address succeeding the <c>@</c> sign.
    /// </summary>
    public string Domain => Value is not null && Value.Contains('@') ? Value[(Value.LastIndexOf('@') + 1)..] : string.Empty;

    /// <summary>
    /// Returns the email address with the local mailbox portion masked for secure diagnostic logging.
    /// </summary>
    /// <returns>The masked email address string.</returns>
    public string Masked()
    {
        if (string.IsNullOrEmpty(Value)) return string.Empty;
        int atIndex = Value.LastIndexOf('@');
        if (atIndex <= 1) return "***" + Value[atIndex..];
        return Value[..1] + "***" + Value[atIndex..];
    }

    /// <summary>
    /// Compares this email address with another email address using ordinal string comparison.
    /// </summary>
    /// <param name="other">The other email address to compare against.</param>
    /// <returns>A value indicating the relative order of the email addresses being compared.</returns>
    public int CompareTo(Email other) => string.Compare(Value, other.Value, StringComparison.Ordinal);

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="Email"/></exception>
    public int CompareTo(object? obj) =>
        obj is Email other ? CompareTo(other) : throw new ArgumentException("Object is not an Email", nameof(obj));

    /// <summary>
    /// Determines whether the left email address is less than the right email address.
    /// </summary>
    /// <param name="left">The first email address to compare.</param>
    /// <param name="right">The second email address to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Email left, Email right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left email address is less than or equal to the right email address.
    /// </summary>
    /// <param name="left">The first email address to compare.</param>
    /// <param name="right">The second email address to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Email left, Email right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left email address is greater than the right email address.
    /// </summary>
    /// <param name="left">The first email address to compare.</param>
    /// <param name="right">The second email address to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Email left, Email right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left email address is greater than or equal to the right email address.
    /// </summary>
    /// <param name="left">The first email address to compare.</param>
    /// <param name="right">The second email address to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Email left, Email right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => Value ?? string.Empty;

    /// <summary>
    /// Parses a string into an <see cref="Email"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="Email"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid email format</exception>
    public static Email Parse(string s, IFormatProvider? provider = null)
    {
        var result = Create(s);
        return result.IsSuccess ? result.Value : throw new FormatException(result.Error.Description);
    }

    /// <summary>
    /// Attempts to parse a string into an <see cref="Email"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed email if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out Email result)
    {
        var res = Create(s);
        if (res.IsSuccess)
        {
            result = res.Value;
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Parses a span of characters into an <see cref="Email"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="Email"/>.</returns>
    public static Email Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        Parse(s.ToString(), provider);

    /// <summary>
    /// Attempts to parse a span of characters into an <see cref="Email"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed email if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Email result) =>
        TryParse(s.ToString(), provider, out result);
}



