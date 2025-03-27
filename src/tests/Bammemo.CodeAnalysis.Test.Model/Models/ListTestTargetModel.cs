using Bammemo.CodeAnalysis.Test.Model.Enums;

namespace Bammemo.CodeAnalysis.Test.Model.Models;

public class ListTestTargetModel
{
    public List<TargetModel>? SourceModelListToTargetModelList { get; set; }
    public List<int>? IntListToIntList { get; set; }
    public List<string>? StringListToStringList { get; set; }
    public List<TestType>? IntListToEnumList { get; set; }
    public List<int>? EnumListToIntList { get; set; }

}
