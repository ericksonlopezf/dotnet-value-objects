// Copyright © Erickson Lopez. MIT License.
using System;
using System.Threading;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Attributes;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Immutable Value Object representing a DGII e-CF authentication challenge seed token (<c>Semilla</c>).
/// Used in the DGII Web Services authentication handshake flow for digital signature exchange.
/// </summary>
[RegulatoryRule("DO.DGII.ECF.SEC.001")]
public sealed record EcfAuthSeed : IValueObject<EcfAuthSeed>
{
    private const int DefaultExpirationMinutes = 5;
    private const int MinimumSeedLength = 8;
    private static readonly AsyncLocal<TimeProvider?> CustomTimeProvider = new();

    /// <summary>Sets or gets the ambient TimeProvider for expiration checks.</summary>
    public static TimeProvider AmbientTimeProvider
    {
        get => CustomTimeProvider.Value ?? TimeProvider.System;
        set => CustomTimeProvider.Value = value;
    }

    /// <summary>Gets the raw cryptographic seed string value.</summary>
    public string SeedValue { get; }

    /// <summary>Alias for <see cref="SeedValue"/>.</summary>
    public string Value => SeedValue;



    /// <summary>Gets the RNC of the receiver/issuer requesting or receiving the seed.</summary>
    public string ReceiverRnc { get; }

    /// <summary>Alias for <see cref="ReceiverRnc"/>.</summary>
    public string TaxpayerRnc => ReceiverRnc;

    /// <summary>Alias for <see cref="ReceiverRnc"/>.</summary>
    public string RNC => ReceiverRnc;

    /// <summary>Gets the generation timestamp (UTC).</summary>
    public DateTimeOffset GeneratedAt { get; }



    /// <summary>Gets the expiration timestamp (UTC).</summary>
    public DateTimeOffset ExpiresAt { get; }



    private EcfAuthSeed(string seedValue, string receiverRnc, DateTimeOffset generatedAt, DateTimeOffset expiresAt)
    {
        SeedValue = seedValue;
        ReceiverRnc = receiverRnc;
        GeneratedAt = generatedAt;
        ExpiresAt = expiresAt;
    }

    /// <summary>Gets a value indicating whether the seed challenge token has expired based on current UTC time.</summary>
    public bool IsExpired => AmbientTimeProvider.GetUtcNow() > ExpiresAt;

    /// <summary>Checks whether the seed is expired as of a specific timestamp.</summary>
    public bool IsExpiredAsOf(DateTimeOffset asOf) => asOf > ExpiresAt;

    /// <summary>Alias overload for compatibility with older tests.</summary>
    public bool IsExpiredAt(DateTimeOffset asOf) => asOf > ExpiresAt;



    /// <summary>Gets the remaining validity time before expiration.</summary>
    public TimeSpan RemainingTime => IsExpired ? TimeSpan.Zero : ExpiresAt - AmbientTimeProvider.GetUtcNow();



    /// <summary>
    /// Creates a validated <see cref="EcfAuthSeed"/> instance.
    /// </summary>
    public static Result<EcfAuthSeed> Create(
        string? seedValue,
        string? receiverRnc,
        DateTimeOffset generatedAt,
        DateTimeOffset expiresAt)
    {
        if (string.IsNullOrWhiteSpace(seedValue))
        {
            return Result<EcfAuthSeed>.Failure(Error.Validation(
                "EcfAuthSeed.Required", "Seed value is required."));
        }

        string cleanSeed = seedValue.Trim();
        if (cleanSeed.Length < MinimumSeedLength)
        {
            return Result<EcfAuthSeed>.Failure(Error.Validation(
                "EcfAuthSeed.TooShort", $"Seed value must contain at least {MinimumSeedLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(receiverRnc))
        {
            return Result<EcfAuthSeed>.Failure(Error.Validation(
                "EcfAuthSeed.RncRequired", "Receiver RNC is required for authentication seed."));
        }

        if (expiresAt <= generatedAt)
        {
            return Result<EcfAuthSeed>.Failure(Error.Validation(
                "EcfAuthSeed.InvalidExpiration", "Expiration timestamp must be greater than generation timestamp."));
        }

        return Result<EcfAuthSeed>.Success(new EcfAuthSeed(cleanSeed, receiverRnc.Trim(), generatedAt, expiresAt));
    }

    /// <summary>
    /// Creates a validated <see cref="EcfAuthSeed"/> instance accepting strongly typed <see cref="Rnc"/>.
    /// </summary>
    public static Result<EcfAuthSeed> Create(
        string? seedValue,
        Rnc taxpayerRnc,
        DateTimeOffset generatedAt,
        DateTimeOffset expiresAt) =>
        Create(seedValue, taxpayerRnc.Value, generatedAt, expiresAt);

    /// <summary>
    /// Creates a validated seed with standard expiration duration (5 minutes).
    /// </summary>
    public static Result<EcfAuthSeed> CreateWithStandardExpiration(
        string? seedValue,
        Rnc taxpayerRnc,
        DateTimeOffset generatedAt,
        int expirationMinutes = DefaultExpirationMinutes) =>
        Create(seedValue, taxpayerRnc.Value, generatedAt, generatedAt.AddMinutes(expirationMinutes));

    /// <summary>
    /// Creates a new validated seed with the specified expiration in minutes.
    /// </summary>
    public static EcfAuthSeed CreateWithExpiration(string seedValue, string receiverRnc, int expirationMinutes = DefaultExpirationMinutes)
    {
        DateTimeOffset now = AmbientTimeProvider.GetUtcNow();
        var res = Create(seedValue, receiverRnc, now, now.AddMinutes(expirationMinutes));
        if (res.IsFailure)
        {
            throw new ArgumentException(res.Error.Description, res.Error.Code);
        }

        return res.Value;
    }

    /// <summary>
    /// Reconstitutes a seed from external XML data.
    /// </summary>
    public static EcfAuthSeed FromXml(string seedValue, string receiverRnc, DateTimeOffset generatedAt, DateTimeOffset expiresAt) =>
        Reconstitute(seedValue, receiverRnc, generatedAt, expiresAt);

    /// <summary>
    /// Validates that the seed has not expired.
    /// </summary>
    public void ValidateExpiration()
    {
        if (IsExpired)
        {
            throw new InvalidOperationException("SEED_EXPIRED");
        }
    }

    /// <summary>
    /// Generates a new fresh seed challenge for the specified receiver RNC.
    /// </summary>
    public static EcfAuthSeed Generate(string receiverRnc, int expirationMinutes = DefaultExpirationMinutes, TimeProvider? timeProvider = null)
    {
        DateTimeOffset now = timeProvider?.GetUtcNow() ?? AmbientTimeProvider.GetUtcNow();
#if NET9_0_OR_GREATER
        string token = $"{Guid.CreateVersion7():N}{now.Ticks}";
#else
        string token = $"{Guid.NewGuid():N}{now.Ticks}";
#endif
        return new EcfAuthSeed(token, receiverRnc.Trim(), now, now.AddMinutes(expirationMinutes));
    }


    /// <summary>Reconstitutes an instance from persistence storage without validation.</summary>
    public static EcfAuthSeed Reconstitute(string seedValue, string receiverRnc, DateTimeOffset generatedAt, DateTimeOffset expiresAt) =>
        new(seedValue, receiverRnc, generatedAt, expiresAt);

    /// <inheritdoc/>
    public override string ToString() => $"EcfAuthSeed[{SeedValue[..Math.Min(8, SeedValue.Length)]}...] RNC:{ReceiverRnc} Exp:{ExpiresAt:HH:mm:ss}";
}
