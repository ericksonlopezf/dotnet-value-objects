// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Immutable Value Object representing geographic coordinates on the WGS 84 ellipsoid.
/// </summary>
public readonly record struct GeoCoordinate : IValueObject<GeoCoordinate>, IParsable<GeoCoordinate>, ISpanParsable<GeoCoordinate>
{
    private const double EarthRadiusKilometers = 6371.0;

    /// <summary>Gets the latitude component in decimal degrees (-90.0 to 90.0).</summary>
    public double Latitude { get; }

    /// <summary>Gets the longitude component in decimal degrees (-180.0 to 180.0).</summary>
    public double Longitude { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="GeoCoordinate"/> with the specified latitude and longitude.
    /// </summary>
    private GeoCoordinate(double latitude, double longitude)
    {
        if (double.IsNaN(latitude) || double.IsInfinity(latitude) || latitude is < -90.0 or > 90.0)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90.0 and 90.0 degrees.");
        }

        if (double.IsNaN(longitude) || double.IsInfinity(longitude) || longitude is < -180.0 or > 180.0)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180.0 and 180.0 degrees.");
        }

        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Creates a validated <see cref="GeoCoordinate"/> instance.
    /// </summary>
    /// <param name="latitude">Latitude in decimal degrees (-90 to 90).</param>
    /// <param name="longitude">Longitude in decimal degrees (-180 to 180).</param>
    /// <returns>A <see cref="Result{GeoCoordinate}"/> containing the validated coordinate or a validation error.</returns>
    public static Result<GeoCoordinate> Create(double latitude, double longitude)
    {
        if (double.IsNaN(latitude) || double.IsInfinity(latitude) || latitude is < -90.0 or > 90.0)
        {
            return Result<GeoCoordinate>.Failure(Error.Validation(
                "GeoCoordinate.InvalidLatitude", $"Latitude must be between -90.0 and 90.0 degrees. Given: {latitude}."));
        }

        if (double.IsNaN(longitude) || double.IsInfinity(longitude) || longitude is < -180.0 or > 180.0)
        {
            return Result<GeoCoordinate>.Failure(Error.Validation(
                "GeoCoordinate.InvalidLongitude", $"Longitude must be between -180.0 and 180.0 degrees. Given: {longitude}."));
        }

        return Result<GeoCoordinate>.Success(new GeoCoordinate(latitude, longitude));
    }

    /// <summary>
    /// Calculates the great-circle distance to another coordinate using the Haversine formula in kilometers.
    /// </summary>
    /// <param name="target">The target coordinate.</param>
    /// <returns>The distance in kilometers.</returns>
    public double DistanceToKilometers(GeoCoordinate target)
    {
        double dLat = DegreesToRadians(target.Latitude - Latitude);
        double dLon = DegreesToRadians(target.Longitude - Longitude);

        double a = Math.Sin(dLat / 2.0) * Math.Sin(dLat / 2.0) +
                   Math.Cos(DegreesToRadians(Latitude)) * Math.Cos(DegreesToRadians(target.Latitude)) *
                   Math.Sin(dLon / 2.0) * Math.Sin(dLon / 2.0);

        double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));
        return EarthRadiusKilometers * c;
    }

    /// <summary>
    /// Calculates the great-circle distance to another coordinate in meters.
    /// </summary>
    public double DistanceToMeters(GeoCoordinate target) => DistanceToKilometers(target) * 1000.0;

    private static double DegreesToRadians(double degrees) => degrees * (Math.PI / 180.0);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{Latitude.ToString("F6", CultureInfo.InvariantCulture)}, {Longitude.ToString("F6", CultureInfo.InvariantCulture)}";

    /// <summary>
    /// Parses a string into a <see cref="GeoCoordinate"/>.
    /// </summary>
    /// <param name="s">The string to parse in "latitude, longitude" format.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="GeoCoordinate"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid coordinate format.</exception>
    public static GeoCoordinate Parse(string s, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        if (TryParse(s.AsSpan(), provider, out var result))
        {
            return result;
        }

        throw new FormatException($"String '{s}' was not recognized as a valid GeoCoordinate.");
    }

    /// <summary>
    /// Attempts to parse a string into a <see cref="GeoCoordinate"/>.
    /// </summary>
    /// <param name="s">The string to parse in "latitude, longitude" format.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed coordinate if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out GeoCoordinate result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <summary>
    /// Parses a span of characters into a <see cref="GeoCoordinate"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse in "latitude, longitude" format.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="GeoCoordinate"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid coordinate format.</exception>
    public static GeoCoordinate Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        throw new FormatException($"Span '{s.ToString()}' was not recognized as a valid GeoCoordinate.");
    }

    /// <summary>
    /// Attempts to parse a span of characters into a <see cref="GeoCoordinate"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse in "latitude, longitude" format.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed coordinate if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out GeoCoordinate result)
    {
        int commaIndex = s.IndexOf(',');
        if (commaIndex < 0)
        {
            result = default;
            return false;
        }

        ReadOnlySpan<char> latSpan = s[..commaIndex].Trim();
        ReadOnlySpan<char> lonSpan = s[(commaIndex + 1)..].Trim();

        if (double.TryParse(latSpan, NumberStyles.Float, provider ?? CultureInfo.InvariantCulture, out double lat) &&
            double.TryParse(lonSpan, NumberStyles.Float, provider ?? CultureInfo.InvariantCulture, out double lon))
        {
            var createResult = Create(lat, lon);
            if (createResult.IsSuccess)
            {
                result = createResult.Value;
                return true;
            }
        }

        result = default;
        return false;
    }
}
