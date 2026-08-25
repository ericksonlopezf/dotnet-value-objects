// Copyright © Erickson Lopez. MIT License.
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EricksonLopez.ValueObjects.Analyzers.UnitTests;

using AwesomeAssertions;
using EricksonLopez.ValueObjects.Analyzers;
using Microsoft.CodeAnalysis;
using Xunit;

public sealed class ValueObjectConstructorAnalyzerTests
{
    private readonly ValueObjectConstructorAnalyzer _analyzer = new();

    [Fact]
    public void SupportedDiagnostics_WhenInstantiated_ExposesELVO001Descriptor()
    {
        var analyzer = new ValueObjectConstructorAnalyzer();
        analyzer.SupportedDiagnostics.Should().HaveCount(1);
        var rule = analyzer.SupportedDiagnostics[0];
        rule.Id.Should().Be("ELVO001");
        rule.Title.ToString().Should().Be("Value Objects must have private or protected constructors");
        rule.DefaultSeverity.Should().Be(DiagnosticSeverity.Error);
        rule.Category.Should().Be("Architecture.Domain");
        rule.IsEnabledByDefault.Should().BeTrue();
    }

    [Fact]
    public async Task Analyze_WhenValueObjectHasPublicConstructor_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}

        public sealed class MySampleVo : IValueObject
        {
            public string Value { get; }
            public MySampleVo(string value) => Value = value;
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO001" && d.GetMessage().Contains("MySampleVo"));
    }

    [Fact]
    public async Task Analyze_WhenGenericInterfaceIValueObjectHasPublicConstructor_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject<TSelf> {}

        public sealed class GenericVo : IValueObject<GenericVo>
        {
            public GenericVo(int value) {}
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO001" && d.GetMessage().Contains("GenericVo"));
    }

    [Fact]
    public async Task Analyze_WhenValueObjectHasInternalConstructor_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public abstract class ValueObject {}

        public sealed class MyVo : ValueObject
        {
            public string Value { get; }
            internal MyVo(string value) => Value = value;
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO001" && d.GetMessage().Contains("MyVo"));
    }

    [Fact]
    public async Task Analyze_WhenNonAbstractValueObjectHasProtectedConstructor_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}

        public class NonAbstractVo : IValueObject
        {
            public string Value { get; }
            protected NonAbstractVo(string value) => Value = value;
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO001" && d.GetMessage().Contains("NonAbstractVo"));
    }

    [Fact]
    public async Task Analyze_WhenBaseClassIsSingleValueObjectHasPublicConstructor_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public abstract class SingleValueObject {}

        public sealed class ScalarVo : SingleValueObject
        {
            public ScalarVo(int value) {}
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO001" && d.GetMessage().Contains("ScalarVo"));
    }

    [Fact]
    public async Task Analyze_WhenBaseClassIsStringValueObjectHasPublicConstructor_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public abstract class StringValueObject {}

        public sealed class StringVo : StringValueObject
        {
            public StringVo(string value) {}
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO001" && d.GetMessage().Contains("StringVo"));
    }

    [Fact]
    public async Task Analyze_WhenAbstractValueObjectHasPublicConstructor_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}

        public abstract class AbstractVoWithPublicCtor : IValueObject
        {
            public AbstractVoWithPublicCtor(string value) {}
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO001" && d.GetMessage().Contains("AbstractVoWithPublicCtor"));
    }

    [Fact]
    public async Task Analyze_WhenValueObjectHasPrivateConstructor_DoesNotReportDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}

        public sealed class ValidVo : IValueObject
        {
            public string Value { get; }
            private ValidVo(string value) => Value = value;
            public static ValidVo Create(string value) => new(value);
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task Analyze_WhenAbstractValueObjectHasProtectedConstructor_DoesNotReportDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}

        public abstract class BaseVo : IValueObject
        {
            public string Value { get; }
            protected BaseVo(string value) => Value = value;
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task Analyze_WhenNonValueObjectTypeHasPublicConstructor_DoesNotReportDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public sealed class RegularClass
        {
            public string Value { get; }
            public RegularClass(string value) => Value = value;
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().BeEmpty();
    }
}







