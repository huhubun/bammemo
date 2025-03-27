using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
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

                            // TODO 避免浅拷贝
                        }
                        else if (targetProp != null)
                        {
                            // 两边都是集合（List 或数组）
                            if (IsEnumerable(
                                    sourceProp,
                                    targetProp,
                                    out var sourceEnumerableType,
                                    out var sourceEnumerableKind,
                                    out var targetEnumerableType,
                                    out var targetEnumerableKind))
                            {
                                var isSourceBasicType = IsBasicType(sourceEnumerableType);
                                var isTargetBasicType = IsBasicType(targetEnumerableType);

                                string toMethodName;
                                switch (targetEnumerableKind)
                                {
                                    case TypeKind.Class:
                                        toMethodName = "ToList";
                                        break;
                                    case TypeKind.Array:
                                        toMethodName = "ToArray";
                                        break;
                                    default:
                                        // 不支持的情况，直接跳过当前循环
                                        continue;
                                }

                                // 如果两边都是基础类型（int、string、枚举等）
                                if (isSourceBasicType && isTargetBasicType)
                                {
                                    mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name}.Cast<{targetEnumerableType.ToDisplayString()}>().{toMethodName}(),");
                                    mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name}.Cast<{targetEnumerableType.ToDisplayString()}>().{toMethodName}();");
                                }
                                // 如果两边都是 class，直接调用 MapTo() 方法，只要添加了 Map 就能够映射
                                else if (!isSourceBasicType && !isTargetBasicType)
                                {
                                    mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name}.Map{toMethodName}<{targetEnumerableType.ToDisplayString()}>(),");
                                    mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name}.Map{toMethodName}<{targetEnumerableType.ToDisplayString()}>();");
                                }
                                else
                                {
                                    // 其它情况不支持
                                    // TODO 可空类型？
                                }
                            }
                            else
                            {
                                var isSourceNullable = IsNullable(sourceProp, out var sourceNullableTypeArgument);
                                var isTargetNullable = IsNullable(targetProp, out var targetNullableTypeArgument);

                                // Source 和 Target 都是不可空类型
                                if (!isSourceNullable && !isTargetNullable)
                                {
                                    var isSourceEnum = IsEnum(sourceProp.Type);
                                    var isTargetEnum = IsEnum(targetProp.Type);

                                    // 两边都是枚举，直接强转（这里不用考虑是两个一样类型的情况，因为最前面已经处理过“类型完全相同”的情况了）
                                    if (isSourceEnum && isTargetEnum)
                                    {
                                        mappingNew.Add($"{targetProp.Name} = ({targetProp.Type.ToDisplayString()})((int)source.{sourceProp.Name}),");
                                        mappingEqual.Add($"target.{targetProp.Name} = ({targetProp.Type.ToDisplayString()})((int)source.{sourceProp.Name});");
                                    }
                                    // 两边都不是枚举，例如 int -> long，直接强转
                                    else if (!isSourceEnum && !isTargetEnum)
                                    {
                                        // 如果是从 string 转换，例如 string -> int，需要用 Parse 方法
                                        if (IsString(sourceProp.Type) && targetProp.Type.GetMembers().Any(m => m.Name == "Parse"))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name} == null ? default : {targetNullableTypeArgument.ToDisplayString()}.Parse(source.{sourceProp.Name}),");
                                            mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name} == null ? default : {targetNullableTypeArgument.ToDisplayString()}.Parse(source.{sourceProp.Name});");
                                        }
                                        // 如果目标是 string ，例如 int -> string，需要用 ToString() 方法
                                        else if (IsString(targetProp.Type) && sourceProp.Type.GetMembers().Any(m => m.Name == "ToString"))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name}.ToString(),");
                                            mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name}.ToString();");
                                        }
                                        else
                                        {
                                            mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name}.MapTo<{targetProp.Type.ToDisplayString()}>(),");
                                            mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name}.MapTo<{targetProp.Type.ToDisplayString()}>();");
                                        }
                                    }
                                    // Source 不是枚举（仅限 Int32），Target 是枚举
                                    else if (!isSourceEnum && isTargetEnum)
                                    {
                                        if (IsInt32(sourceProp.Type))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = ({targetProp.Type.ToDisplayString()})source.{sourceProp.Name},");
                                            mappingEqual.Add($"target.{targetProp.Name} = ({targetProp.Type.ToDisplayString()})source.{sourceProp.Name};");
                                        }
                                        else
                                        {
                                            // Not support yet
                                        }
                                    }
                                    // Source 是枚举，Target 不是枚举（仅限 Int32）
                                    else if (isSourceEnum && !isTargetEnum)
                                    {
                                        if (IsInt32(targetProp.Type))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = (int)source.{sourceProp.Name},");
                                            mappingEqual.Add($"target.{targetProp.Name} = (int)source.{sourceProp.Name};");
                                        }
                                        else
                                        {
                                            // Not support yet
                                        }
                                    }
                                }
                                // Source 和 Target 都是可空类型
                                else if (isSourceNullable && isTargetNullable)
                                {
                                    var isSourceEnum = IsEnum(sourceNullableTypeArgument);
                                    var isTargetEnum = IsEnum(targetNullableTypeArgument);

                                    // 两边都是枚举，直接强转（这里不用考虑是两个一样类型的情况，因为最前面已经处理过“类型完全相同”的情况了）
                                    if (isSourceEnum && isTargetEnum)
                                    {
                                        mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name}.HasValue ? ({targetProp.Type.ToDisplayString()})((int)source.{sourceProp.Name}.Value) : default,");
                                        mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name}.HasValue ? ({targetProp.Type.ToDisplayString()})((int)source.{sourceProp.Name}.Value) : default;");
                                    }
                                    // 两边都不是枚举，例如 int? -> long?，直接强转
                                    else if (!isSourceEnum && !isTargetEnum)
                                    {
                                        mappingNew.Add($"{targetProp.Name} = ({targetProp.Type.ToDisplayString()})source.{sourceProp.Name},");
                                        mappingEqual.Add($"target.{targetProp.Name} = ({targetProp.Type.ToDisplayString()})source.{sourceProp.Name};");
                                    }
                                    // Source 不是枚举（仅限 Int32?），Target 是枚举
                                    else if (!isSourceEnum && isTargetEnum)
                                    {
                                        if (IsInt32(sourceNullableTypeArgument))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name}.HasValue ?   ({targetNullableTypeArgument.ToDisplayString()})source.{sourceProp.Name}.Value : default,");
                                            mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name}.HasValue ?   ({targetNullableTypeArgument.ToDisplayString()})source.{sourceProp.Name}.Value : default;");
                                        }
                                        else
                                        {
                                            // Not support yet
                                        }
                                    }
                                    // Source 是枚举，Target 不是枚举（仅限 Int32?）
                                    else if (isSourceEnum && !isTargetEnum)
                                    {
                                        if (IsInt32(targetNullableTypeArgument))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = (int?)source.{sourceProp.Name},");
                                            mappingEqual.Add($"target.{targetProp.Name} = (int?)source.{sourceProp.Name};");
                                        }
                                        else
                                        {
                                            // Not support yet
                                        }
                                    }
                                }
                                // Source 是不可空类型，Target 是可空类型
                                else if (isSourceNullable == false && isTargetNullable == true)
                                {
                                    var isSourceEnum = IsEnum(sourceProp.Type);
                                    var isTargetEnum = IsEnum(targetNullableTypeArgument);

                                    // 两边都是枚举，直接强转
                                    if (isSourceEnum && isTargetEnum)
                                    {
                                        mappingNew.Add($"{targetProp.Name} = ({targetNullableTypeArgument.ToDisplayString()})((int)source.{sourceProp.Name}),");
                                        mappingEqual.Add($"target.{targetProp.Name} = ({targetNullableTypeArgument.ToDisplayString()})((int)source.{sourceProp.Name});");
                                    }
                                    // 两边都不是枚举，例如 int -> long?，直接强转
                                    else if (!isSourceEnum && !isTargetEnum)
                                    {
                                        // 如果是从 string 转换，例如 string -> int?，需要用 Parse 方法
                                        if (IsString(sourceProp.Type) && targetNullableTypeArgument.GetMembers().Any(m => m.Name == "Parse"))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name} == null ? default : {targetNullableTypeArgument.ToDisplayString()}.Parse(source.{sourceProp.Name}),");
                                            mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name} == null ? default : {targetNullableTypeArgument.ToDisplayString()}.Parse(source.{sourceProp.Name});");
                                        }
                                        else
                                        {
                                            mappingNew.Add($"{targetProp.Name} = ({targetNullableTypeArgument.ToDisplayString()})source.{sourceProp.Name},");
                                            mappingEqual.Add($"target.{targetProp.Name} = ({targetNullableTypeArgument.ToDisplayString()})source.{sourceProp.Name};");
                                        }
                                    }
                                    // Source 不是枚举（仅限 Int32），Target 是枚举，直接强转
                                    else if (!isSourceEnum && isTargetEnum)
                                    {
                                        if (IsInt32(sourceProp.Type))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = ({targetNullableTypeArgument.ToDisplayString()})source.{sourceProp.Name},");
                                            mappingEqual.Add($"target.{targetProp.Name} = ({targetNullableTypeArgument.ToDisplayString()})source.{sourceProp.Name};");
                                        }
                                        else
                                        {
                                            // Not support yet
                                        }
                                    }
                                    // Source 是枚举，Target 不是枚举（仅限 Int32?）
                                    else if (isSourceEnum && !isTargetEnum)
                                    {
                                        if (IsInt32(targetNullableTypeArgument))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = (int?)source.{sourceProp.Name},");
                                            mappingEqual.Add($"target.{targetProp.Name} = (int?)source.{sourceProp.Name};");
                                        }
                                        else
                                        {
                                            // Not support yet
                                        }
                                    }
                                }
                                // Source 是可空类型，Target 是不可空类型
                                else
                                {
                                    var isSourceEnum = IsEnum(sourceNullableTypeArgument);
                                    var isTargetEnum = IsEnum(targetProp.Type);

                                    // 两边都是枚举 EnumA? -> EnumB，直接强转
                                    if (isSourceEnum && isTargetEnum)
                                    {
                                        mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name}.HasValue ? ({targetProp.Type.ToDisplayString()})((int)source.{sourceProp.Name}.Value) : default,");
                                        mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name}.HasValue ? ({targetProp.Type.ToDisplayString()})((int)source.{sourceProp.Name}.Value) : default;");
                                    }
                                    // 两边都不是枚举，例如 int? -> long，直接强转
                                    else if (!isSourceEnum && !isTargetEnum)
                                    {
                                        // 如果目标是 string ，例如 int? -> string，需要用 ToString() 方法
                                        if (IsString(targetProp.Type) && sourceNullableTypeArgument.GetMembers().Any(m => m.Name == "ToString"))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name} == null ? default : source.{sourceProp.Name}.ToString(),");
                                            mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name} == null ? default : source.{sourceProp.Name}.ToString();");
                                        }
                                        else
                                        {
                                            mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name}.HasValue ? ({targetProp.Type.ToDisplayString()})source.{sourceProp.Name}.Value : default,");
                                            mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name}.HasValue ? ({targetProp.Type.ToDisplayString()})source.{sourceProp.Name}.Value : default;");
                                        }
                                    }
                                    // Source 不是枚举（仅限 Int32），Target 是枚举，int? -> EnumA，直接强转
                                    else if (!isSourceEnum && isTargetEnum)
                                    {
                                        if (IsInt32(sourceNullableTypeArgument))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name}.HasValue ? ({targetProp.Type.ToDisplayString()})source.{sourceProp.Name}.Value : default,");
                                            mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name}.HasValue ? ({targetProp.Type.ToDisplayString()})source.{sourceProp.Name}.Value : default;");
                                        }
                                        else
                                        {
                                            // Not support yet
                                        }
                                    }
                                    // Source 是枚举，Target 不是枚举（仅限 Int32），EnumA? -> int
                                    else if (isSourceEnum && !isTargetEnum)
                                    {
                                        if (IsInt32(targetNullableTypeArgument))
                                        {
                                            mappingNew.Add($"{targetProp.Name} = source.{sourceProp.Name}.HasValue ? (int)source.{sourceProp.Name} : default,");
                                            mappingEqual.Add($"target.{targetProp.Name} = source.{sourceProp.Name}.HasValue ? (int)source.{sourceProp.Name} : default;");
                                        }
                                        else
                                        {
                                            // Not support yet
                                        }
                                    }
                                }
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
                    {String.Join("\n                    ", mappingNew)}
                }};
            }}
            else
            {{
                {String.Join("\n                ", mappingEqual)}
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
            if(source == null)
            {{
                return default;
            }}

            {String.Join("\n                ", nullSwitchs)}

            throw new NotSupportedException($""Map {sourceFullName} to {{typeof(TDest)}}"");
        }}

        public static TDest MapTo<TDest>(this {sourceFullName} source, TDest dest) where TDest : class
        {{
            ArgumentNullException.ThrowIfNull(source);

            return dest switch
            {{
                {String.Join("\n                ", switchs)}
                _ => throw new NotSupportedException($""Map {sourceFullName} to {{typeof(TDest)}}"")
            }};
        }}

        public static List<TDest> MapToList<TDest>(this IEnumerable<{sourceFullName}> source) where TDest : class
            => source.Select(s => s.MapTo<TDest>()).ToList();

        public static TDest[] MapToArray<TDest>(this IEnumerable<{sourceFullName}> source) where TDest : class
            => source.Select(s => s.MapTo<TDest>()).ToArray();

        {String.Join("\n        ", sourceToTargetMethods)}

#endregion";

                allMethods.Add(method);
            }

            var mapperClass = $@"// <auto-generated/>
using System;

namespace {classSymbol.ContainingNamespace.ToDisplayString()}
{{
    public static partial class {classSymbol.Name}
    {{
        {String.Join("\n", allMethods)}
    }}
}}";

            context.AddSource($"{classSymbol.Name}.g.cs", SourceText.From(mapperClass, Encoding.UTF8));
        }

        private static bool IsNullable(ITypeSymbol typeSymbol)
            => typeSymbol.ContainingNamespace?.Name == "System" && typeSymbol.Name == "Nullable";

        private static bool IsNullable(IPropertySymbol sourceProp, out ITypeSymbol nullableTypeArgument)
        {
            var isNullable = IsNullable(sourceProp.Type);
            if (isNullable)
            {
                nullableTypeArgument = (sourceProp.Type as INamedTypeSymbol)?.TypeArguments[0];
                return true;
            }

            nullableTypeArgument = null;
            return false;
        }

        private static bool IsEnum(ITypeSymbol type)
            => type.TypeKind == TypeKind.Enum;

        private static bool IsInt32(ITypeSymbol type)
            => type.ContainingNamespace.Name == "System" && type.Name == "Int32";

        private static bool IsEnumerable(IPropertySymbol sourceProp, IPropertySymbol targetProp, out ITypeSymbol sourceEnumerableType, out TypeKind sourceEnumerableKind, out ITypeSymbol targetEnumerableType, out TypeKind targetEnumerableKind)
        {
            bool sourceIsEnumerable;
            bool targetIsEnumerable;

            if (IsArray(sourceProp.Type, out sourceEnumerableType))
            {
                sourceIsEnumerable = true;
                sourceEnumerableKind = TypeKind.Array;
            }
            else if (IsList(sourceProp.Type, out sourceEnumerableType))
            {
                sourceIsEnumerable = true;
                sourceEnumerableKind = TypeKind.Class;
            }
            else
            {
                sourceIsEnumerable = false;
                sourceEnumerableKind = TypeKind.Unknown;
            }

            if (IsArray(targetProp.Type, out targetEnumerableType))
            {
                targetIsEnumerable = true;
                targetEnumerableKind = TypeKind.Array;
            }
            else if (IsList(targetProp.Type, out targetEnumerableType))
            {
                targetIsEnumerable = true;
                targetEnumerableKind = TypeKind.Class;
            }
            else
            {
                targetIsEnumerable = false;
                targetEnumerableKind = TypeKind.Unknown;
            }

            return sourceIsEnumerable && targetIsEnumerable;
        }

        private static bool IsArray(ITypeSymbol typeSymbol, out ITypeSymbol arrayTypeSymbol)
        {
            if (typeSymbol.TypeKind == TypeKind.Array)
            {
                arrayTypeSymbol = (typeSymbol as IArrayTypeSymbol).ElementType;
                return true;
            }

            arrayTypeSymbol = null;
            return false;
        }

        private static bool IsList(ITypeSymbol typeSymbol, out ITypeSymbol listTypeSymbol)
        {
            if (typeSymbol.Interfaces.Any(i => i.ToDisplayString() == "System.Collections.IEnumerable"))
            {
                listTypeSymbol = (typeSymbol as INamedTypeSymbol).TypeArguments.FirstOrDefault();

                if (listTypeSymbol == null)
                {
                    return false;
                }

                return true;
            }

            listTypeSymbol = null;
            return false;
        }

        private static bool IsString(ITypeSymbol typeSymbol)
            => typeSymbol.ContainingNamespace.Name == "System" && typeSymbol.Name == "String";

        private static bool IsBasicType(ITypeSymbol typeSymbol)
            => typeSymbol.IsValueType || IsString(typeSymbol);
    }
}