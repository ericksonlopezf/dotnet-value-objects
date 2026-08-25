// Copyright © Erickson Lopez. MIT License.
using System;
using System.Diagnostics;
using System.Linq;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a cryptographically hashed password (argon2, bcrypt, scrypt, pbkdf2, etc.).
/// Guarantees that raw plaintext passwords are never stored or logged in domain aggregates.
/// Masked by default via <see cref="SensitiveDataAttribute"/> and <see cref="object.ToString()"/>.
/// </summary>
[SensitiveData(mask: "***HASHED***")]
[DebuggerDisplay("{" + nameof(ToString) + "()}")]
public sealed record PasswordHash : StringValueObject<PasswordHash>
{
    /// <inheritdoc/>
    protected override bool IsSensitive => true;
    /// <inheritdoc/>
    protected override string Mask => "***HASHED***";
    private PasswordHash(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="PasswordHash"/> instance after validating format and length.
    /// </summary>
    /// <param name="value">The raw password hash string.</param>
    /// <returns>A <see cref="Result{PasswordHash}"/> containing the created instance or a validation error.</returns>
    public static Result<PasswordHash> Create(string? value)
    {
        Result<string> normalized = StringPipeline.RequiredString(
            value, nameof(PasswordHash), 20, 512, static raw => raw.Trim());

        if (normalized.IsFailure)
        {
            return Result<PasswordHash>.Failure(normalized.Error);
        }

        if (normalized.Value.Any(char.IsWhiteSpace))
        {
            return Result<PasswordHash>.Failure(Error.Validation(
                "PasswordHash.ContainsWhitespace", "Password hash cannot contain whitespace."));
        }

        return Result<PasswordHash>.Success(new PasswordHash(normalized.Value));
    }
}


