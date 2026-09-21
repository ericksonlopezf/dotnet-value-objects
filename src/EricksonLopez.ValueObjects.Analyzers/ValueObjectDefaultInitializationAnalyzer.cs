// Copyright © Erickson Lopez. MIT License.
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EricksonLopez.ValueObjects.Analyzers;

/// <summary>
/// Provides a Roslyn diagnostic analyzer that enforces Value Objects are not initialized with default.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ValueObjectDefaultInitializationAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Represents the diagnostic identifier for this rule (<c>ELVO004</c>).
    /// </summary>
    public const string DiagnosticId = "ELVO004";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "Value Objects should not be initialized with default",
        "Value Object '{0}' cannot be initialized with default; use its Create method to ensure validation",
        "Architecture.Domain",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Value Objects defined as structs bypass their factory methods when initialized with default(T). This can result in objects with an invalid state, violating Domain-Driven Design principles.");

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeDefaultExpression, SyntaxKind.DefaultExpression);
        context.RegisterSyntaxNodeAction(AnalyzeDefaultLiteral, SyntaxKind.DefaultLiteralExpression);
        context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
    }

    private static void AnalyzeDefaultExpression(SyntaxNodeAnalysisContext context)
    {
        var defaultExpr = (DefaultExpressionSyntax)context.Node;
        var typeInfo = context.SemanticModel.GetTypeInfo(defaultExpr.Type);

        CheckAndReport(context, typeInfo.Type, defaultExpr.GetLocation());
    }

    private static void AnalyzeDefaultLiteral(SyntaxNodeAnalysisContext context)
    {
        var defaultLiteral = (LiteralExpressionSyntax)context.Node;
        var typeInfo = context.SemanticModel.GetTypeInfo(defaultLiteral);

        CheckAndReport(context, typeInfo.Type, defaultLiteral.GetLocation());
    }

    private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
    {
        var objectCreation = (ObjectCreationExpressionSyntax)context.Node;
        if (objectCreation.ArgumentList == null || objectCreation.ArgumentList.Arguments.Count == 0)
        {
            var typeInfo = context.SemanticModel.GetTypeInfo(objectCreation.Type);
            CheckAndReport(context, typeInfo.Type, objectCreation.GetLocation());
        }
    }

    private static void CheckAndReport(SyntaxNodeAnalysisContext context, ITypeSymbol? typeSymbol, Location location)
    {
        if (typeSymbol is null) return;

        // We only care about structs because reference types become null (which is handled by NRT and CA)
        if (!typeSymbol.IsValueType) return;

        bool isValueObject = typeSymbol.AllInterfaces.Any(i => i.Name == "IValueObject")
            || typeSymbol.GetAttributes().Any(a => a.AttributeClass?.Name is "ValueObjectAttribute" or "ValueObject");


        if (isValueObject)
        {
            var diagnostic = Diagnostic.Create(Rule, location, typeSymbol.Name);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
