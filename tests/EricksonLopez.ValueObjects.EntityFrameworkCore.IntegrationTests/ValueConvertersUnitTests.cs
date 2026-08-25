// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xunit;

namespace EricksonLopez.ValueObjects.EntityFrameworkCore.IntegrationTests;

using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xunit;

public sealed class ValueConvertersUnitTests
{
    private sealed record TestIntVo : SingleValueObject<TestIntVo, int>
    {
        private TestIntVo(int value) : base(value) { }
        public static Result<TestIntVo> Create(int value) => Result<TestIntVo>.Success(new TestIntVo(value));
    }

    private sealed record TestIntVoWithNormalization : SingleValueObject<TestIntVoWithNormalization, int>
    {
        private TestIntVoWithNormalization(int value) : base(value) { }
        public static Result<TestIntVoWithNormalization> Create(int value) =>
            Result<TestIntVoWithNormalization>.Success(new TestIntVoWithNormalization(value + 1000));
    }

    private sealed record TestIntVoWithFailure : SingleValueObject<TestIntVoWithFailure, int>
    {
        private TestIntVoWithFailure(int value) : base(value) { }
        public static Result<TestIntVoWithFailure> Create(int value) =>
            value > 0
                ? Result<TestIntVoWithFailure>.Success(new TestIntVoWithFailure(value))
                : Result<TestIntVoWithFailure>.Failure(Error.Validation("Invalid", "Value must be positive."));
    }

    private sealed record TestIntVoDirectReturn : SingleValueObject<TestIntVoDirectReturn, int>
    {
        private TestIntVoDirectReturn(int value) : base(value) { }
        public static TestIntVoDirectReturn Create(int value) => new(value);
    }

    private sealed record TestIntVoUnexpectedReturn : SingleValueObject<TestIntVoUnexpectedReturn, int>
    {
        private TestIntVoUnexpectedReturn(int value) : base(value) { }
        public static string Create(int value) => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    private sealed record TestIntVoWithTwoCtors : SingleValueObject<TestIntVoWithTwoCtors, int>
    {
        public TestIntVoWithTwoCtors() : base(0) { }
        public TestIntVoWithTwoCtors(int value) : base(value) { }
    }

    private sealed record TestIntVoNoFactoryNoCtor : SingleValueObject<TestIntVoNoFactoryNoCtor, int>
    {
        private TestIntVoNoFactoryNoCtor() : base(0) { }
    }

    private sealed record TestStrVo : StringValueObject<TestStrVo>
    {
        private TestStrVo(string value) : base(value) { }
        public static Result<TestStrVo> Create(string value) => Result<TestStrVo>.Success(new TestStrVo(value));
    }

    private sealed record TestStrVoWithNormalization : StringValueObject<TestStrVoWithNormalization>
    {
        private TestStrVoWithNormalization(string value) : base(value) { }
        public static Result<TestStrVoWithNormalization> Create(string value) =>
            Result<TestStrVoWithNormalization>.Success(new TestStrVoWithNormalization(value.ToUpperInvariant()));
    }

    private sealed record TestStrVoWithFailure : StringValueObject<TestStrVoWithFailure>
    {
        private TestStrVoWithFailure(string value) : base(value) { }
        public static Result<TestStrVoWithFailure> Create(string value) =>
            !string.IsNullOrWhiteSpace(value)
                ? Result<TestStrVoWithFailure>.Success(new TestStrVoWithFailure(value))
                : Result<TestStrVoWithFailure>.Failure(Error.Validation("Invalid", "Value cannot be empty."));
    }

    private sealed record TestStrVoDirectReturn : StringValueObject<TestStrVoDirectReturn>
    {
        private TestStrVoDirectReturn(string value) : base(value) { }
        public static TestStrVoDirectReturn Create(string value) => new(value);
    }

    private sealed record TestStrVoUnexpectedReturn : StringValueObject<TestStrVoUnexpectedReturn>
    {
        private TestStrVoUnexpectedReturn(string value) : base(value) { }
        public static int Create(string value) => value.Length;
    }

    private sealed record TestStrVoWithTwoCtors : StringValueObject<TestStrVoWithTwoCtors>
    {
        public TestStrVoWithTwoCtors() : base(string.Empty) { }
        public TestStrVoWithTwoCtors(string value) : base(value) { }
    }

    private sealed record TestStrVoNoFactoryNoCtor : StringValueObject<TestStrVoNoFactoryNoCtor>
    {
        private TestStrVoNoFactoryNoCtor() : base(string.Empty) { }
    }

    [Fact]
    public void SingleValueObjectValueConverter_NullFactory_ThrowsArgumentNullException()
    {
        Action act = () => _ = new SingleValueObjectValueConverter<TestIntVo, int>(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("factory");
    }

    [Fact]
    public void StringValueObjectValueConverter_NullFactory_ThrowsArgumentNullException()
    {
        Action act = () => _ = new StringValueObjectValueConverter<TestStrVo>(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("factory");
    }

    [Fact]
    public void SingleValueObjectValueConverter_DefaultState_ConvertsCorrectly()
    {
        var converter = new SingleValueObjectValueConverter<TestIntVo, int>(val => TestIntVo.Create(val).Value);
        var toProvider = converter.ConvertToProvider;
        var fromProvider = converter.ConvertFromProvider;

        var vo = TestIntVo.Create(42).Value;
        toProvider(vo).Should().Be(42);
        fromProvider(42).Should().Be(vo);
    }

    [Fact]
    public void SingleValueObjectValueConverter_ParameterlessConstructor_ResolvesDefaultFactorySuccessfully()
    {
        var converter = new SingleValueObjectValueConverter<TestIntVo, int>();
        var vo = TestIntVo.Create(100).Value;

        converter.ConvertToProvider(vo).Should().Be(100);
        ((TestIntVo)converter.ConvertFromProvider(100)!).Value.Should().Be(100);
    }

    [Fact]
    public void SingleValueObjectValueConverter_ParameterlessConstructor_WhenTwoCreateMethods_SelectsMatchingSignature()
    {
        var converter = new SingleValueObjectValueConverter<PublicTestIntVoWithTwoCreateMethods, int>();
        var vo = (PublicTestIntVoWithTwoCreateMethods)converter.ConvertFromProvider(55)!;

        vo.Value.Should().Be(55);
    }

    [Fact]
    public void SingleValueObjectValueConverter_ParameterlessConstructor_ExecutesCreateMethodAndNotBypassed()
    {
        var converter = new SingleValueObjectValueConverter<TestIntVoWithNormalization, int>();
        var vo = (TestIntVoWithNormalization)converter.ConvertFromProvider(50)!;

        vo.Value.Should().Be(1050);
    }

    [Fact]
    public void SingleValueObjectValueConverter_ParameterlessConstructor_WhenResultFailure_ThrowsInvalidOperationException()
    {
        var converter = new SingleValueObjectValueConverter<TestIntVoWithFailure, int>();
        Action act = () => converter.ConvertFromProvider(-5);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot convert '-5' to 'TestIntVoWithFailure': Value must be positive.");
    }

    [Fact]
    public void SingleValueObjectValueConverter_ParameterlessConstructor_WhenDirectInstanceReturned_ConvertsSuccessfully()
    {
        var converter = new SingleValueObjectValueConverter<TestIntVoDirectReturn, int>();
        var vo = TestIntVoDirectReturn.Create(77);

        converter.ConvertToProvider(vo).Should().Be(77);
        converter.ConvertFromProvider(77).Should().Be(vo);
    }

    [Fact]
    public void SingleValueObjectValueConverter_ParameterlessConstructor_WhenUnexpectedReturnType_ThrowsInvalidOperationException()
    {
        var converter = new SingleValueObjectValueConverter<TestIntVoUnexpectedReturn, int>();
        Action act = () => converter.ConvertFromProvider(12);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Unexpected result from factory method 'Create' on 'TestIntVoUnexpectedReturn'.");
    }

    [Fact]
    public void SingleValueObjectValueConverter_ParameterlessConstructor_WhenPublicCtorFallback_ConvertsSuccessfully()
    {
        var converter = new SingleValueObjectValueConverter<PublicTestIntVoPublicCtorOnly, int>();
        var vo = new PublicTestIntVoPublicCtorOnly(99);

        converter.ConvertToProvider(vo).Should().Be(99);
        ((PublicTestIntVoPublicCtorOnly)converter.ConvertFromProvider(99)!).Value.Should().Be(99);
    }

    [Fact]
    public void SingleValueObjectValueConverter_ParameterlessConstructor_WhenPrivateCtorFallback_ConvertsSuccessfully()
    {
        var converter = new SingleValueObjectValueConverter<PublicTestIntVoPrivateCtorOnly, int>();
        ((PublicTestIntVoPrivateCtorOnly)converter.ConvertFromProvider(88)!).Value.Should().Be(88);
    }

    [Fact]
    public void SingleValueObjectValueConverter_ParameterlessConstructor_WhenMultipleCtors_SelectsMatchingSignature()
    {
        var converter = new SingleValueObjectValueConverter<TestIntVoWithTwoCtors, int>();
        var vo = new TestIntVoWithTwoCtors(123);

        converter.ConvertToProvider(vo).Should().Be(123);
        converter.ConvertFromProvider(123).Should().Be(vo);
    }

    [Fact]
    public void SingleValueObjectValueConverter_ParameterlessConstructor_WhenNoFactoryAndNoCtor_ThrowsInvalidOperationException()
    {
        Action act = () => _ = new SingleValueObjectValueConverter<TestIntVoNoFactoryNoCtor, int>();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"No 'Create({typeof(int).Name})' method or constructor found on '{typeof(TestIntVoNoFactoryNoCtor).FullName}'.");
    }

    [Fact]
    public void StringValueObjectValueConverter_DefaultState_ConvertsCorrectly()
    {
        var converter = new StringValueObjectValueConverter<TestStrVo>(val => TestStrVo.Create(val).Value);
        var toProvider = converter.ConvertToProvider;
        var fromProvider = converter.ConvertFromProvider;

        var vo = TestStrVo.Create("hello").Value;
        toProvider(vo).Should().Be("hello");
        fromProvider("hello").Should().Be(vo);
    }

    [Fact]
    public void StringValueObjectValueConverter_ParameterlessConstructor_ResolvesDefaultFactorySuccessfully()
    {
        var converter = new StringValueObjectValueConverter<TestStrVo>();
        var vo = TestStrVo.Create("sample").Value;

        converter.ConvertToProvider(vo).Should().Be("sample");
        ((TestStrVo)converter.ConvertFromProvider("sample")!).Value.Should().Be("sample");
    }

    [Fact]
    public void StringValueObjectValueConverter_ParameterlessConstructor_WhenTwoCreateMethods_SelectsMatchingSignature()
    {
        var converter = new StringValueObjectValueConverter<PublicTestStrVoWithTwoCreateMethods>();
        var vo = (PublicTestStrVoWithTwoCreateMethods)converter.ConvertFromProvider("specific")!;

        vo.Value.Should().Be("specific");
    }

    [Fact]
    public void StringValueObjectValueConverter_ParameterlessConstructor_ExecutesCreateMethodAndNotBypassed()
    {
        var converter = new StringValueObjectValueConverter<TestStrVoWithNormalization>();
        var vo = (TestStrVoWithNormalization)converter.ConvertFromProvider("lowercase")!;

        vo.Value.Should().Be("LOWERCASE");
    }

    [Fact]
    public void StringValueObjectValueConverter_ParameterlessConstructor_WhenResultFailure_ThrowsInvalidOperationException()
    {
        var converter = new StringValueObjectValueConverter<TestStrVoWithFailure>();
        Action act = () => converter.ConvertFromProvider("");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot convert '' to 'TestStrVoWithFailure': Value cannot be empty.");
    }

    [Fact]
    public void StringValueObjectValueConverter_ParameterlessConstructor_WhenDirectInstanceReturned_ConvertsSuccessfully()
    {
        var converter = new StringValueObjectValueConverter<TestStrVoDirectReturn>();
        var vo = TestStrVoDirectReturn.Create("direct");

        converter.ConvertToProvider(vo).Should().Be("direct");
        converter.ConvertFromProvider("direct").Should().Be(vo);
    }

    [Fact]
    public void StringValueObjectValueConverter_ParameterlessConstructor_WhenUnexpectedReturnType_ThrowsInvalidOperationException()
    {
        var converter = new StringValueObjectValueConverter<TestStrVoUnexpectedReturn>();
        Action act = () => converter.ConvertFromProvider("abc");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Unexpected result from factory method 'Create' on 'TestStrVoUnexpectedReturn'.");
    }

    [Fact]
    public void StringValueObjectValueConverter_ParameterlessConstructor_WhenPublicCtorFallback_ConvertsSuccessfully()
    {
        var converter = new StringValueObjectValueConverter<PublicTestStrVoPublicCtorOnly>();
        var vo = new PublicTestStrVoPublicCtorOnly("ctor-only");

        converter.ConvertToProvider(vo).Should().Be("ctor-only");
        ((PublicTestStrVoPublicCtorOnly)converter.ConvertFromProvider("ctor-only")!).Value.Should().Be("ctor-only");
    }

    [Fact]
    public void StringValueObjectValueConverter_ParameterlessConstructor_WhenPrivateCtorFallback_ConvertsSuccessfully()
    {
        var converter = new StringValueObjectValueConverter<PublicTestStrVoPrivateCtorOnly>();
        ((PublicTestStrVoPrivateCtorOnly)converter.ConvertFromProvider("private-ctor")!).Value.Should().Be("private-ctor");
    }

    [Fact]
    public void StringValueObjectValueConverter_ParameterlessConstructor_WhenMultipleCtors_SelectsMatchingSignature()
    {
        var converter = new StringValueObjectValueConverter<TestStrVoWithTwoCtors>();
        var vo = new TestStrVoWithTwoCtors("signature-match");

        converter.ConvertToProvider(vo).Should().Be("signature-match");
        converter.ConvertFromProvider("signature-match").Should().Be(vo);
    }

    [Fact]
    public void StringValueObjectValueConverter_ParameterlessConstructor_WhenNoFactoryAndNoCtor_ThrowsInvalidOperationException()
    {
        Action act = () => _ = new StringValueObjectValueConverter<TestStrVoNoFactoryNoCtor>();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"No 'Create(string)' method or constructor found on '{typeof(TestStrVoNoFactoryNoCtor).FullName}'.");
    }

    [Fact]
    public void ScalarValueConverters_DefaultState_ConvertCorrectly()
    {
        var emailConverter = new EmailValueConverter();
        var email = Email.Create("user@test.com").Value;
        emailConverter.ConvertToProvider(email).Should().Be("user@test.com");
        emailConverter.ConvertFromProvider("user@test.com").Should().Be(email);

        var phoneConverter = new PhoneNumberValueConverter();
        var phone = PhoneNumber.Create("+18095551234").Value;
        phoneConverter.ConvertToProvider(phone).Should().Be("+18095551234");
        phoneConverter.ConvertFromProvider("+18095551234").Should().Be(phone);

        var postalConverter = new PostalCodeValueConverter();
        var postal = PostalCode.Create("10101").Value;
        postalConverter.ConvertToProvider(postal).Should().Be("10101");
        postalConverter.ConvertFromProvider("10101").Should().Be(postal);

        var currencyConverter = new CurrencyCodeValueConverter();
        var currency = CurrencyCode.Create("USD").Value;
        currencyConverter.ConvertToProvider(currency).Should().Be("USD");
        currencyConverter.ConvertFromProvider("USD").Should().Be(currency);

        var percentageConverter = new PercentageValueConverter();
        var percentage = Percentage.Create(18.5m).Value;
        percentageConverter.ConvertToProvider(percentage).Should().Be(18.5m);
        percentageConverter.ConvertFromProvider(18.5m).Should().Be(percentage);

        var taxRateConverter = new TaxRateValueConverter();
        var taxRate = TaxRate.Create(18.0m).Value;
        taxRateConverter.ConvertToProvider(taxRate).Should().Be(18.0m);
        taxRateConverter.ConvertFromProvider(18.0m).Should().Be(taxRate);

        var quantityConverter = new QuantityValueConverter();
        var qty = Quantity.Create(25).Value;
        quantityConverter.ConvertToProvider(qty).Should().Be(25);
        quantityConverter.ConvertFromProvider(25).Should().Be(qty);
    }

    [Fact]
    public void GenericStringValueObjectValueConverter_DefaultState_ConvertsFiscalRecordClassesCorrectly()
    {
        var cedulaConverter = new StringValueObjectValueConverter<Cedula>(raw => Cedula.Create(raw).Value);
        var cedula = Cedula.Create("00112345673").Value;
        cedulaConverter.ConvertToProvider(cedula).Should().Be("00112345673");
        cedulaConverter.ConvertFromProvider("00112345673").Should().Be(cedula);

        var rncConverter = new StringValueObjectValueConverter<Rnc>(raw => Rnc.Create(raw).Value);
        var rnc = Rnc.Create("131880738").Value;
        rncConverter.ConvertToProvider(rnc).Should().Be("131880738");
        rncConverter.ConvertFromProvider("131880738").Should().Be(rnc);

        var ncfConverter = new StringValueObjectValueConverter<Ncf>(raw => Ncf.Create(raw).Value);
        var ncf = Ncf.Create("B0100000001").Value;
        ncfConverter.ConvertToProvider(ncf).Should().Be("B0100000001");
        ncfConverter.ConvertFromProvider("B0100000001").Should().Be(ncf);
    }

    [Fact]
    public void GenericValueConverter_DefaultState_ConvertsFiscalRecordStructsCorrectly()
    {
        var rfcConverter = new ValueConverter<Rfc, string>(v => v.Value, p => Rfc.Create(p).Value);
        var rfc = Rfc.Create("XAXX010101000").Value;
        rfcConverter.ConvertToProvider(rfc).Should().Be("XAXX010101000");
        rfcConverter.ConvertFromProvider("XAXX010101000").Should().Be(rfc);

        var curpConverter = new ValueConverter<Curp, string>(v => v.Value, p => Curp.Create(p).Value);
        var curp = Curp.Create("AAAA000000HDFRRR00").Value;
        curpConverter.ConvertToProvider(curp).Should().Be("AAAA000000HDFRRR00");
        curpConverter.ConvertFromProvider("AAAA000000HDFRRR00").Should().Be(curp);

        var cuitConverter = new ValueConverter<Cuit, string>(v => v.Value, p => Cuit.Create(p).Value);
        var cuit = Cuit.Create("20123456786").Value;
        cuitConverter.ConvertToProvider(cuit).Should().Be("20123456786");
        cuitConverter.ConvertFromProvider("20123456786").Should().Be(cuit);

        var rutConverter = new ValueConverter<Rut, string>(v => v.ToCanonicalString(), p => Rut.Create(p).Value);
        var rut = Rut.Create("12345678-5").Value;
        rutConverter.ConvertToProvider(rut).Should().Be("12345678-5");
        rutConverter.ConvertFromProvider("12345678-5").Should().Be(rut);

        var rucConverter = new ValueConverter<Ruc, string>(v => v.Value, p => Ruc.Create(p).Value);
        var ruc = Ruc.Create("20100070970").Value;
        rucConverter.ConvertToProvider(ruc).Should().Be("20100070970");
        rucConverter.ConvertFromProvider("20100070970").Should().Be(ruc);
    }
}





