// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EricksonLopez.ValueObjects.Analyzers;

/// <summary>
/// Provides a Roslyn diagnostic analyzer that enforces Value Object properties and fields are immutable.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ValueObjectImmutabilityAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Represents the diagnostic identifier for this rule (<c>ELVO003</c>).
    /// </summary>
    public const string DiagnosticId = "ELVO003";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "Value Objects must be immutable",
        "Property '{0}' on Value Object '{1}' must be read-only or init-only to preserve domain immutability",
        "Architecture.Domain",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Value Objects in Domain-Driven Design must be immutable by design. Mutable setters violate value equality and thread safety.");

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

        // Check if type implements IValueObject or inherits SingleValueObject / ValueObject
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

        foreach (var member in namedTypeSymbol.GetMembers())
        {
            if (member is IPropertySymbol propertySymbol)
            {
                if (propertySymbol.SetMethod is not null && !propertySymbol.SetMethod.IsInitOnly)
                {
                    var diagnostic = Diagnostic.Create(
                        Rule,
                        propertySymbol.Locations[0],
                        propertySymbol.Name,
                        namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
            else if (member is IFieldSymbol fieldSymbol)
            {
                if (!fieldSymbol.IsReadOnly && !fieldSymbol.IsConst && !fieldSymbol.IsImplicitlyDeclared)
                {
                    var diagnostic = Diagnostic.Create(
                        Rule,
                        fieldSymbol.Locations[0],
                        fieldSymbol.Name,
                        namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}


