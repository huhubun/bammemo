using Bammemo.CodeAnalysis.Test.Model.Enums;

namespace Bammemo.CodeAnalysis.Test.Model.Models;

public class EnumTestTargetModel
{
    public int TestType { get; set; }
    public int? ToNullableInt{ get; set; }
    public TestType? ToNullableEnum{ get; set; }
    public TestType TestTypeValue { get; set; }
}
