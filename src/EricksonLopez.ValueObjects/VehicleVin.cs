// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a Vehicle Identification Number (VIN) compliant with ISO 3779 and ISO 3780 standards.
/// Exactly 17 alphanumeric characters (excluding letters I, O, Q) with Modulo 11 check-digit verification at position 9.
/// </summary>
public sealed record VehicleVin : StringValueObject<VehicleVin>
{
    private static readonly int[] PositionWeights = [8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2];

    private static readonly FrozenDictionary<char, int> ModelYearCodes = new Dictionary<char, int>
    {
        ['A'] = 2010,
        ['B'] = 2011,
        ['C'] = 2012,
        ['D'] = 2013,
        ['E'] = 2014,
        ['F'] = 2015,
        ['G'] = 2016,
        ['H'] = 2017,
        ['J'] = 2018,
        ['K'] = 2019,
        ['L'] = 2020,
        ['M'] = 2021,
        ['N'] = 2022,
        ['P'] = 2023,
        ['R'] = 2024,
        ['S'] = 2025,
        ['T'] = 2026,
        ['V'] = 2027,
        ['W'] = 2028,
        ['X'] = 2029,
        ['Y'] = 2030,
        ['1'] = 2001,
        ['2'] = 2002,
        ['3'] = 2003,
        ['4'] = 2004,
        ['5'] = 2005,
        ['6'] = 2006,
        ['7'] = 2007,
        ['8'] = 2008,
        ['9'] = 2009
    }.ToFrozenDictionary();

    private VehicleVin(string value) : base(value) { }

    /// <summary>Gets the World Manufacturer Identifier (WMI) occupying positions 1 to 3.</summary>
    public string Wmi => Value[..3];

    /// <summary>Alias for <see cref="Wmi"/>.</summary>
    public string WorldManufacturerIdentifier => Wmi;

    /// <summary>Gets the Vehicle Descriptor Section (VDS) occupying positions 4 to 9.</summary>
    public string Vds => Value[3..9];

    /// <summary>Alias for <see cref="Vds"/>.</summary>
    public string VehicleDescriptorSection => Vds;

    /// <summary>Gets the Vehicle Identifier Section (VIS) occupying positions 10 to 17.</summary>
    public string Vis => Value[9..17];

    /// <summary>Alias for <see cref="Vis"/>.</summary>
    public string VehicleIdentifierSection => Vis;

    /// <summary>Gets the decoded model year from position 10, or <c>0</c> if unrecognized.</summary>
    public int ModelYear => ModelYearCodes.GetValueOrDefault(Value[9], 0);

    /// <summary>Reconstitutes an instance from persistence storage without validation.</summary>
    internal static VehicleVin Reconstitute(string value) => new(value);

    /// <summary>Hydrates an instance from persistence storage.</summary>
    internal static VehicleVin Hydrate(string value) => new(value);

    /// <summary>
    /// Creates a validated <see cref="VehicleVin"/> instance according to ISO 3779 check digit specifications.
    /// </summary>
    /// <param name="value">The raw 17-character VIN string.</param>
    /// <returns>A <see cref="Result{VehicleVin}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<VehicleVin> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<VehicleVin>.Failure(Error.Validation(
                "VehicleVin.Required", "Vehicle Identification Number (VIN) is required."));
        }

        string normalized = value.Trim().ToUpperInvariant();
        if (normalized.Length != 17)
        {
            return Result<VehicleVin>.Failure(Error.Validation(
                "VehicleVin.InvalidLength", "VIN must be exactly 17 characters long."));
        }

        for (int i = 0; i < 17; i++)
        {
            char c = normalized[i];
            if (c is 'I' or 'O' or 'Q')
            {
                return Result<VehicleVin>.Failure(Error.Validation(
                    "VehicleVin.ForbiddenCharacters", "VIN must not contain forbidden characters 'I', 'O', or 'Q'."));
            }

            if (!char.IsLetterOrDigit(c))
            {
                return Result<VehicleVin>.Failure(Error.Validation(
                    "VehicleVin.InvalidFormat", "VIN must contain only uppercase alphanumeric characters."));
            }
        }

        if (!IsValidCheckDigit(normalized))
        {
            return Result<VehicleVin>.Failure(Error.Validation(
                "VehicleVin.InvalidCheckDigit", "VIN check digit at position 9 is invalid per ISO 3779."));
        }

        return Result<VehicleVin>.Success(new VehicleVin(normalized));
    }

    private static bool IsValidCheckDigit(ReadOnlySpan<char> vin)
    {
        int sum = 0;
        for (int i = 0; i < 17; i++)
        {
            int translit = Transliterate(vin[i]);
            if (translit < 0)
            {
                return false;
            }

            sum += translit * PositionWeights[i];
        }

        int remainder = sum % 11;
        char expectedChar = remainder == 10 ? 'X' : (char)('0' + remainder);

        return vin[8] == expectedChar;
    }

    private static int Transliterate(char c) => c switch
    {
        'A' or 'J' or '1' => 1,
        'B' or 'K' or 'S' or '2' => 2,
        'C' or 'L' or 'T' or '3' => 3,
        'D' or 'M' or 'U' or '4' => 4,
        'E' or 'N' or 'V' or '5' => 5,
        'F' or 'W' or '6' => 6,
        'G' or 'P' or 'X' or '7' => 7,
        'H' or 'Y' or '8' => 8,
        'R' or 'Z' or '9' => 9,
        '0' => 0,
        _ => -1
    };
}
