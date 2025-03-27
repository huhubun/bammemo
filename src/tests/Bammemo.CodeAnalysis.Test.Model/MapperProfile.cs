using Bammemo.CodeAnalysis.Test.Model.Models;
using Bammemo.Service.Abstractions.Attributes;

namespace Bammemo.CodeAnalysis.Test.Model;

[Map<ArrayTestSourceModel, ArrayTestTargetModel>]
[Map<ListTestSourceModel, ListTestTargetModel>]
[Map<SourceModel, TargetModel>]
[Map<SourceModel, TargetDifferentPropertyNameModel>]
[Map<EnumTestSourceModel, EnumTestTargetModel>]
[Map<NullableTestSourceModel, NullableTestTargetModel>]
[Map<NestedTypeSourceModel.NestedTypeSourceModelInside, TargetModel>]
public static partial class MapperProfile
{
    static partial void AfterMap(SourceModel source, TargetDifferentPropertyNameModel target)
    {
        if(source.Name?.Contains(' ') ?? false)
        {
            var names = source.Name.Split(' ');
            target.FirstName = names[0];
            target.LastName = names[1];
        }
        else
        {
            target.FirstName = source.Name;
        }
    }
}
