// Copyright © Erickson Lopez. MIT License.
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EricksonLopez.ValueObjects.EntityFrameworkCore.IntegrationTests;

using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

/// <summary>
/// Integration tests verifying that Value Objects seamlessly map, persist, and query
/// against a real relational database engine (SQLite In-Memory).
/// </summary>
public sealed class ValueObjectSqliteEntityFrameworkCoreTests
{
    public sealed class SqliteCustomerEntity
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

    public sealed class SqliteTestDbContext : DbContext
    {
        public DbSet<SqliteCustomerEntity> Customers => Set<SqliteCustomerEntity>();

        public SqliteTestDbContext(DbContextOptions<SqliteTestDbContext> options)
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
            modelBuilder.Entity<SqliteCustomerEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Rfc).HasConversion(v => v.Value, p => Rfc.Create(p).Value);
                entity.Property(e => e.Curp).HasConversion(v => v.Value, p => Curp.Create(p).Value);
                entity.Property(e => e.Cuit).HasConversion(v => v.Value, p => Cuit.Create(p).Value);
                entity.Property(e => e.Rut).HasConversion(v => v.ToCanonicalString(), p => Rut.Create(p).Value);
                entity.Property(e => e.Ruc).HasConversion(v => v.Value, p => Ruc.Create(p).Value);
            });
        }
    }

    [Fact]
    public async Task Sqlite_SaveAndQueryWithFilter_PersistsAndQueriesCorrectly()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync(ct);

        var options = new DbContextOptionsBuilder<SqliteTestDbContext>()
            .UseSqlite(connection)
            .Options;

        var email = Email.Create("sqlite.test@example.com").Value;
        var phone = PhoneNumber.Create("+18095559876").Value;
        var postalCode = PostalCode.Create("10102").Value;
        var currency = CurrencyCode.Create("USD").Value;
        var discount = Percentage.Create(10.0m).Value;
        var taxRate = TaxRate.Create(18.0m).Value;
        var quantity = Quantity.Create(25).Value;
        var cedula = Cedula.Create("00112345673").Value;
        var rnc = Rnc.Create("131880738").Value;
        var ncf = Ncf.Create("B0100000001").Value;
        var rfc = Rfc.Create("XAXX010101000").Value;
        var curp = Curp.Create("AAAA000000HDFRRR00").Value;
        var cuit = Cuit.Create("20123456786").Value;
        var rut = Rut.Create("12345678-5").Value;
        var ruc = Ruc.Create("20100070970").Value;

        var entity = new SqliteCustomerEntity
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

        // Act - Ensure schema and insert
        await using (var context = new SqliteTestDbContext(options))
        {
            await context.Database.EnsureCreatedAsync(ct);
            context.Customers.Add(entity);
            await context.SaveChangesAsync(ct);
        }

        // Act & Assert - Query with LINQ filters against SQLite
        await using (var context = new SqliteTestDbContext(options))
        {
            var loaded = await context.Customers.FirstOrDefaultAsync(c => c.Email == email, ct);

            loaded.Should().NotBeNull();
            loaded!.Email.Value.Should().Be("sqlite.test@example.com");
            loaded.Phone.Value.Should().Be("+18095559876");
            loaded.PostalCode.Value.Should().Be("10102");
            loaded.Currency.Value.Should().Be("USD");
            loaded.Discount.Value.Should().Be(10.0m);
            loaded.TaxRate.Value.Should().Be(18.0m);
            loaded.Quantity.Value.Should().Be(25);
            loaded.Cedula.Value.Should().Be("00112345673");
            loaded.Rnc.Value.Should().Be("131880738");
            loaded.Ncf.Value.Should().Be("B0100000001");
            loaded.Rfc.Value.Should().Be("XAXX010101000");
            loaded.Curp.Value.Should().Be("AAAA000000HDFRRR00");
            loaded.Cuit.Value.Should().Be("20123456786");
            loaded.Rut.ToCanonicalString().Should().Be("12345678-5");
            loaded.Ruc.Value.Should().Be("20100070970");

            // Update
            var updatedTaxRate = TaxRate.Create(16.0m).Value;
            loaded.TaxRate = updatedTaxRate;
            await context.SaveChangesAsync(ct);
        }

        // Verify update persisted
        await using (var context = new SqliteTestDbContext(options))
        {
            var reloaded = await context.Customers.FirstOrDefaultAsync(c => c.Id == 1, ct);
            reloaded.Should().NotBeNull();
            reloaded!.TaxRate.Value.Should().Be(16.0m);
        }
    }

    [Fact]
    public async Task SaveChangesAsync_WhenCancellationTokenIsCancelled_ThrowsOperationCanceledException()
    {
        // Arrange
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<SqliteTestDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var context = new SqliteTestDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            context.Customers.Add(new SqliteCustomerEntity
            {
                Id = 100,
                Email = Email.Create("cancelled@example.com").Value,
                Phone = PhoneNumber.Create("+18095551111").Value,
                PostalCode = PostalCode.Create("10101").Value,
                Currency = CurrencyCode.Create("USD").Value,
                Discount = Percentage.Create(5.0m).Value,
                TaxRate = TaxRate.Create(18.0m).Value,
                Quantity = Quantity.Create(1).Value,
                Cedula = Cedula.Create("00112345673").Value,
                Rnc = Rnc.Create("131880738").Value,
                Ncf = Ncf.Create("B0100000001").Value,
                Rfc = Rfc.Create("XAXX010101000").Value,
                Curp = Curp.Create("AAAA000000HDFRRR00").Value,
                Cuit = Cuit.Create("20123456786").Value,
                Rut = Rut.Create("12345678-5").Value,
                Ruc = Ruc.Create("20100070970").Value
            });

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            Func<Task> act = async () => await context.SaveChangesAsync(cts.Token);

            // Assert
            await act.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}







