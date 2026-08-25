// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EricksonLopez.ValueObjects.EntityFrameworkCore.IntegrationTests;

using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using Microsoft.EntityFrameworkCore;
using Xunit;

public sealed class ValueObjectModelConfigurationExtensionsTests
{
    private sealed class TestDomainEntity
    {
        public int Id { get; set; }
        public Email Email { get; set; }
        public PhoneNumber Phone { get; set; }
        public PostalCode PostalCode { get; set; } = default!;
        public CurrencyCode Currency { get; set; }
        public Percentage Percentage { get; set; }
        public TaxRate TaxRate { get; set; }
        public Quantity Quantity { get; set; }
    }

    private sealed class TestFiscalEntity
    {
        public int Id { get; set; }
        public Cedula Cedula { get; set; } = default!;
    }

    private sealed class TestModelDbContext : DbContext
    {
        public DbSet<TestDomainEntity> DomainEntities => Set<TestDomainEntity>();
        public DbSet<TestFiscalEntity> FiscalEntities => Set<TestFiscalEntity>();

        public TestModelDbContext(DbContextOptions<TestModelDbContext> options)
            : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.ConfigureDomainValueObjects();
            configurationBuilder.Properties<Cedula>().HaveConversion<StringValueObjectValueConverter<Cedula>>();
        }
    }

    private static TestModelDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestModelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableServiceProviderCaching(false)
            .Options;
        return new TestModelDbContext(options);
    }

    [Fact]
    public void ConfigureDomainValueObjects_NullConfigurationBuilder_ThrowsArgumentNullException()
    {
        ModelConfigurationBuilder builder = null!;
        Action act = () => builder.ConfigureDomainValueObjects();
        act.Should().Throw<ArgumentNullException>().WithParameterName("configurationBuilder");
    }

    [Fact]
    public void ConfigureDomainValueObjects_Email_ConfiguresConverter()
    {
        using var context = CreateContext();
        var prop = context.Model.FindEntityType(typeof(TestDomainEntity))?.FindProperty(nameof(TestDomainEntity.Email));
        prop.Should().NotBeNull();
        prop!.GetValueConverter().Should().NotBeNull().And.BeOfType<EmailValueConverter>();
    }

    [Fact]
    public void ConfigureDomainValueObjects_Phone_ConfiguresConverter()
    {
        using var context = CreateContext();
        var prop = context.Model.FindEntityType(typeof(TestDomainEntity))?.FindProperty(nameof(TestDomainEntity.Phone));
        prop.Should().NotBeNull();
        prop!.GetValueConverter().Should().NotBeNull().And.BeOfType<PhoneNumberValueConverter>();
    }

    [Fact]
    public void ConfigureDomainValueObjects_PostalCode_ConfiguresConverter()
    {
        using var context = CreateContext();
        var prop = context.Model.FindEntityType(typeof(TestDomainEntity))?.FindProperty(nameof(TestDomainEntity.PostalCode));
        prop.Should().NotBeNull();
        prop!.GetValueConverter().Should().NotBeNull().And.BeOfType<PostalCodeValueConverter>();
    }

    [Fact]
    public void ConfigureDomainValueObjects_Currency_ConfiguresConverter()
    {
        using var context = CreateContext();
        var prop = context.Model.FindEntityType(typeof(TestDomainEntity))?.FindProperty(nameof(TestDomainEntity.Currency));
        prop.Should().NotBeNull();
        prop!.GetValueConverter().Should().NotBeNull().And.BeOfType<CurrencyCodeValueConverter>();
    }

    [Fact]
    public void ConfigureDomainValueObjects_Percentage_ConfiguresConverter()
    {
        using var context = CreateContext();
        var prop = context.Model.FindEntityType(typeof(TestDomainEntity))?.FindProperty(nameof(TestDomainEntity.Percentage));
        prop.Should().NotBeNull();
        prop!.GetValueConverter().Should().NotBeNull().And.BeOfType<PercentageValueConverter>();
    }

    [Fact]
    public void ConfigureDomainValueObjects_TaxRate_ConfiguresConverter()
    {
        using var context = CreateContext();
        var prop = context.Model.FindEntityType(typeof(TestDomainEntity))?.FindProperty(nameof(TestDomainEntity.TaxRate));
        prop.Should().NotBeNull();
        prop!.GetValueConverter().Should().NotBeNull().And.BeOfType<TaxRateValueConverter>();
    }

    [Fact]
    public void ConfigureDomainValueObjects_Quantity_ConfiguresConverter()
    {
        using var context = CreateContext();
        var prop = context.Model.FindEntityType(typeof(TestDomainEntity))?.FindProperty(nameof(TestDomainEntity.Quantity));
        prop.Should().NotBeNull();
        prop!.GetValueConverter().Should().NotBeNull().And.BeOfType<QuantityValueConverter>();
    }

    [Fact]
    public void GenericStringValueObjectValueConverter_DefaultState_ConfiguresConverterForFiscalVO()
    {
        using var context = CreateContext();
        var prop = context.Model.FindEntityType(typeof(TestFiscalEntity))?.FindProperty(nameof(TestFiscalEntity.Cedula));
        prop.Should().NotBeNull();
        prop!.GetValueConverter().Should().NotBeNull().And.BeOfType<StringValueObjectValueConverter<Cedula>>();
    }
}




