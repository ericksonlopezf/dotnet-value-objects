// Copyright © Erickson Lopez. MIT License.
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EricksonLopez.ValueObjects.EntityFrameworkCore.IntegrationTests;

using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using Microsoft.EntityFrameworkCore;
using Xunit;

public sealed class ValueObjectEntityFrameworkCoreTests
{
    public sealed class TestCustomerEntity
    {
        public int Id { get; set; }
        public Email Email { get; set; }
        public PhoneNumber Phone { get; set; }
        public PostalCode PostalCode { get; set; } = default!;
        public CurrencyCode Currency { get; set; }
        public Percentage Discount { get; set; }
        public TaxRate TaxRate { get; set; }
        public Quantity Quantity { get; set; }
        public Cedula Cedula { get; set; } = default!;
        public Rnc Rnc { get; set; } = default!;
        public Ncf Ncf { get; set; } = default!;
        public Rfc Rfc { get; set; }
        public Curp Curp { get; set; }
        public Cuit Cuit { get; set; }
        public Rut Rut { get; set; }
        public Ruc Ruc { get; set; }
    }

    public sealed class TestDbContext : DbContext
    {
        public DbSet<TestCustomerEntity> Customers => Set<TestCustomerEntity>();

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.ConfigureDomainValueObjects();
            configurationBuilder.Properties<Cedula>().HaveConversion<StringValueObjectValueConverter<Cedula>>();
            configurationBuilder.Properties<Rnc>().HaveConversion<StringValueObjectValueConverter<Rnc>>();
            configurationBuilder.Properties<Ncf>().HaveConversion<StringValueObjectValueConverter<Ncf>>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestCustomerEntity>(entity =>
            {
                entity.Property(e => e.Rfc).HasConversion(v => v.Value, p => Rfc.Create(p).Value);
                entity.Property(e => e.Curp).HasConversion(v => v.Value, p => Curp.Create(p).Value);
                entity.Property(e => e.Cuit).HasConversion(v => v.Value, p => Cuit.Create(p).Value);
                entity.Property(e => e.Rut).HasConversion(v => v.ToCanonicalString(), p => Rut.Create(p).Value);
                entity.Property(e => e.Ruc).HasConversion(v => v.Value, p => Ruc.Create(p).Value);
            });
        }
    }

    [Fact]
    public async Task SaveAndRetrieve_ValueObjects_PersistsCorrectly()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var email = Email.Create("elopez@example.com").Value;
        var phone = PhoneNumber.Create("+18095551234").Value;
        var postalCode = PostalCode.Create("10101").Value;
        var currency = CurrencyCode.Create("USD").Value;
        var discount = Percentage.Create(15.5m).Value;
        var taxRate = TaxRate.Create(18.0m).Value;
        var quantity = Quantity.Create(50).Value;
        var cedula = Cedula.Create("00112345673").Value;
        var rnc = Rnc.Create("131880738").Value;
        var ncf = Ncf.Create("B0100000001").Value;
        var rfc = Rfc.Create("XAXX010101000").Value;
        var curp = Curp.Create("AAAA000000HDFRRR00").Value;
        var cuit = Cuit.Create("20123456786").Value;
        var rut = Rut.Create("12345678-5").Value;
        var ruc = Ruc.Create("20100070970").Value;

        var entity = new TestCustomerEntity
        {
            Id = 1,
            Email = email,
            Phone = phone,
            PostalCode = postalCode,
            Currency = currency,
            Discount = discount,
            TaxRate = taxRate,
            Quantity = quantity,
            Cedula = cedula,
            Rnc = rnc,
            Ncf = ncf,
            Rfc = rfc,
            Curp = curp,
            Cuit = cuit,
            Rut = rut,
            Ruc = ruc
        };

        // Act - Save
        await using (var context = new TestDbContext(options))
        {
            context.Customers.Add(entity);
            await context.SaveChangesAsync(ct);
        }

        // Act & Assert - Retrieve
        await using (var context = new TestDbContext(options))
        {
            var loaded = await context.Customers.FirstOrDefaultAsync(c => c.Id == 1, ct);

            loaded.Should().NotBeNull();
            loaded!.Email.Value.Should().Be("elopez@example.com");
            loaded.Phone.Value.Should().Be("+18095551234");
            loaded.PostalCode.Value.Should().Be("10101");
            loaded.Currency.Value.Should().Be("USD");
            loaded.Discount.Value.Should().Be(15.5m);
            loaded.TaxRate.Value.Should().Be(18.0m);
            loaded.Quantity.Value.Should().Be(50);
            loaded.Cedula.Value.Should().Be("00112345673");
            loaded.Rnc.Value.Should().Be("131880738");
            loaded.Ncf.Value.Should().Be("B0100000001");
            loaded.Rfc.Value.Should().Be("XAXX010101000");
            loaded.Curp.Value.Should().Be("AAAA000000HDFRRR00");
            loaded.Cuit.Value.Should().Be("20123456786");
            loaded.Rut.ToCanonicalString().Should().Be("12345678-5");
            loaded.Ruc.Value.Should().Be("20100070970");
        }
    }
}






