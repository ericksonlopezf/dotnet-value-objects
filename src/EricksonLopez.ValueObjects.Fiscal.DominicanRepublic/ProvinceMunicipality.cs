// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Attributes;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Value Object representing a Dominican Republic Province and Municipality code according to the official DGII catalog.
/// Format: <c>PPMMM</c> where <c>PP</c> = Province (01-32), <c>MMM</c> = Municipality (001-999).
/// </summary>
[RegulatoryRule("DO.DGII.CAT.002")]
public sealed record ProvinceMunicipality : StringValueObject<ProvinceMunicipality>
{
    private static readonly FrozenDictionary<string, string> ProvinceNames = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["01"] = "Distrito Nacional",
        ["02"] = "Azua",
        ["03"] = "Baoruco",
        ["04"] = "Barahona",
        ["05"] = "Dajabón",
        ["06"] = "Duarte",
        ["07"] = "Elías Piña",
        ["08"] = "El Seibo",
        ["09"] = "Espaillat",
        ["10"] = "Independencia",
        ["11"] = "La Altagracia",
        ["12"] = "La Romana",
        ["13"] = "La Vega",
        ["14"] = "María Trinidad Sánchez",
        ["15"] = "Monte Cristi",
        ["16"] = "Pedernales",
        ["17"] = "Peravia",
        ["18"] = "Puerto Plata",
        ["19"] = "Hermanas Mirabal",
        ["20"] = "Samaná",
        ["21"] = "San Cristóbal",
        ["22"] = "San Juan",
        ["23"] = "San Pedro de Macorís",
        ["24"] = "Sánchez Ramírez",
        ["25"] = "Santiago",
        ["26"] = "Santiago Rodríguez",
        ["27"] = "Valverde",
        ["28"] = "Monseñor Nouel",
        ["29"] = "Monte Plata",
        ["30"] = "Hato Mayor",
        ["31"] = "San José de Ocoa",
        ["32"] = "Santo Domingo"
    }.ToFrozenDictionary();

    private ProvinceMunicipality(string value) : base(value) { }

    /// <summary>Gets the full 5-digit code (<c>PPMMM</c>).</summary>
    public string Code => Value;

    /// <summary>Gets the 2-digit province code (<c>PP</c>).</summary>
    public string ProvinceCode => Value[..2];

    /// <summary>Gets the 3-digit municipality code (<c>MMM</c>).</summary>
    public string MunicipalityCode => Value[2..];

    /// <summary>Gets the official DGII name of the province.</summary>
    public string ProvinceName => ProvinceNames.GetValueOrDefault(ProvinceCode, "Desconocida");

    /// <summary>Reconstitutes an instance from persistence storage without validation.</summary>
    internal static ProvinceMunicipality Reconstitute(string value) => new(value);

    /// <summary>Hydrates an instance from persistence storage.</summary>
    internal static ProvinceMunicipality Hydrate(string value) => new(value);

    /// <summary>
    /// Creates a validated <see cref="ProvinceMunicipality"/> instance from a 5-digit string.
    /// </summary>
    /// <param name="value">The 5-digit <c>PPMMM</c> string.</param>
    /// <returns>A <see cref="Result{ProvinceMunicipality}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<ProvinceMunicipality> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<ProvinceMunicipality>.Failure(Error.Validation(
                "ProvinceMunicipality.Required", "Province/Municipality code is required."));
        }

        string trimmed = value.Trim();
        if (trimmed.Length != 5)
        {
            return Result<ProvinceMunicipality>.Failure(Error.Validation(
                "ProvinceMunicipality.InvalidLength", "Province/Municipality code must be exactly 5 numeric digits."));
        }

        for (int i = 0; i < 5; i++)
        {
            if (!char.IsAsciiDigit(trimmed[i]))
            {
                return Result<ProvinceMunicipality>.Failure(Error.Validation(
                    "ProvinceMunicipality.InvalidFormat", "Province/Municipality code must contain only digits."));
            }
        }

        string provinceCode = trimmed[..2];
        if (!ProvinceNames.ContainsKey(provinceCode))
        {
            return Result<ProvinceMunicipality>.Failure(Error.Validation(
                "ProvinceMunicipality.InvalidProvince", $"Province code '{provinceCode}' is outside the valid DGII range (01-32)."));
        }

        return Result<ProvinceMunicipality>.Success(new ProvinceMunicipality(trimmed));
    }

    /// <summary>
    /// Tries to create an instance without throwing.
    /// </summary>
    public static bool TryCreate(string? value, out ProvinceMunicipality? result)
    {
        var res = Create(value);
        if (res.IsSuccess)
        {
            result = res.Value;
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>Gets the formatted display name: ProvinceName (Code).</summary>
    public string DisplayName => $"{ProvinceName} ({Value})";
}
