// Copyright © Erickson Lopez. MIT License.
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Dapper;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Dapper;
using Microsoft.Data.Sqlite;
using Xunit;

namespace EricksonLopez.ValueObjects.Dapper.IntegrationTests;

/// <summary>
/// Integration tests verifying end-to-end Dapper persistence, parameterization, and query materialization
/// with Value Objects against a real SQLite In-Memory database engine.
/// </summary>
public sealed class ValueObjectDapperSqliteIntegrationTests
{
    static ValueObjectDapperSqliteIntegrationTests()
    {
        // Register Dapper TypeHandlers for Value Objects under test
        ValueObjectTypeHandler.RegisterStruct<Email, string>(static s => Email.Create(s), static e => e.Value);
        ValueObjectTypeHandler.RegisterStruct<Quantity, int>(static i => Quantity.Create(i), static q => q.Value);
        ValueObjectTypeHandler.RegisterStruct<Percentage, decimal>(static d => Percentage.Create(d), static p => p.Value);
        ValueObjectTypeHandler.Register<Comment, string>(static s => Comment.Create(s));
    }

    public sealed class TenantRecord
    {
        public int Id { get; set; }
        public Email ContactEmail { get; set; }
        public Quantity BaseQuantity { get; set; }
        public Percentage DiscountPercentage { get; set; }
        public Comment? comments { get; set; }
    }

    private static async Task<SqliteConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync(cancellationToken);

        const string createTableSql = """
            CREATE TABLE Tenants (
                Id INTEGER PRIMARY KEY,
                ContactEmail TEXT NOT NULL,
                BaseQuantity NUMERIC NOT NULL,
                DiscountPercentage NUMERIC NOT NULL,
                comments TEXT NULL
            );
            """;

        await connection.ExecuteAsync(new CommandDefinition(createTableSql, cancellationToken: cancellationToken));
        return connection;
    }

    [Fact]
    public async Task ExecuteAndQuery_WhenValueObjectsProvided_PersistsAndMaterializesCorrectly()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await CreateOpenConnectionAsync(ct);

        var contactEmail = Email.Create("admin@acme.corp").Value;
        var baseQuantity = Quantity.Create(1500).Value;
        var discountPercentage = Percentage.Create(15.5m).Value;
        var comments = Comment.Create("Primary production tenant").Value;

        const string insertSql = """
            INSERT INTO Tenants (Id, ContactEmail, BaseQuantity, DiscountPercentage, comments)
            VALUES (@Id, @ContactEmail, @BaseQuantity, @DiscountPercentage, @comments);
            """;

        // Act - Insert with Dapper parameters
        var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(insertSql, new
        {
            Id = 1,
            ContactEmail = contactEmail,
            BaseQuantity = baseQuantity,
            DiscountPercentage = discountPercentage,
            comments = comments
        }, cancellationToken: ct));

        rowsAffected.Should().Be(1);

        // Act - Query back
        const string selectSql = "SELECT Id, ContactEmail, BaseQuantity, DiscountPercentage, comments FROM Tenants WHERE Id = @Id;";
        var tenant = await connection.QuerySingleAsync<TenantRecord>(new CommandDefinition(selectSql, new { Id = 1 }, cancellationToken: ct));

        // Assert
        tenant.Should().NotBeNull();
        tenant.Id.Should().Be(1);
        tenant.ContactEmail.Should().Be(contactEmail);
        tenant.ContactEmail.Value.Should().Be("admin@acme.corp");
        tenant.BaseQuantity.Should().Be(baseQuantity);
        tenant.BaseQuantity.Value.Should().Be(1500);
        tenant.DiscountPercentage.Should().Be(discountPercentage);
        tenant.DiscountPercentage.Value.Should().Be(15.5m);
        tenant.comments.Should().NotBeNull();
        tenant.comments!.Value.Should().Be("Primary production tenant");
    }

    [Fact]
    public async Task ExecuteAndQuery_WithNullableValueObject_WhenDatabaseColumnIsNull_ReturnsNull()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await CreateOpenConnectionAsync(ct);

        var contactEmail = Email.Create("beta@test.corp").Value;
        var baseQuantity = Quantity.Create(20).Value;
        var discountPercentage = Percentage.Create(0m).Value;

        const string insertSql = """
            INSERT INTO Tenants (Id, ContactEmail, BaseQuantity, DiscountPercentage, comments)
            VALUES (@Id, @ContactEmail, @BaseQuantity, @DiscountPercentage, @comments);
            """;

        await connection.ExecuteAsync(new CommandDefinition(insertSql, new
        {
            Id = 2,
            ContactEmail = contactEmail,
            BaseQuantity = baseQuantity,
            DiscountPercentage = discountPercentage,
            comments = (Comment?)null
        }, cancellationToken: ct));

        // Act
        const string selectSql = "SELECT Id, ContactEmail, BaseQuantity, DiscountPercentage, comments FROM Tenants WHERE Id = @Id;";
        var tenant = await connection.QuerySingleAsync<TenantRecord>(new CommandDefinition(selectSql, new { Id = 2 }, cancellationToken: ct));

        // Assert
        tenant.Should().NotBeNull();
        tenant.Id.Should().Be(2);
        tenant.ContactEmail.Should().Be(contactEmail);
        tenant.comments.Should().BeNull();
    }

    [Fact]
    public async Task Query_WhenDatabaseContainsInvalidValue_ThrowsDataException()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await CreateOpenConnectionAsync(ct);

        // Insert invalid email directly via raw SQL
        const string rawInsertSql = """
            INSERT INTO Tenants (Id, ContactEmail, BaseQuantity, DiscountPercentage, comments)
            VALUES (99, 'INVALID--EMAIL@@.com..', 10, 5, NULL);
            """;

        await connection.ExecuteAsync(new CommandDefinition(rawInsertSql, cancellationToken: ct));

        // Act & Assert
        const string selectSql = "SELECT Id, ContactEmail, BaseQuantity, DiscountPercentage, comments FROM Tenants WHERE Id = 99;";
        Func<Task> act = async () => await connection.QuerySingleAsync<TenantRecord>(new CommandDefinition(selectSql, cancellationToken: ct));

        var assertion = await act.Should().ThrowAsync<DataException>();
        assertion.Which.Message.Should().Contain("Error parsing column");
        assertion.WithInnerException<DataException>()
            .WithMessage("*Failed to map database value 'INVALID--EMAIL@@.com..' to 'Email'*");
    }

    [Fact]
    public async Task Query_WithFilteringOnValueObjects_ReturnsMatchingRows()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await CreateOpenConnectionAsync(ct);

        var emailA = Email.Create("alpha@corp.com").Value;
        var emailB = Email.Create("beta@corp.com").Value;
        var qtyA = Quantity.Create(100).Value;
        var qtyB = Quantity.Create(200).Value;

        await connection.ExecuteAsync(new CommandDefinition("""
            INSERT INTO Tenants (Id, ContactEmail, BaseQuantity, DiscountPercentage, comments) VALUES
            (10, @EmailA, @QtyA, 10, NULL),
            (20, @EmailB, @QtyB, 20, NULL);
            """, new
        {
            EmailA = emailA,
            QtyA = qtyA,
            EmailB = emailB,
            QtyB = qtyB
        }, cancellationToken: ct));

        // Act - Query with VO filter parameters
        const string filterSql = "SELECT Id, ContactEmail, BaseQuantity, DiscountPercentage, comments FROM Tenants WHERE ContactEmail = @ContactEmail AND BaseQuantity = @BaseQuantity;";
        var result = await connection.QuerySingleOrDefaultAsync<TenantRecord>(new CommandDefinition(filterSql, new
        {
            ContactEmail = emailB,
            BaseQuantity = qtyB
        }, cancellationToken: ct));

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(20);
        result.ContactEmail.Should().Be(emailB);
        result.BaseQuantity.Should().Be(qtyB);
    }

    [Fact]
    public async Task Query_WhenCancellationTokenCancelled_ThrowsOperationCanceledException()
    {
        // Arrange
        await using var connection = await CreateOpenConnectionAsync(TestContext.Current.CancellationToken);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        const string selectSql = "SELECT * FROM Tenants;";
        Func<Task> act = async () => await connection.QueryAsync<TenantRecord>(new CommandDefinition(selectSql, cancellationToken: cts.Token));

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
