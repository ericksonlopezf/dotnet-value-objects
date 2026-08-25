// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.Json;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Dapper;
using EricksonLopez.ValueObjects.EntityFrameworkCore;
using EricksonLopez.ValueObjects.Serialization.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EricksonLopez.ValueObjects.Samples.Levels;

/// <summary>
/// Level 09: Persistence, Serialization, and Infrastructure Integration.
/// Demonstrates System.Text.Json converters (RangeJsonConverter, StringValueObjectJsonConverter),
/// Dapper TypeHandlers for both reference and struct VOs (Register + RegisterStruct),
/// and Entity Framework Core 10 Value Converters
/// (ConfigureDomainValueObjects, SingleValueObjectValueConverter, StringValueObjectValueConverter).
/// </summary>
public static class Level09_PersistenceAndSerialization
{
    /// <summary>
    /// Executes the persistence and serialization demonstrations.
    /// </summary>
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n===============================================================================");
        Console.WriteLine(" [LEVEL 9] INFRASTRUCTURE: JSON SERIALIZATION, DAPPER, AND EF CORE 10");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        // ─── 1. JSON SERIALIZATION WITH SYSTEM.TEXT.JSON (AOT-FRIENDLY) ────────────────
        Console.WriteLine("[1. System.Text.Json Native Serialization]");
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        jsonOptions.Converters.Add(new RangeJsonConverter<DateOnly>());

        var dateRange = Range<DateOnly>.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31)).Value;
        string json = JsonSerializer.Serialize(dateRange, jsonOptions);
        Console.WriteLine($"  - Serialized Range<DateOnly>:\n{json}");

        var deserializedRange = JsonSerializer.Deserialize<Range<DateOnly>>(json, jsonOptions);
        Console.WriteLine($"  - Successfully deserialized: [{deserializedRange.Start} .. {deserializedRange.End}]");

        // ─── 2. DAPPER TYPE HANDLERS — Reference VOs (StringValueObject) ───────────────
        Console.WriteLine("\n[2. Dapper TypeHandler.Register<TVO, TPrimitive>() — Reference VOs]");
        // Register<TVO, TPrimitive>(Func<TPrimitive, Result<TVO>>) — for SingleValueObject (records)
        ValueObjectTypeHandler.Register<TenantCode, string>(TenantCode.Create);
        ValueObjectTypeHandler.Register<PostalCode, string>(PostalCode.Create);
        Console.WriteLine("  - Register<TenantCode, string> ✔");
        Console.WriteLine("  - Register<PostalCode, string> ✔");

        // ─── 3. DAPPER TYPE HANDLERS — Struct VOs (RegisterStruct) ────────────────────
        Console.WriteLine("\n[3. Dapper TypeHandler.RegisterStruct<TVO, TPrimitive>() — Struct VOs]");
        // RegisterStruct requires an additional valueSelector because structs cannot be null.
        // Signature: RegisterStruct<TVO, TPrimitive>(Func<TPrimitive, Result<TVO>>, Func<TVO, TPrimitive>)
        ValueObjectTypeHandler.RegisterStruct<TaxRate, decimal>(
            factory:       TaxRate.Create,
            valueSelector: vo => vo.Value);

        ValueObjectTypeHandler.RegisterStruct<Quantity, int>(
            factory:       val => Quantity.Create(int.Parse(val.ToString()!)),
            valueSelector: vo => vo.Value);

        Console.WriteLine("  - RegisterStruct<TaxRate, decimal>  ✔");
        Console.WriteLine("  - RegisterStruct<Quantity, int>      ✔");

        // ─── 4. ENTITY FRAMEWORK CORE 10 — ConfigureDomainValueObjects ────────────────
        Console.WriteLine("\n[4. Entity Framework Core 10 — ConfigureDomainValueObjects()]");
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var dbOptions = new DbContextOptionsBuilder<SampleAppDbContext>()
            .UseSqlite(connection)
            .Options;

        using (var context = new SampleAppDbContext(dbOptions))
        {
            context.Database.EnsureCreated();

            var sampleOrg = new SampleOrganization
            {
                Id              = Guid.NewGuid(),
                Name            = "EricksonLopez Corp",
                BillingEmail    = Email.Create("billing@ericksonlopez.dev").Value,
                SupportPhone    = PhoneNumber.Create("+1-809-555-0100").Value,
                VatRate         = TaxRate.Create(18.0m).Value,
                DefaultCurrency = CurrencyCode.Create("USD").Value
            };

            context.Organizations.Add(sampleOrg);
            context.SaveChanges();
        }

        using (var readContext = new SampleAppDbContext(dbOptions))
        {
            var savedOrg = readContext.Organizations.FirstAsync().GetAwaiter().GetResult();
            Console.WriteLine($"  - Read from SQLite in-memory database:");
            Console.WriteLine($"    Company   : {savedOrg.Name}");
            Console.WriteLine($"    Email VO  : {savedOrg.BillingEmail.Value}");
            Console.WriteLine($"    Phone VO  : {savedOrg.SupportPhone.Value}");
            Console.WriteLine($"    TaxRate VO: {savedOrg.VatRate}");
            Console.WriteLine($"    Currency  : {savedOrg.DefaultCurrency}");
        }

        // ─── 5. EF CORE — SingleValueObjectValueConverter — Direct Usage ───────────────
        Console.WriteLine("\n[5. EF Core — SingleValueObjectValueConverter<TVO, TValue> — Direct Usage]");
        Console.WriteLine("  - SingleValueObjectValueConverter<TVO,TValue> maps any SingleValueObject.");
        Console.WriteLine("  - Default constructor → resolves Create(TValue) via reflection.");
        Console.WriteLine("  - Factory constructor → resolves without reflection (AOT-safe):");

        // Example: map TaxRate manually without ConfigureDomainValueObjects
        var taxRateConverter = new TaxRateValueConverter();
        Console.WriteLine($"  - TaxRateValueConverter: {taxRateConverter.GetType().BaseType?.Name}");

        // Example: SingleValueObjectValueConverter with explicit factory
        var postalConverter = new SingleValueObjectValueConverter<PostalCode, string>(
            factory: raw => PostalCode.Create(raw).Value
        );
        Console.WriteLine($"  - SingleValueObjectValueConverter<PostalCode, string>: instantiated ✔");

        // ─── 6. EF CORE — StringValueObjectValueConverter — Direct Usage ───────────────
        Console.WriteLine("\n[6. EF Core — StringValueObjectValueConverter<TVO> — Direct Usage]");
        Console.WriteLine("  - StringValueObjectValueConverter<TVO> specializes in StringValueObject<TVO>.");
        Console.WriteLine("  - Default constructor → resolves Create(string) via reflection.");
        Console.WriteLine("  - Factory constructor → resolves without reflection (AOT-safe):");

        var tenantConverter = new StringValueObjectValueConverter<TenantCode>(
            factory: raw => TenantCode.Create(raw).Value
        );
        Console.WriteLine($"  - StringValueObjectValueConverter<TenantCode>: instantiated ✔");
        Console.WriteLine($"  - Base type: {tenantConverter.GetType().BaseType?.Name}");
    }

    /// <summary>
    /// Sample Entity Framework Core DbContext utilizing central ValueObject mappings.
    /// Demonstrates <see cref="ValueObjectModelConfigurationExtensions.ConfigureDomainValueObjects"/> convention.
    /// </summary>
    private sealed class SampleAppDbContext : DbContext
    {
        public DbSet<SampleOrganization> Organizations => Set<SampleOrganization>();

        public SampleAppDbContext(DbContextOptions<SampleAppDbContext> options) : base(options) { }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Registers all built-in EF Core ValueConverters for core domain Value Objects centrally:
            // Email, PhoneNumber, PostalCode, CurrencyCode, Percentage, TaxRate, Quantity
            configurationBuilder.ConfigureDomainValueObjects();
            base.ConfigureConventions(configurationBuilder);
        }
    }

    /// <summary>
    /// Sample domain entity with strongly-typed Value Objects.
    /// </summary>
    private sealed class SampleOrganization
    {
        public Guid Id              { get; set; }
        public string Name          { get; set; } = string.Empty;
        public Email BillingEmail   { get; set; }
        public PhoneNumber SupportPhone { get; set; }
        public TaxRate VatRate      { get; set; }
        public CurrencyCode DefaultCurrency { get; set; }
    }
}
