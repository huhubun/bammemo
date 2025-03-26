using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bammemo.CodeAnalysis
{
    [Generator]
    public class MapperGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var provider = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => IsSyntaxTargetForGeneration(node),
                    transform: (ctx, _) => GetSemanticTargetForGeneration(ctx));

            context.RegisterSourceOutput(provider, (spc, typePair) => Execute(spc, typePair));
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        {
            return node is ClassDeclarationSyntax classDecl &&
                   classDecl.AttributeLists.Count > 0;
        }

        private static (INamedTypeSymbol, IEnumerable<(ITypeSymbol source, ITypeSymbol target)>) GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var classDecl = (ClassDeclarationSyntax)context.Node;
            var semanticModel = context.SemanticModel;

            if (!(semanticModel.GetDeclaredSymbol(classDecl) is INamedTypeSymbol classSymbol) || classSymbol == null)
            {
                return default;
            }

            var mapToAttr = classSymbol.GetAttributes()
                .Where(ad => ad.AttributeClass.ContainingAssembly.Name == "Bammemo.Service.Abstractions")
                .Where(ad => ad.AttributeClass.Name == "MapAttribute")
                .Where(ad => ad.AttributeClass.TypeArguments.Length == 2);

            if (mapToAttr == null || !mapToAttr.Any())
                return default;

            return (classSymbol, mapToAttr.Where(a => a.AttributeClass.TypeArguments[0] != null).Select(a => (source: a.AttributeClass.TypeArguments[0], target: a.AttributeClass.TypeArguments[1])));
        }

        private static bool IsNullable(ITypeSymbol typeSymbol)
            => typeSymbol.ContainingNamespace.Name == "System" && typeSymbol.Name == "Nullable";

        private static void Execute(SourceProductionContext context, (INamedTypeSymbol classSymbol, IEnumerable<(ITypeSymbol source, ITypeSymbol target)> types) symbol)
        {
            var (classSymbol, types) = symbol;

            if (classSymbol == null) return;

            var allMethods = new List<string>();

            foreach (var groupTypes in types.GroupBy(t => t.source, SymbolEqualityComparer.Default))
            {
                var sourceToTargetMethods = new List<string>();
                var switchs = new List<string>();
                var nullSwitchs = new List<string>();

                var sourceType = groupTypes.Key as ITypeSymbol;
                var sourceProperties = sourceType.GetMembers().OfType<IPropertySymbol>().Where(p => p.GetMethod != null);
                var sourceFullName = sourceType.ToDisplayString();

                foreach (var (_, targetType) in groupTypes)
                {
                    var targetProperties = targetType.GetMembers().OfType<IPropertySymbol>().Where(p => p.SetMethod != null)
                        .ToDictionary(p => p.Name, p => p);

                    var mappingNew = new List<string>();
                    var mappingEqual = new List<string>();

                    foreach (var sourceProp in sourceProperties)
                    {
                        // 类型完全相同
                        if (targetProperties.TryGetValue(sourceProp.Name, out var targetProp) &&
                            SymbolEqualityComparer.Default.Equals(sourceProp.Type, targetProp.Type))
                        {
                            mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name},");
                            mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name};");
                        }
                        else if (targetProp != null)
                        {
                            // Source 是可空类型，Target 是不可空类型
                            if (IsNullableToNotNullable(sourceProp, targetProp, out var typeArgument))
                            {
                                mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name} ?? default,");
                                mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name} ?? default;");
                            }
                            // Source 是不可空类型，Target 是可空类型
                            else if (IsNotNullableToNullable(sourceProp, targetProp))
                            {
                                mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name},");
                                mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name};");
                            }
                            // Source 是枚举，Target 是 int（暂不考虑其它类型的情况）
                            else if (sourceProp.Type.TypeKind == TypeKind.Enum && targetProp.Type.ContainingNamespace.Name == "System" && targetProp.Type.Name == "Int32")
                            {
                                mappingNew.Add($"{targetProp.Name} = (int)source.{sourceProp.Name},");
                                mappingEqual.Add($"target.{targetProp.Name} = (int)source.{sourceProp.Name};");
                            }
                            // Source 是 int，Target 是枚举（暂不考虑其它类型的情况）
                            else if (sourceProp.Type.ContainingNamespace.Name == "System" && sourceProp.Type.Name == "Int32" && targetProp.Type.TypeKind == TypeKind.Enum)
                            {
                                mappingNew.Add($"{targetProp.Name} = ({targetProp.Type.ContainingNamespace}.{targetProp.Type.Name})source.{sourceProp.Name},");
                                mappingEqual.Add($"target.{targetProp.Name} = ({targetProp.Type.ContainingNamespace}.{targetProp.Type.Name})source.{sourceProp.Name};");
                            }
                        }
                    }

                    var targetFullName = targetType.ToDisplayString();

                    switchs.Add($"{targetFullName} => MapTo(source, dest as {targetFullName}) as TDest,");

                    nullSwitchs.Add($@"if(typeof(TDest) == typeof({targetFullName}))
            {{
                return MapTo(source, null as {targetFullName}) as TDest;
            }}");

                                var sourceToTargetMethod = $@"static {targetFullName} MapTo(this {sourceFullName} source, {targetFullName} target)
        {{
            if(target == null)
            {{
                target = new {targetFullName}
                {{
                    {string.Join("\n                    ", mappingNew)}
                }};
            }}
            else
            {{
                {string.Join("\n                ", mappingEqual)}
            }}

            AfterMap(source, target);

            return target;
        }}

        static partial void AfterMap({sourceFullName} source, {targetFullName} target);";

                    sourceToTargetMethods.Add(sourceToTargetMethod);
                }

                var method = $@"
#region {sourceFullName}

        public static TDest MapTo<TDest>(this {sourceFullName} source) where TDest : class
        {{
            ArgumentNullException.ThrowIfNull(source);

                {string.Join("\n                ", nullSwitchs)}

            throw new NotSupportedException($""Map {sourceFullName} to {{typeof(TDest)}}"");
        }}

        public static TDest MapTo<TDest>(this {sourceFullName} source, TDest dest) where TDest : class
        {{
            ArgumentNullException.ThrowIfNull(source);

            return dest switch
            {{
                {string.Join("\n                ", switchs)}
                _ => throw new NotSupportedException($""Map {sourceFullName} to {{typeof(TDest)}}"")
            }};
        }}

        public static List<TDest> MapToList<TDest>(this IEnumerable<{sourceFullName}> source) where TDest : class
            => source.Select(s => s.MapTo<TDest>()).ToList();

        public static TDest[] MapToArray<TDest>(this IEnumerable<{sourceFullName}> source) where TDest : class
            => source.Select(s => s.MapTo<TDest>()).ToArray();

        {string.Join("\n        ", sourceToTargetMethods)}

#endregion";

                allMethods.Add(method);
            }

            var mapperClass = $@"// <auto-generated/>
using System;

namespace {classSymbol.ContainingAssembly.Name}
{{
    public static partial class {classSymbol.Name}
    {{
        {string.Join("\n", allMethods)}
    }}
}}";

            context.AddSource($"{classSymbol.Name}.g.cs", SourceText.From(mapperClass, Encoding.UTF8));
        }

        private static bool IsNullableToNotNullable(IPropertySymbol sourceProp, IPropertySymbol targetProp, out ITypeSymbol typeArgument)
        {
            var isSourceNullable = IsNullable(sourceProp.Type);
            if (isSourceNullable)
            {
                typeArgument = (sourceProp.Type as INamedTypeSymbol)?.TypeArguments[0];
                return targetProp.Type.ContainingNamespace.Name == typeArgument.ContainingNamespace.Name
                    && targetProp.Type.Name == typeArgument.Name;
            }

            typeArgument = null;
            return false;
        }

        private static bool IsNotNullableToNullable(IPropertySymbol sourceProp, IPropertySymbol targetProp)
        {
            var isTargetNullable = IsNullable(targetProp.Type);
            if (isTargetNullable)
            {
                var targetTypeArgument = (targetProp.Type as INamedTypeSymbol)?.TypeArguments[0];
                return sourceProp.Type.ContainingNamespace.Name == targetTypeArgument.ContainingNamespace.Name
                    && sourceProp.Type.Name == targetTypeArgument.Name;
            }

            return false;
        }
    }
}
