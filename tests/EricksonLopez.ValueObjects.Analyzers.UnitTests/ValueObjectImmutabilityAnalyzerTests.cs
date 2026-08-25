// Copyright © Erickson Lopez. MIT License.
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EricksonLopez.ValueObjects.Analyzers.UnitTests;

using AwesomeAssertions;
using EricksonLopez.ValueObjects.Analyzers;
using Microsoft.CodeAnalysis;
using Xunit;

public sealed class ValueObjectImmutabilityAnalyzerTests
{
    private readonly ValueObjectImmutabilityAnalyzer _analyzer = new();

    [Fact]
    public void SupportedDiagnostics_WhenInstantiated_ExposesELVO003Descriptor()
    {
        var analyzer = new ValueObjectImmutabilityAnalyzer();
        analyzer.SupportedDiagnostics.Should().HaveCount(1);
        var rule = analyzer.SupportedDiagnostics[0];
        rule.Id.Should().Be("ELVO003");
        rule.Title.ToString().Should().Be("Value Objects must be immutable");
        rule.DefaultSeverity.Should().Be(DiagnosticSeverity.Error);
        rule.Category.Should().Be("Architecture.Domain");
        rule.IsEnabledByDefault.Should().BeTrue();
    }

    [Fact]
    public async Task Analyze_WhenInterfaceIValueObjectHasMutableProperty_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}

        public sealed class MySampleVo : IValueObject
        {
            public string Name { get; set; } = string.Empty;
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO003" && d.GetMessage().Contains("Property 'Name' on Value Object 'MySampleVo'"));
    }

    [Fact]
    public async Task Analyze_WhenGenericInterfaceIValueObjectHasMutableProperty_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject<TSelf> {}

        public sealed class MySampleVo : IValueObject<MySampleVo>
        {
            public int Value { get; set; }
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO003" && d.GetMessage().Contains("Property 'Value' on Value Object 'MySampleVo'"));
    }

    [Fact]
    public async Task Analyze_WhenBaseTypeIsValueObjectWithMutableProperty_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public abstract class ValueObject {}

        public sealed class ComplexAddress : ValueObject
        {
            public string Street { get; set; } = string.Empty;
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO003" && d.GetMessage().Contains("Street"));
    }

    [Fact]
    public async Task Analyze_WhenBaseTypeIsSingleValueObjectWithMutableField_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public abstract class SingleValueObject {}

        public sealed class MoneyVo : SingleValueObject
        {
            public decimal RawAmount;
            public decimal Amount { get; }
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO003" && (d.GetMessage().Contains("Property 'RawAmount'") || d.GetMessage().Contains("RawAmount")));
    }

    [Fact]
    public async Task Analyze_WhenBaseTypeIsStringValueObjectWithMutableProperty_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public abstract class StringValueObject {}

        public sealed class CustomerCode : StringValueObject
        {
            public string Code { get; private set; } = string.Empty;
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO003" && d.GetMessage().Contains("Code"));
    }

    [Fact]
    public async Task Analyze_WhenValueObjectIsFullyImmutable_DoesNotReportDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}

        public sealed class ValidVo : IValueObject
        {
            public const int MaxLength = 100;
            public static readonly string DefaultKey = "DEF";
            private readonly string _internalValue;

            public ValidVo(string val) => _internalValue = val;

            public string ReadOnlyProp { get; } = string.Empty;
            public string InitOnlyProp { get; init; } = string.Empty;
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task Analyze_WhenNonValueObjectTypeHasMutableProperty_DoesNotReportDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public sealed class RegularEntity
        {
            public string Name { get; set; } = string.Empty;
            public int Age;
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task Analyze_WhenValueObjectInheritsDeeplyWithMutableProperty_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public abstract class ValueObject {}
        public abstract class IntermediateBase : ValueObject {}

        public sealed class DeepVo : IntermediateBase
        {
            public int MutableCounter { get; set; }
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO003" && d.GetMessage().Contains("MutableCounter"));
    }
}







