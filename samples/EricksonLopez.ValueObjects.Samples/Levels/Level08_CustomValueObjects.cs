// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Generic;
using System.Text.Json;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Attributes;
using EricksonLopez.ValueObjects.Serialization.Json;

namespace EricksonLopez.ValueObjects.Samples.Levels;

/// <summary>
/// Level 08: Custom Value Object Creation, Extensibility, and Attribute Governance.
/// Demonstrates:
/// <list type="bullet">
///   <item><see cref="IValueObject"/> and <see cref="IValueObject{TSelf}"/> interface contracts</item>
///   <item><see cref="SensitiveDataAttribute"/> on a custom VO for automatic PII masking</item>
///   <item><see cref="RegulatoryRuleAttribute"/> to trace back domain rules to legal identifiers</item>
///   <item><see cref="ValueObjectAttribute"/> as a source generator trigger</item>
///   <item>Custom <see cref="StringValueObject{TSelf}"/> with uppercase normalization</item>
///   <item>Custom composite <see cref="ValueObject"/> with domain invariants</item>
///   <item><see cref="StringValueObjectJsonConverter{TSelf}"/> extension pattern</item>
///   <item><see cref="SingleValueObjectJsonConverter{TSelf,TValue}"/> extension pattern</item>
/// </list>
/// </summary>
public static class Level08_CustomValueObjects
{
    /// <summary>
    /// Executes the custom Value Object demonstration.
    /// </summary>
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n===============================================================================");
        Console.WriteLine(" [LEVEL 8] EXTENSIBILITY: CUSTOM VALUE OBJECTS, ATTRIBUTES, AND JSON");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        // ─── 1. IValueObject / IValueObject<TSelf> — Interface contracts ───────────────
        Console.WriteLine("[1. IValueObject & IValueObject<TSelf> — Interface Contracts]");

        // IValueObject is a marker interface — all VOs implement it.
        IValueObject emailAsInterface = Email.Create("demo@ericksonlopez.dev").Value;
        Console.WriteLine($"  - Email implements IValueObject: {emailAsInterface is IValueObject}");

        // IValueObject<TSelf> adds IEquatable<TSelf> constraint.
        // All struct VOs implement IValueObject<TSelf> directly.
        var money1 = Money.Create(100.00m, "USD").Value;
        var money2 = Money.Create(100.00m, "USD").Value;
        IValueObject<Money> moneyAsTyped = money1;
        Console.WriteLine($"  - Money implements IValueObject<Money>: {moneyAsTyped is IValueObject<Money>}");
        Console.WriteLine($"  - IEquatable<Money>.Equals: {money1.Equals(money2)}");

        // ─── 2. Custom StringValueObject with [SensitiveData] ───────────────────────────
        Console.WriteLine("\n[2. Custom StringValueObject<TSelf> with [SensitiveData] PII Masking]");
        var apiKeyResult = ApiKey.Create("sk-live-abc123-super-secret-token-xyz");
        if (apiKeyResult.IsSuccess)
        {
            var apiKey = apiKeyResult.Value;
            Console.WriteLine($"  - Real value       : {apiKey.Value}");
            Console.WriteLine($"  - ToString() masked: {apiKey}  (SensitiveData masking)");
        }

        // ─── 3. Custom VO with [RegulatoryRule] ─────────────────────────────────────────
        Console.WriteLine("\n[3. Custom VO with [RegulatoryRule] — Legal Traceability]");
        var isinResult = IsinCode.Create("US0378331005");
        if (isinResult.IsSuccess)
        {
            var isin = isinResult.Value;
            Console.WriteLine($"  - Valid ISIN       : {isin.Value}");
            Console.WriteLine($"  - [RegulatoryRule] traces to ISO 6166 standard in type metadata");
        }

        var isinBad = IsinCode.Create("INVALID");
        Console.WriteLine($"  - Invalid ISIN     : IsFailure={isinBad.IsFailure}, Code={isinBad.Error.Code}");

        // ─── 4. [ValueObjectAttribute] — Source Generator Trigger ──────────────────────
        Console.WriteLine("\n[4. [ValueObjectAttribute] — Source Generator Trigger]");
        Console.WriteLine("  - [ValueObject] decorates a type so the Roslyn Source Generator");
        Console.WriteLine("    automatically synthesizes factories, parsers, formatters, and equality.");
        Console.WriteLine("  - GenerateConversionOperators = true: generates implicit/explicit operators.");
        Console.WriteLine("  - GeneratePersistenceHooks = true (default): generates persistence hooks.");
        Console.WriteLine("  (This demonstration operates at compile-time; output is reflected in generated code.)");

        // ─── 5. Custom StringValueObject for ProjectCode ──────────────────────────────
        Console.WriteLine("\n[5. Custom StringValueObject<TSelf> with Normalization Pipeline]");
        var projectCodeResult = ProjectCode.Create("  prj-2026-cloud-migration  ");
        if (projectCodeResult.IsSuccess)
        {
            var projectCode = projectCodeResult.Value;
            Console.WriteLine($"  - Project Code     : {projectCode.Value}");
            Console.WriteLine($"  - Normalization    : Uppercase and sanitized");
            Console.WriteLine($"  - ToString()       : {projectCode}  (unmasked)");

            // Comparison via IComparable (inherited from SingleValueObject<TSelf,TValue>)
            var projectCode2 = ProjectCode.Create("PRJ-2026-DATA-MIGRATION").Value;
            Console.WriteLine($"  - CompareTo(other) : {projectCode.CompareTo(projectCode2)} (ordinal)");
        }

        // ─── 6. Custom Composite ValueObject ───────────────────────────────────────────
        Console.WriteLine("\n[6. Custom Composite ValueObject — GeoCoordinate]");
        var geoResult = GeoCoordinate.Create(18.4861, -69.9312);
        if (geoResult.IsSuccess)
        {
            var geo = geoResult.Value;
            Console.WriteLine($"  - GeoCoordinate    : {geo}");
            Console.WriteLine($"  - Latitude         : {geo.Latitude}");
            Console.WriteLine($"  - Longitude        : {geo.Longitude}");
        }

        // Invalid coordinates
        var geoInvalid = GeoCoordinate.Create(95.0, 0.0); // Latitude > 90
        Console.WriteLine($"  - Lat=95 IsFailure : {geoInvalid.IsFailure}, [{geoInvalid.Error.Code}]");

        // ─── 7. StringValueObjectJsonConverter<TSelf> — Extensible JSON converter ───────
        Console.WriteLine("\n[7. StringValueObjectJsonConverter<TSelf> — Extensible JSON Serialization]");
        var options = new JsonSerializerOptions { WriteIndented = false };
        options.Converters.Add(new ProjectCodeJsonConverter());

        var code = ProjectCode.Create("PRJ-2026-CLOUD-MIGRATION").Value;
        string jsonOut = JsonSerializer.Serialize(code, options);
        Console.WriteLine($"  - Serialized       : {jsonOut}");

        var deserialized = JsonSerializer.Deserialize<ProjectCode>(jsonOut, options);
        Console.WriteLine($"  - Deserialized     : {deserialized?.Value}");

        // ─── 8. SingleValueObjectJsonConverter<TSelf,TValue> — for non-string VOs ──────
        Console.WriteLine("\n[8. SingleValueObjectJsonConverter<TSelf,TValue> — For Non-String Primitive VOs]");
        Console.WriteLine("  - For string VOs → use StringValueObjectJsonConverter<TSelf>");
        Console.WriteLine("  - For VOs with TValue != string → inherit from SingleValueObjectJsonConverter<TSelf,TValue>");
        Console.WriteLine("  - Both classes are abstract; consumer implements only CreateInstance(value).");
        Console.WriteLine("  - See ProjectCodeJsonConverter (below) as reference implementation.");
    }

    // ═══════════════════════════════════════════════════════════════════════════════════
    // INTERNAL CUSTOM VALUE OBJECTS — Demonstración de implementación
    // ═══════════════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Custom string-based Value Object for API keys.
    /// Decorated with <see cref="SensitiveDataAttribute"/> to mask the value in logs.
    /// </summary>
    [SensitiveData(mask: "sk-***")]
    private sealed record ApiKey : StringValueObject<ApiKey>
    {
        // IsSensitive = true porque el atributo lo exige, pero como este VO
        // es un record (no struct), necesitamos sobreescribir IsSensitive explícitamente.
        protected override bool IsSensitive => true;
        protected override string Mask => "sk-***";

        private ApiKey(string value) : base(value) { }

        /// <summary>Creates a validated API key with a minimum length and 'sk-' prefix.</summary>
        public static Result<ApiKey> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<ApiKey>.Failure(Error.Validation("ApiKey.Required", "API key is required."));

            string trimmed = value.Trim();
            if (trimmed.Length < 10)
                return Result<ApiKey>.Failure(Error.Validation("ApiKey.TooShort", "API key must be at least 10 characters."));

            if (!trimmed.StartsWith("sk-", StringComparison.Ordinal))
                return Result<ApiKey>.Failure(Error.Validation("ApiKey.InvalidPrefix", "API key must start with 'sk-'."));

            return Result<ApiKey>.Success(new ApiKey(trimmed));
        }
    }

    /// <summary>
    /// Custom string-based Value Object for ISIN financial instrument identifiers.
    /// Decorated with <see cref="RegulatoryRuleAttribute"/> to trace back to ISO 6166.
    /// </summary>
    [RegulatoryRule("ISO-6166")]
    private sealed record IsinCode : StringValueObject<IsinCode>
    {
        private IsinCode(string value) : base(value) { }

        /// <summary>Creates a validated ISIN code (12-character alphanumeric, uppercase).</summary>
        public static Result<IsinCode> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<IsinCode>.Failure(Error.Validation("IsinCode.Required", "ISIN code is required."));

            string normalized = value.Trim().ToUpperInvariant();

            if (normalized.Length != 12)
                return Result<IsinCode>.Failure(Error.Validation("IsinCode.InvalidLength", "ISIN must be exactly 12 characters."));

            foreach (char c in normalized)
            {
                if (!char.IsLetterOrDigit(c))
                    return Result<IsinCode>.Failure(Error.Validation("IsinCode.InvalidChars", "ISIN must contain only letters and digits."));
            }

            return Result<IsinCode>.Success(new IsinCode(normalized));
        }
    }

    /// <summary>
    /// Custom string-based Value Object for project codes.
    /// Demonstrates StringValueObject pattern with uppercase normalization.
    /// </summary>
    [ValueObject(GenerateConversionOperators = false, GeneratePersistenceHooks = false)]
    private sealed record ProjectCode : StringValueObject<ProjectCode>
    {
        private ProjectCode(string value) : base(value) { }

        /// <summary>Creates a validated project code normalized to uppercase.</summary>
        public static Result<ProjectCode> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<ProjectCode>.Failure(Error.Validation("ProjectCode.Required", "Project code cannot be empty."));

            string normalized = value.Trim().ToUpperInvariant();
            if (!normalized.StartsWith("PRJ-", StringComparison.Ordinal))
                return Result<ProjectCode>.Failure(Error.Validation("ProjectCode.InvalidFormat", "Project code must start with 'PRJ-'."));

            return Result<ProjectCode>.Success(new ProjectCode(normalized));
        }
    }

    /// <summary>
    /// Custom composite Value Object for geographic coordinates.
    /// Demonstrates <see cref="ValueObject"/> base with multiple domain invariants.
    /// </summary>
    private sealed record GeoCoordinate : ValueObject
    {
        public double Latitude  { get; }
        public double Longitude { get; }

        private GeoCoordinate(double latitude, double longitude)
        {
            Latitude  = latitude;
            Longitude = longitude;
        }

        /// <summary>Creates a validated geo-coordinate within valid Earth bounds.</summary>
        public static Result<GeoCoordinate> Create(double latitude, double longitude)
        {
            if (latitude is < -90.0 or > 90.0)
                return Result<GeoCoordinate>.Failure(Error.Validation("GeoCoordinate.InvalidLatitude",  "Latitude must be between -90 and 90."));

            if (longitude is < -180.0 or > 180.0)
                return Result<GeoCoordinate>.Failure(Error.Validation("GeoCoordinate.InvalidLongitude", "Longitude must be between -180 and 180."));

            return Result<GeoCoordinate>.Success(new GeoCoordinate(latitude, longitude));
        }

        public override string ToString() => $"({Latitude:F4}, {Longitude:F4})";
    }

    // ═══════════════════════════════════════════════════════════════════════════════════
    // CUSTOM JSON CONVERTERS — Demonstración de extensión de los base converters
    // ═══════════════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Custom JSON converter for <see cref="ProjectCode"/> inheriting <see cref="StringValueObjectJsonConverter{TSelf}"/>.
    /// Only requires implementing <see cref="CreateInstance"/> — all read/write logic is inherited.
    /// </summary>
    private sealed class ProjectCodeJsonConverter : StringValueObjectJsonConverter<ProjectCode>
    {
        protected override Result<ProjectCode> CreateInstance(string value) => ProjectCode.Create(value);
    }
}
