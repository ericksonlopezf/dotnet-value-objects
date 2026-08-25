// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;
using Result = global::EricksonLopez.Result.Result;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class AbstractionsFunctionalTests
{
    // ──────────────────────────────────────────────────────────────────
    // Result & Result<T> Tests
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Success_WhenCalledNonGeneric_ReturnsSuccessfulResult()
    {
        var result = global::EricksonLopez.Result.Result.Success();
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Failure_WhenCalledNonGeneric_ReturnsFailedResult()
    {
        var error = Error.Validation("Test.Code", "Test message");
        var result = global::EricksonLopez.Result.Result.Failure(error);
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Test.Code");
    }

    [Fact]
    public void Success_WhenGenericWithValue_ContainsValue()
    {
        var result = Result<int>.Success(42);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Failure_WhenGenericWithError_ContainsError()
    {
        var error = Error.Validation("Field.Required", "Field is required.");
        var result = Result<int>.Failure(error);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Field.Required");
    }

    [Fact]
    public void Map_WhenResultIsSuccess_TransformsValue()
    {
        var result = Result<int>.Success(42);
        var mapped = result.Map(v => v.ToString(CultureInfo.InvariantCulture));
        mapped.IsSuccess.Should().BeTrue();
        mapped.Value.Should().Be("42");
    }

    [Fact]
    public void Map_WhenResultIsFailure_PropagatesError()
    {
        var error = Error.Validation("Test.Error", "Something failed.");
        var result = Result<int>.Failure(error);
        var mapped = result.Map(v => v.ToString(CultureInfo.InvariantCulture));
        mapped.IsFailure.Should().BeTrue();
        mapped.Error.Code.Should().Be("Test.Error");
    }

    [Fact]
    public void Bind_WhenResultIsSuccess_ChainsOperation()
    {
        var result = Result<int>.Success(42);
        var bound = result.Bind(v => Result<string>.Success($"Value is {v}"));
        bound.IsSuccess.Should().BeTrue();
        bound.Value.Should().Be("Value is 42");
    }

    [Fact]
    public void Bind_WhenInnerOperationFails_PropagatesInnerError()
    {
        var result = Result<int>.Success(42);
        var innerError = Error.Validation("Inner.Error", "Inner operation failed.");
        var bound = result.Bind<string>(_ => Result<string>.Failure(innerError));
        bound.IsFailure.Should().BeTrue();
        bound.Error.Code.Should().Be("Inner.Error");
    }

    [Fact]
    public void Bind_WhenOuterOperationFails_PropagatesOuterError()
    {
        var error = Error.Validation("Outer.Error", "Outer failed.");
        var result = Result<int>.Failure(error);
        var bound = result.Bind(v => Result<string>.Success($"Value is {v}"));
        bound.IsFailure.Should().BeTrue();
        bound.Error.Code.Should().Be("Outer.Error");
    }

    [Fact]
    public void Match_WhenResultEvaluated_ExecutesCorrectBranch()
    {
        var success = Result<int>.Success(10);
        var res1 = success.Match(v => v * 2, _ => 0);
        res1.Should().Be(20);

        var failure = Result<int>.Failure(Error.Validation("E", "Fail"));
        var res2 = failure.Match(_ => 100, e => e.Code.Length);
        res2.Should().Be(1);
    }

    // ──────────────────────────────────────────────────────────────────
    // Error & ErrorType Tests
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Validation_WhenCreated_HasValidationType()
    {
        var error = Error.Validation("Field.Required", "Field is required.");
        error.Type.Should().Be(ErrorType.Validation);
        error.Code.Should().Be("Field.Required");
        error.Description.Should().Be("Field is required.");
    }

    [Fact]
    public void NotFound_WhenCreated_HasNotFoundType()
    {
        var error = Error.NotFound("Entity.NotFound", "Entity was not found.");
        error.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public void Conflict_WhenCreated_HasConflictType()
    {
        var error = Error.Conflict("Entity.Duplicate", "Entity already exists.");
        error.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public void Unauthorized_WhenCreated_HasUnauthorizedType()
    {
        var error = Error.Unauthorized("Operation.Forbidden", "Not allowed.");
        error.Type.Should().Be(ErrorType.Unauthorized);
    }

    [Fact]
    public void Failure_WhenCreated_HasFailureType()
    {
        var error = Error.Failure("System.Error", "Unexpected error.");
        error.Type.Should().Be(ErrorType.Failure);
    }

    [Fact]
    public void ToString_WhenCalledOnError_IncludesCode()
    {
        var error = Error.Validation("Test.Code", "Test description.");
        error.ToString().Should().Contain("Test.Code");
    }

    [Fact]
    public void Constructors_WhenMaskSpecifiedOrOmitted_SetsMaskProperly()
    {
        var defaultAttr = new SensitiveDataAttribute();
        defaultAttr.Mask.Should().Be("***");

        var customAttr = new SensitiveDataAttribute("[CUSTOM-MASK]");
        customAttr.Mask.Should().Be("[CUSTOM-MASK]");
    }

    // ──────────────────────────────────────────────────────────────────
    // NumericValidation Tests
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void IsScaleAtMost_WhenEvaluatingDecimals_CalculatesBitwiseAccuracy()
    {
        NumericValidation.IsScaleAtMost(100.12m, 2).Should().BeTrue();
        NumericValidation.IsScaleAtMost(100.123m, 2).Should().BeFalse();
        NumericValidation.IsScaleAtMost(100.123456m, 6).Should().BeTrue();
        NumericValidation.IsScaleAtMost(100.1234567m, 6).Should().BeFalse();
    }

    // ──────────────────────────────────────────────────────────────────
    // IParsable & ISpanParsable Tests on Core Scalar VOs
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void ParseAndTryParse_WhenCurrencyCodeProvided_ParsesCorrectly()
    {
        CurrencyCode.Parse("USD", CultureInfo.InvariantCulture).Should().Be(CurrencyCode.USD);
        CurrencyCode.Parse("EUR".AsSpan(), CultureInfo.InvariantCulture).Should().Be(CurrencyCode.EUR);

        CurrencyCode.TryParse("GBP", null, out var gbp).Should().BeTrue();
        gbp.Should().Be(CurrencyCode.GBP);

        CurrencyCode.TryParse("JPY".AsSpan(), null, out var jpy).Should().BeTrue();
        jpy.Should().Be(CurrencyCode.JPY);

        CurrencyCode.TryParse("INVALID", null, out _).Should().BeFalse();
        Action act = () => CurrencyCode.Parse("INVALID", CultureInfo.InvariantCulture);
        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void ParseAndMasked_WhenEmailProvided_ParsesAndMasksCorrectly()
    {
        var email = Email.Parse("john.doe@example.com", CultureInfo.InvariantCulture);
        email.Value.Should().Be("john.doe@example.com");
        email.Masked().Should().Be("j***@example.com");

        Email.Parse("jane@example.com".AsSpan(), CultureInfo.InvariantCulture).Value.Should().Be("jane@example.com");

        Email.TryParse("info@corp.com", null, out var parsed).Should().BeTrue();
        parsed.Value.Should().Be("info@corp.com");

        Email.TryParse("invalid-email", null, out _).Should().BeFalse();
        Action act = () => Email.Parse("invalid-email", CultureInfo.InvariantCulture);
        act.Should().Throw<FormatException>();

        default(Email).Masked().Should().Be(string.Empty);
        Email.Create("a@b.com").Value.Masked().Should().Be("***@b.com");
    }

    [Fact]
    public void ParseAndTryParse_WhenPhoneNumberProvided_ParsesCorrectly()
    {
        var phone = PhoneNumber.Parse("+18095551234", CultureInfo.InvariantCulture);
        phone.Value.Should().Be("+18095551234");

        PhoneNumber.Parse("+18095555678".AsSpan(), CultureInfo.InvariantCulture).Value.Should().Be("+18095555678");

        PhoneNumber.TryParse("+18095559999", null, out var parsed).Should().BeTrue();
        parsed.Value.Should().Be("+18095559999");

        PhoneNumber.TryParse("12345", null, out _).Should().BeFalse();
        Action act = () => PhoneNumber.Parse("invalid", CultureInfo.InvariantCulture);
        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void ParseAndTryFormat_WhenPercentageProvided_ParsesAndFormatsCorrectly()
    {
        var p1 = Percentage.Parse("18%", CultureInfo.InvariantCulture);
        p1.Value.Should().Be(18m);

        var p2 = Percentage.Parse("18.5".AsSpan(), CultureInfo.InvariantCulture);
        p2.Value.Should().Be(18.5m);

        Percentage.TryParse("25%", null, out var p3).Should().BeTrue();
        p3.Value.Should().Be(25m);

        Percentage.TryParse("150%", null, out _).Should().BeFalse();
        Percentage.TryParse("invalid", null, out _).Should().BeFalse();
        Percentage.TryParse("", null, out _).Should().BeFalse();

        Action act = () => Percentage.Parse("200%", CultureInfo.InvariantCulture);
        act.Should().Throw<FormatException>();
        Action actInvalid = () => Percentage.Parse("abc", CultureInfo.InvariantCulture);
        actInvalid.Should().Throw<FormatException>();

        // Formatting
        p1.ToString("0.00", CultureInfo.InvariantCulture).Should().Be("18.00%");
        Span<char> span = stackalloc char[10];
        p1.TryFormat(span, out int written, "0.0", CultureInfo.InvariantCulture).Should().BeTrue();
        span[..written].ToString().Should().Be("18.0%");

        Span<char> smallSpan = stackalloc char[2];
        p1.TryFormat(smallSpan, out int writtenSmall, "0.0", CultureInfo.InvariantCulture).Should().BeFalse();
        writtenSmall.Should().Be(0);
    }

    [Fact]
    public void ParseAndTryFormat_WhenTaxRateProvided_ParsesAndFormatsCorrectly()
    {
        var tr1 = TaxRate.Parse("18%", CultureInfo.InvariantCulture);
        tr1.Value.Should().Be(18m);

        var tr2 = TaxRate.Parse("21".AsSpan(), CultureInfo.InvariantCulture);
        tr2.Value.Should().Be(21m);

        TaxRate.TryParse("16%", null, out var tr3).Should().BeTrue();
        tr3.Value.Should().Be(16m);

        TaxRate.TryParse("150%", null, out _).Should().BeFalse();
        TaxRate.TryParse("", null, out _).Should().BeFalse();

        Action act = () => TaxRate.Parse("invalid", CultureInfo.InvariantCulture);
        act.Should().Throw<FormatException>();

        Span<char> span = stackalloc char[10];
        tr1.TryFormat(span, out int written, "0.0", CultureInfo.InvariantCulture).Should().BeTrue();
        span[..written].ToString().Should().Be("18.0%");

        Span<char> smallSpan = stackalloc char[2];
        tr1.TryFormat(smallSpan, out int writtenSmall, "0.0", CultureInfo.InvariantCulture).Should().BeFalse();
        writtenSmall.Should().Be(0);
    }

    [Fact]
    public void ParseAndTryFormat_WhenDiscountRateProvided_ParsesAndFormatsCorrectly()
    {
        var dr1 = DiscountRate.Parse("10%", CultureInfo.InvariantCulture);
        dr1.Value.Should().Be(10m);

        var dr2 = DiscountRate.Parse("15".AsSpan(), CultureInfo.InvariantCulture);
        dr2.Value.Should().Be(15m);

        DiscountRate.TryParse("5%", null, out var dr3).Should().BeTrue();
        dr3.Value.Should().Be(5m);

        DiscountRate.TryParse("150%", null, out _).Should().BeFalse();
        DiscountRate.TryParse("", null, out _).Should().BeFalse();

        Action act = () => DiscountRate.Parse("invalid", CultureInfo.InvariantCulture);
        act.Should().Throw<FormatException>();

        Span<char> span = stackalloc char[10];
        dr1.TryFormat(span, out int written, "0.0", CultureInfo.InvariantCulture).Should().BeTrue();
        span[..written].ToString().Should().Be("10.0%");

        Span<char> smallSpan = stackalloc char[2];
        dr1.TryFormat(smallSpan, out int writtenSmall, "0.0", CultureInfo.InvariantCulture).Should().BeFalse();
        writtenSmall.Should().Be(0);
    }

    [Fact]
    public void ParseAndTryFormat_WhenQuantityProvided_ParsesAndFormatsCorrectly()
    {
        var q1 = Quantity.Parse("50", CultureInfo.InvariantCulture);
        q1.Value.Should().Be(50);

        var q2 = Quantity.Parse("100".AsSpan(), CultureInfo.InvariantCulture);
        q2.Value.Should().Be(100);

        Quantity.TryParse("25", null, out var q3).Should().BeTrue();
        q3.Value.Should().Be(25);

        Quantity.TryParse("-5", null, out _).Should().BeFalse();
        Quantity.TryParse("invalid", null, out _).Should().BeFalse();

        Action act = () => Quantity.Parse("-10", CultureInfo.InvariantCulture);
        act.Should().Throw<FormatException>();
        Action actInvalid = () => Quantity.Parse("invalid", CultureInfo.InvariantCulture);
        actInvalid.Should().Throw<FormatException>();

        Action actSpan = () => Quantity.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        actSpan.Should().Throw<FormatException>();

        Span<char> span = stackalloc char[10];
        q1.TryFormat(span, out int written, "D3", CultureInfo.InvariantCulture).Should().BeTrue();
        span[..written].ToString().Should().Be("050");
    }

    [Fact]
    public void ParseAndTryFormat_WhenBusinessDateProvided_ParsesAndFormatsCorrectly()
    {
        var bd1 = BusinessDate.Parse("2026-08-16", CultureInfo.InvariantCulture);
        bd1.Value.Should().Be(new DateOnly(2026, 8, 16));

        var bd2 = BusinessDate.Parse("2026-12-31".AsSpan(), CultureInfo.InvariantCulture);
        bd2.Value.Should().Be(new DateOnly(2026, 12, 31));

        BusinessDate.TryParse("2026-01-01", null, out var bd3).Should().BeTrue();
        bd3.Value.Should().Be(new DateOnly(2026, 1, 1));

        BusinessDate.TryParse("invalid", null, out _).Should().BeFalse();
        Action act = () => BusinessDate.Parse("invalid", CultureInfo.InvariantCulture);
        act.Should().Throw<FormatException>();

        Action actSpan = () => BusinessDate.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        actSpan.Should().Throw<FormatException>();

        Span<char> span = stackalloc char[20];
        bd1.TryFormat(span, out int written, "yyyy/MM/dd", CultureInfo.InvariantCulture).Should().BeTrue();
        span[..written].ToString().Should().Be("2026/08/16");
    }

    [Fact]
    public void TryFormat_WhenMoneyFormatted_SupportsCustomFormatAndSpan()
    {
        var money = Money.Create(1250.50m, CurrencyCode.USD).Value;
        money.ToString("C", CultureInfo.InvariantCulture).Should().Contain("1,250.50");

        Span<char> span = stackalloc char[30];
        money.TryFormat(span, out int written, "N2", CultureInfo.InvariantCulture).Should().BeTrue();
        span[..written].ToString().Should().Be("1,250.50 USD");

        Span<char> smallSpan = stackalloc char[3];
        money.TryFormat(smallSpan, out int writtenSmall, "N2", CultureInfo.InvariantCulture).Should().BeFalse();
        writtenSmall.Should().Be(0);
    }
}



