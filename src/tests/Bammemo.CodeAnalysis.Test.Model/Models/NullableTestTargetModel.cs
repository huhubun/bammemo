namespace Bammemo.CodeAnalysis.Test.Model.Models;

public class NullableTestTargetModel
{
    public int? SourceNotNullable { get; set; }
    public int SourceIsNullableAndNullTargetNeedsDefault { get; set; }
    public int SourceIsNullableAndHasValue { get; set; }
}
