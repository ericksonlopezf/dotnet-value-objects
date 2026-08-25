// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EricksonLopez.ValueObjects.Analyzers;

/// <summary>
/// Provides a Roslyn diagnostic analyzer that enforces Value Objects declare private or protected constructors.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ValueObjectConstructorAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Represents the diagnostic identifier for this rule (<c>ELVO001</c>).
    /// </summary>
    public const string DiagnosticId = "ELVO001";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "Value Objects must have private or protected constructors",
        "Constructor on Value Object '{0}' must be private or protected to enforce factory method creation",
        "Architecture.Domain",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Constructors of Value Objects in Domain-Driven Design must be private (or protected for abstract base types) to prevent instantiation in invalid states and enforce static factory methods.");

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

        foreach (var constructor in namedTypeSymbol.Constructors)
        {
            if (constructor.IsImplicitlyDeclared) continue;

            // Allow private constructors, or protected constructors if the type is abstract
            bool isAllowed = constructor.DeclaredAccessibility == Accessibility.Private
                             || (namedTypeSymbol.IsAbstract && constructor.DeclaredAccessibility == Accessibility.Protected);

            if (!isAllowed)
            {
                var location = constructor.Locations.FirstOrDefault();
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
}


