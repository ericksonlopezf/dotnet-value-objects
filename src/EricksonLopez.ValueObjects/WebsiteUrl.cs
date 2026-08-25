// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an absolute HTTP or HTTPS website URL.
/// </summary>
public sealed record WebsiteUrl : StringValueObject<WebsiteUrl>
{
    /// <summary>
    /// Gets the host component of the URL.
    /// </summary>
    public string Host { get; }

    private WebsiteUrl(string value, string host) : base(value)
    {
        Host = host;
    }

    /// <summary>
    /// Creates a validated <see cref="WebsiteUrl"/> instance from an absolute HTTP or HTTPS URL string.
    /// </summary>
    /// <param name="value">The raw website URL string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated website URL, or a validation failure.</returns>
    public static Result<WebsiteUrl> Create(string? value)
    {
        Result<string> normalized = StringPipeline.RequiredString(
            value,
            nameof(WebsiteUrl),
            8,
            2048,
            static raw => raw.Trim());

        if (normalized.IsFailure)
        {
            return Result<WebsiteUrl>.Failure(normalized.Error);
        }

        if (!Uri.TryCreate(normalized.Value, UriKind.Absolute, out Uri? uri) ||
            uri.Scheme is not ("http" or "https") ||
            string.IsNullOrWhiteSpace(uri.Host))
        {
            return Result<WebsiteUrl>.Failure(Error.Validation(
                "WebsiteUrl.InvalidFormat",
                "Website URL must be an absolute HTTP or HTTPS URL."));
        }

        return Result<WebsiteUrl>.Success(new WebsiteUrl(uri.ToString(), uri.Host));
    }
}


