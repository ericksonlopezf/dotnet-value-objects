// Copyright © Erickson Lopez. MIT License.
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EricksonLopez.ValueObjects.Analyzers.UnitTests;

using AwesomeAssertions;
using EricksonLopez.ValueObjects.Analyzers;
using Microsoft.CodeAnalysis;
using Xunit;

public sealed class ValueObjectFactoryAnalyzerTests
{
    private readonly ValueObjectFactoryAnalyzer _analyzer = new();

    [Fact]
    public void SupportedDiagnostics_WhenInstantiated_ExposesELVO002Descriptor()
    {
        var analyzer = new ValueObjectFactoryAnalyzer();
        analyzer.SupportedDiagnostics.Should().HaveCount(1);
        var rule = analyzer.SupportedDiagnostics[0];
        rule.Id.Should().Be("ELVO002");
        rule.Title.ToString().Should().Be("Value Objects must provide a static Create factory method");
        rule.DefaultSeverity.Should().Be(DiagnosticSeverity.Error);
        rule.Category.Should().Be("Architecture.Domain");
        rule.IsEnabledByDefault.Should().BeTrue();
    }

    [Fact]
    public async Task Analyze_WhenValueObjectHasNoCreateMethod_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}

        public sealed class MySampleVo : IValueObject
        {
            public string Value { get; }
            private MySampleVo(string value) => Value = value;
            public static MySampleVo From(string value) => new(value);
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO002" && d.GetMessage().Contains("MySampleVo"));
    }

    [Fact]
    public async Task Analyze_WhenCreateMethodIsNotStatic_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}

        public sealed class MySampleVo : IValueObject
        {
            public string Value { get; }
            private MySampleVo(string value) => Value = value;
            public MySampleVo Create(string value) => new(value);
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO002" && d.GetMessage().Contains("MySampleVo"));
    }

    [Fact]
    public async Task Analyze_WhenCreateMethodIsPrivateStatic_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}

        public sealed class MySampleVo : IValueObject
        {
            public string Value { get; }
            private MySampleVo(string value) => Value = value;
            private static MySampleVo Create(string value) => new(value);
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO002" && d.GetMessage().Contains("MySampleVo"));
    }

    [Fact]
    public async Task Analyze_WhenMemberNamedCreateIsNotMethod_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}

        public sealed class MySampleVo : IValueObject
        {
            public static int Create => 42;
            private MySampleVo() {}
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO002" && d.GetMessage().Contains("MySampleVo"));
    }

    [Fact]
    public async Task Analyze_WhenGenericInterfaceIValueObjectHasNoCreateMethod_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject<TSelf> {}

        public sealed class GenericVo : IValueObject<GenericVo>
        {
            private GenericVo() {}
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO002" && d.GetMessage().Contains("GenericVo"));
    }

    [Fact]
    public async Task Analyze_WhenBaseClassIsValueObjectHasNoCreateMethod_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public abstract class ValueObject {}

        public sealed class CompositeVo : ValueObject
        {
            private CompositeVo() {}
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO002" && d.GetMessage().Contains("CompositeVo"));
    }

    [Fact]
    public async Task Analyze_WhenBaseClassIsSingleValueObjectHasNoCreateMethod_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public abstract class SingleValueObject {}

        public sealed class ScalarVo : SingleValueObject
        {
            private ScalarVo() {}
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO002" && d.GetMessage().Contains("ScalarVo"));
    }

    [Fact]
    public async Task Analyze_WhenBaseClassIsStringValueObjectHasNoCreateMethod_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public abstract class StringValueObject {}

        public sealed class StringVo : StringValueObject
        {
            private StringVo() {}
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO002" && d.GetMessage().Contains("StringVo"));
    }

    [Fact]
    public async Task Analyze_WhenValueObjectIsInterface_DoesNotReportDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public interface IValueObject {}
        public interface ICustomVo : IValueObject {}
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task Analyze_WhenValueObjectHasPublicStaticCreateMethod_DoesNotReportDiagnostic()
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
    public async Task Analyze_WhenAbstractValueObject_DoesNotReportDiagnostic()
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
    public async Task Analyze_WhenNonValueObjectType_DoesNotReportDiagnostic()
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







