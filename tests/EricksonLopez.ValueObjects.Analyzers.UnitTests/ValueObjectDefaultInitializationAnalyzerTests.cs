// Copyright © Erickson Lopez. MIT License.
using System;
using System.Threading.Tasks;
using AwesomeAssertions;
using EricksonLopez.ValueObjects.Analyzers;
using Microsoft.CodeAnalysis;
using Xunit;

namespace EricksonLopez.ValueObjects.Analyzers.UnitTests;

public sealed class ValueObjectDefaultInitializationAnalyzerTests
{
    private readonly ValueObjectDefaultInitializationAnalyzer _analyzer = new();

    [Fact]
    public void SupportedDiagnostics_WhenInstantiated_ExposesELVO004Descriptor()
    {
        var analyzer = new ValueObjectDefaultInitializationAnalyzer();
        analyzer.SupportedDiagnostics.Should().HaveCount(1);
        var rule = analyzer.SupportedDiagnostics[0];
        rule.Id.Should().Be("ELVO004");
        rule.Title.ToString().Should().Be("Value Objects should not be initialized with default");
        rule.DefaultSeverity.Should().Be(DiagnosticSeverity.Error);
        rule.Category.Should().Be("Architecture.Domain");
        rule.IsEnabledByDefault.Should().BeTrue();
    }

    [Fact]
    public async Task Analyze_WhenDefaultExpressionUsedOnStructValueObject_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        using EricksonLopez.ValueObjects;

        public readonly record struct MyStructVo : IValueObject
        {
            public string Value { get; }
        }

        public class Consumer
        {
            public void Run()
            {
                var x = default(MyStructVo);
            }
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO004" && d.GetMessage().Contains("MyStructVo"));
    }

    [Fact]
    public async Task Analyze_WhenDefaultLiteralUsedOnStructValueObject_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        using EricksonLopez.ValueObjects;

        public readonly record struct MyStructVo : IValueObject<MyStructVo>
        {
            public int Value { get; }
        }

        public class Consumer
        {
            public void Run()
            {
                MyStructVo x = default;
            }
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO004" && d.GetMessage().Contains("MyStructVo"));
    }

    [Fact]
    public async Task Analyze_WhenObjectCreationParameterlessUsedOnStructValueObject_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        using EricksonLopez.ValueObjects;

        public struct MyStructVo : IValueObject
        {
            public string Value { get; set; }
        }

        public class Consumer
        {
            public void Run()
            {
                var x = new MyStructVo();
            }
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO004" && d.GetMessage().Contains("MyStructVo"));
    }

    [Fact]
    public async Task Analyze_WhenAttributeAppliedOnStruct_ReportsDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        using System;

        [AttributeUsage(AttributeTargets.Struct)]
        public class ValueObjectAttribute : Attribute {}

        [ValueObject]
        public struct AttributedStructVo
        {
            public int Value { get; set; }
        }

        public class Consumer
        {
            public void Run()
            {
                var x = default(AttributedStructVo);
            }
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().ContainSingle(d => d.Id == "ELVO004" && d.GetMessage().Contains("AttributedStructVo"));
    }

    [Fact]
    public async Task Analyze_WhenValueObjectIsClass_DoesNotReportDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        using EricksonLopez.ValueObjects;

        public sealed class MyClassVo : IValueObject
        {
            public string Value { get; }
            private MyClassVo(string val) => Value = val;
        }

        public class Consumer
        {
            public void Run()
            {
                var x = default(MyClassVo);
            }
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task Analyze_WhenNormalStruct_DoesNotReportDiagnostic()
    {
        var source = """
        namespace SampleNamespace;

        public struct NormalStruct
        {
            public int Value;
        }

        public class Consumer
        {
            public void Run()
            {
                var x = default(NormalStruct);
                var y = new NormalStruct();
                NormalStruct z = default;
            }
        }
        """;

        var diagnostics = await RoslynAnalyzerTestHelper.RunAnalyzerAsync(_analyzer, source, TestContext.Current.CancellationToken);
        diagnostics.Should().BeEmpty();
    }
}
