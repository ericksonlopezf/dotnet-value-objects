// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EricksonLopez.ValueObjects.Generators;

/// <summary>
/// Provides an incremental source generator that generates parsing, JSON serialization, and conversion implementations for Value Objects.
/// </summary>
[Generator]
public sealed class ValueObjectIncrementalGenerator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var typeDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsCandidateType(s),
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m is not null);

        var compilationAndTypes = context.CompilationProvider.Combine(typeDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndTypes, static (spc, source) => Execute(source.Left, source.Right!, spc));
    }

    internal static bool IsCandidateType(SyntaxNode node) =>
        node is TypeDeclarationSyntax typeDecl && typeDecl.AttributeLists.Count > 0;

    internal static bool IsValueObjectAttribute(INamedTypeSymbol attributeClass) =>
        (attributeClass.Name is "ValueObjectAttribute" or "ValueObject")
        && (attributeClass.ContainingNamespace.IsGlobalNamespace
            || attributeClass.ContainingNamespace.ToDisplayString() == "EricksonLopez.ValueObjects");

    private static INamedTypeSymbol? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var typeDecl = (TypeDeclarationSyntax)context.Node;
        foreach (var attributeList in typeDecl.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attribute).Symbol is IMethodSymbol { ContainingType: { } containingType }
                    && IsValueObjectAttribute(containingType))
                {
                    return context.SemanticModel.GetDeclaredSymbol(typeDecl) as INamedTypeSymbol;
                }
            }
        }
        return null;
    }

    private static void Execute(Compilation compilation, ImmutableArray<INamedTypeSymbol> types, SourceProductionContext context)
    {
        bool hasDapper = compilation.GetTypeByMetadataName("Dapper.SqlMapper") != null;
        bool hasEfCore = compilation.GetTypeByMetadataName("Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter`2") != null;

        foreach (var typeSymbol in types.Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default))
        {
            var source = GenerateCode(typeSymbol, hasDapper, hasEfCore);
            if (!string.IsNullOrEmpty(source))
            {
                context.AddSource($"{typeSymbol.Name}_ValueObject.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }
    }

    internal static AttributeData? FindValueObjectAttribute(INamedTypeSymbol typeSymbol)
    {
        foreach (var a in typeSymbol.GetAttributes())
        {
            if (a.AttributeClass is { } attrClass && IsValueObjectAttribute(attrClass))
            {
                return a;
            }
        }
        return null;
    }

    private static string GenerateCode(INamedTypeSymbol typeSymbol, bool hasDapper = false, bool hasEfCore = false)
    {
        var valueObjectAttr = FindValueObjectAttribute(typeSymbol);

        bool genConv = false;
        bool genHooks = true;
        const bool genJson = true;

        if (valueObjectAttr != null)
        {
            foreach (var arg in valueObjectAttr.NamedArguments)
            {
                if (arg.Key == "GenerateConversionOperators" && arg.Value.Value is bool gc) genConv = gc;
                if (arg.Key == "GeneratePersistenceHooks" && arg.Value.Value is bool gh) genHooks = gh;
            }
        }

        var baseType = typeSymbol.BaseType;
        string? valueTypeName = null;
        while (baseType != null && valueTypeName == null)
        {
            var baseName = baseType.OriginalDefinition.ToDisplayString();
            if (baseName == "EricksonLopez.ValueObjects.SingleValueObject<TSelf, TValue>")
            {
                valueTypeName = baseType.TypeArguments[1].ToDisplayString();
            }
            else
            {
                baseType = baseType.BaseType;
            }
        }

        if (valueTypeName == null)
        {
            var createMethod = typeSymbol.GetMembers("Create")
                .OfType<IMethodSymbol>()
                .FirstOrDefault(m => m.IsStatic && m.Parameters.Length == 1);
            if (createMethod != null)
            {
                valueTypeName = createMethod.Parameters[0].Type.ToDisplayString();
            }
            else
            {
                var valueProp = typeSymbol.GetMembers("Value")
                    .OfType<IPropertySymbol>()
                    .FirstOrDefault();
                if (valueProp != null)
                {
                    valueTypeName = valueProp.Type.ToDisplayString();
                }
                else
                {
                    valueTypeName = "string";
                }
            }
        }

        bool hasSpanCreate = typeSymbol.GetMembers("Create")
            .OfType<IMethodSymbol>()
            .Any(m => m.IsStatic && m.Parameters.Length == 1 && m.Parameters[0].Type.ToDisplayString().Contains("ReadOnlySpan<char>"));

        var namespaceName = typeSymbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : typeSymbol.ContainingNamespace.ToDisplayString();

        var fileModifier = typeSymbol.IsFileLocal ? "file " : string.Empty;
        var typeKind = fileModifier + (typeSymbol.IsRecord ? (typeSymbol.IsValueType ? "readonly partial record struct" : "sealed partial record class") : (typeSymbol.IsValueType ? "readonly partial struct" : "sealed partial class"));
        var typeName = typeSymbol.Name;

        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        if (!string.IsNullOrEmpty(namespaceName))
        {
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
        }

        if (genJson)
        {
            sb.AppendLine($"    [global::System.Text.Json.Serialization.JsonConverter(typeof({typeName}JsonConverter))]");
        }

        sb.AppendLine($"    {typeKind} {typeName} : global::System.IParsable<{typeName}>, global::System.ISpanParsable<{typeName}>, global::EricksonLopez.ValueObjects.IValueObject<{typeName}>");
        sb.AppendLine("    {");

        // Conversions
        if (genConv)
        {
            sb.AppendLine($"        public static explicit operator {valueTypeName}({typeName} value) => value.Value;");
            sb.AppendLine($$"""
        public static explicit operator {{typeName}}({{valueTypeName}} value)
        {
            var result = Create(value);
            if (result.IsFailure)
            {
                throw new global::System.InvalidCastException($"Cannot cast '{value}' to {{typeName}}: {result.Error.Description}");
            }
            return result.Value;
        }
""");
        }

        var rawValueTypeName = valueTypeName.TrimEnd('?');
        var parseTarget = rawValueTypeName is "int" or "long" or "decimal" or "double" or "float" or "short" or "byte" or "bool"
            ? rawValueTypeName
            : (rawValueTypeName.StartsWith("global::", StringComparison.Ordinal) ? rawValueTypeName : $"global::{rawValueTypeName}");

        if (rawValueTypeName == "string")
        {
            if (hasSpanCreate)
            {
                sb.AppendLine($$"""
        public static {{typeName}} Parse(string s, global::System.IFormatProvider? provider = null)
        {
            global::System.ArgumentNullException.ThrowIfNull(s);
            var result = Create(s);
            if (result.IsFailure)
            {
                throw new global::System.FormatException($"Cannot parse '{s}' as {{typeName}}: {result.Error.Description}");
            }
            return result.Value;
        }

        public static bool TryParse(string? s, global::System.IFormatProvider? provider, out {{typeName}} result)
        {
            if (s is null)
            {
                result = default!;
                return false;
            }

            var res = Create(s);
            if (res.IsSuccess)
            {
                result = res.Value;
                return true;
            }

            result = default!;
            return false;
        }

        public static {{typeName}} Parse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider = null)
        {
            var result = Create(s);
            if (result.IsFailure)
            {
                throw new global::System.FormatException($"Cannot parse '{s.ToString()}' as {{typeName}}: {result.Error.Description}");
            }
            return result.Value;
        }

        public static bool TryParse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider, out {{typeName}} result)
        {
            var res = Create(s);
            if (res.IsSuccess)
            {
                result = res.Value;
                return true;
            }

            result = default!;
            return false;
        }
""");
            }
            else
            {
                sb.AppendLine($$"""
        public static {{typeName}} Parse(string s, global::System.IFormatProvider? provider = null)
        {
            global::System.ArgumentNullException.ThrowIfNull(s);
            var result = Create(s);
            if (result.IsFailure)
            {
                throw new global::System.FormatException($"Cannot parse '{s}' as {{typeName}}: {result.Error.Description}");
            }
            return result.Value;
        }

        public static bool TryParse(string? s, global::System.IFormatProvider? provider, out {{typeName}} result)
        {
            if (s is null)
            {
                result = default!;
                return false;
            }

            var res = Create(s);
            if (res.IsSuccess)
            {
                result = res.Value;
                return true;
            }

            result = default!;
            return false;
        }

        public static {{typeName}} Parse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider = null)
        {
            var str = s.ToString();
            var result = Create(str);
            if (result.IsFailure)
            {
                throw new global::System.FormatException($"Cannot parse '{str}' as {{typeName}}: {result.Error.Description}");
            }
            return result.Value;
        }

        public static bool TryParse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider, out {{typeName}} result)
        {
            var str = s.ToString();
            var res = Create(str);
            if (res.IsSuccess)
            {
                result = res.Value;
                return true;
            }

            result = default!;
            return false;
        }
""");
            }
        }
        else
        {
            sb.AppendLine($$"""
        public static {{typeName}} Parse(string s, global::System.IFormatProvider? provider = null)
        {
            global::System.ArgumentNullException.ThrowIfNull(s);
            if (!{{parseTarget}}.TryParse(s, provider, out var parsedVal))
            {
                throw new global::System.FormatException($"Cannot parse '{s}' as {{rawValueTypeName}} for {{typeName}}.");
            }
            var result = Create(parsedVal);
            if (result.IsFailure)
            {
                throw new global::System.FormatException($"Cannot parse '{s}' as {{typeName}}: {result.Error.Description}");
            }
            return result.Value;
        }

        public static bool TryParse(string? s, global::System.IFormatProvider? provider, out {{typeName}} result)
        {
            if (s is null || !{{parseTarget}}.TryParse(s, provider, out var parsedVal))
            {
                result = default!;
                return false;
            }

            var res = Create(parsedVal);
            if (res.IsSuccess)
            {
                result = res.Value;
                return true;
            }

            result = default!;
            return false;
        }

        public static {{typeName}} Parse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider = null)
        {
            if (!{{parseTarget}}.TryParse(s, provider, out var parsedVal))
            {
                throw new global::System.FormatException($"Cannot parse span as {{rawValueTypeName}} for {{typeName}}.");
            }
            var result = Create(parsedVal);
            if (result.IsFailure)
            {
                throw new global::System.FormatException($"Cannot parse span as {{typeName}}: {result.Error.Description}");
            }
            return result.Value;
        }

        public static bool TryParse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider, out {{typeName}} result)
        {
            if (!{{parseTarget}}.TryParse(s, provider, out var parsedVal))
            {
                result = default!;
                return false;
            }

            var res = Create(parsedVal);
            if (res.IsSuccess)
            {
                result = res.Value;
                return true;
            }

            result = default!;
            return false;
        }
""");
        }

        sb.AppendLine("    }");

        // Json Converter
        if (genJson)
        {
            rawValueTypeName = valueTypeName.TrimEnd('?');
            var valueAccess = typeSymbol.GetMembers("Value").Any() ? "value.Value" : "value.ToString()";
            var converterClassDecl = typeSymbol.IsFileLocal
                ? $"file sealed class {typeName}JsonConverter : global::System.Text.Json.Serialization.JsonConverter<{typeName}>"
                : $"public sealed class {typeName}JsonConverter : global::System.Text.Json.Serialization.JsonConverter<{typeName}>";

            sb.AppendLine();
            sb.AppendLine($$"""
    {{converterClassDecl}}
    {
        public override {{typeName}} Read(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
        {
            {{rawValueTypeName}}? value = global::System.Text.Json.JsonSerializer.Deserialize<{{rawValueTypeName}}>(ref reader, options);
            if (value is null)
            {
                throw new global::System.Text.Json.JsonException($"Expected a valid {{rawValueTypeName}} for {{typeName}}");
            }

            var result = {{typeName}}.Create(value);
            if (result.IsFailure)
            {
                throw new global::System.Text.Json.JsonException(result.Error.Description);
            }

            return result.Value;
        }

        public override void Write(global::System.Text.Json.Utf8JsonWriter writer, {{typeName}} value, global::System.Text.Json.JsonSerializerOptions options)
        {
            global::System.Text.Json.JsonSerializer.Serialize(writer, {{valueAccess}}, options);
        }
    }
""");
        }

        if (genHooks && hasDapper)
        {
            rawValueTypeName = valueTypeName.TrimEnd('?');
            var valueAccess = typeSymbol.GetMembers("Value").Any() ? "value.Value" : "value.ToString()";
            sb.AppendLine();
            sb.AppendLine($$"""
    public sealed class {{typeName}}DapperTypeHandler : global::Dapper.SqlMapper.TypeHandler<{{typeName}}>
    {
        public override void SetValue(global::System.Data.IDbDataParameter parameter, {{typeName}} value)
        {
            parameter.Value = {{valueAccess}};
        }

        public override {{typeName}} Parse(object value)
        {
            if (value is {{rawValueTypeName}} raw)
            {
                var res = {{typeName}}.Create(raw);
                if (res.IsSuccess) return res.Value;
                throw new global::System.FormatException(res.Error.Description);
            }
            throw new global::System.FormatException($"Expected {{rawValueTypeName}} for {{typeName}}");
        }
    }
""");
        }

        if (genHooks && hasEfCore)
        {
            rawValueTypeName = valueTypeName.TrimEnd('?');
            sb.AppendLine();
            sb.AppendLine($$"""
    public sealed class {{typeName}}ValueConverter : global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<{{typeName}}, {{rawValueTypeName}}>
    {
        public {{typeName}}ValueConverter()
            : base(
                v => v.Value,
                v => {{typeName}}.Create(v).Value)
        {
        }
    }
""");
        }

        if (!string.IsNullOrEmpty(namespaceName))
        {
            sb.AppendLine("}");
        }

        return sb.ToString();
    }
}


