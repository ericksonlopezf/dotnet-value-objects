// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EricksonLopez.ValueObjects.Analyzers;

/// <summary>
/// Provides a Roslyn diagnostic analyzer that enforces Value Objects declare at least one public static factory method named 'Create'.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ValueObjectFactoryAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Represents the diagnostic identifier for this rule (<c>ELVO002</c>).
    /// </summary>
    public const string DiagnosticId = "ELVO002";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "Value Objects must provide a static Create factory method",
        "Value Object '{0}' must declare at least one public static 'Create' factory method returning a Result",
        "Architecture.Domain",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Value Objects in Domain-Driven Design must provide a static factory method named 'Create' that encapsulates invariant validation and returns a Result.");

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
    }

    private static void AnalyzeNamedType(SymbolAnalysisContext context)
    {
        var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

        // Skip abstract types or interfaces
        if (namedTypeSymbol.IsAbstract || namedTypeSymbol.TypeKind == TypeKind.Interface) return;

        bool isValueObject = namedTypeSymbol.AllInterfaces.Any(i => i.Name is "IValueObject" or "IValueObject`1");
        if (!isValueObject)
        {
            var baseType = namedTypeSymbol.BaseType;
            while (baseType is not null)
            {
                if (baseType.Name is "ValueObject" or "SingleValueObject" or "StringValueObject")
                {
                    isValueObject = true;
                    break;
                }
                baseType = baseType.BaseType;
            }
        }

        if (!isValueObject) return;

        // Check if there is at least one public static Create method
        bool hasCreateFactory = namedTypeSymbol.GetMembers("Create")
            .OfType<IMethodSymbol>()
            .Any(m => m.IsStatic && m.DeclaredAccessibility == Accessibility.Public);

        if (!hasCreateFactory)
        {
            var location = namedTypeSymbol.Locations.FirstOrDefault();
            if (location is not null)
            {
                var diagnostic = Diagnostic.Create(
                    Rule,
                    location,
                    namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}


