// Copyright © Erickson Lopez. MIT License.
using System;
using System.Threading;
using System.Threading.Tasks;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.Analyzers.UnitTests;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Reusable test fixture helper for executing Roslyn diagnostic analyzers against in-memory C# compilations.
/// </summary>
internal static class RoslynAnalyzerTestHelper
{
    public static async Task<ImmutableArray<Diagnostic>> RunAnalyzerAsync(
        DiagnosticAnalyzer analyzer,
        string source,
        CancellationToken cancellationToken = default)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source, cancellationToken: cancellationToken);

        var referenceList = new List<MetadataReference>();
        var assemblies = new[]
        {
            typeof(object).Assembly,
            typeof(Attribute).Assembly,
            typeof(Enumerable).Assembly,
            typeof(ValueTask).Assembly,
            typeof(IValueObject).Assembly
        };

        foreach (var assembly in assemblies)
        {
            if (!string.IsNullOrEmpty(assembly.Location) && File.Exists(assembly.Location))
            {
                referenceList.Add(MetadataReference.CreateFromFile(assembly.Location));
            }
        }

        var runtimeDir = Path.GetDirectoryName(typeof(object).Assembly.Location);
        if (!string.IsNullOrEmpty(runtimeDir))
        {
            var systemRuntimePath = Path.Combine(runtimeDir, "System.Runtime.dll");
            if (File.Exists(systemRuntimePath) && referenceList.All(r => (r as PortableExecutableReference)?.FilePath != systemRuntimePath))
            {
                referenceList.Add(MetadataReference.CreateFromFile(systemRuntimePath));
            }
        }

        var compilation = CSharpCompilation.Create(
            "TestAssembly",
            [syntaxTree],
            referenceList,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var compilationWithAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create(analyzer));
        return await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync(cancellationToken);
    }
}





