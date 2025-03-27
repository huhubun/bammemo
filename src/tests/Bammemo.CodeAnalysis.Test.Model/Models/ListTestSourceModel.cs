using Bammemo.CodeAnalysis.Test.Model.Enums;

namespace Bammemo.CodeAnalysis.Test.Model.Models;

public class ListTestSourceModel
{
    public List<SourceModel>? SourceModelListToTargetModelList { get; set; }
    public List<int>? IntListToIntList { get; set; }
    public List<string>? StringListToStringList { get; set; }
    public List<int>? IntListToEnumList { get; set; }
    public List<TestType>? EnumListToIntList { get; set; }
}
