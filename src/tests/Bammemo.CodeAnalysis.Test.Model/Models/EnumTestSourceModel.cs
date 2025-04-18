﻿using Bammemo.CodeAnalysis.Test.Model.Enums;

namespace Bammemo.CodeAnalysis.Test.Model.Models;

public class EnumTestSourceModel
{
    public TestType TestType { get; set; }
    public TestType ToNullableInt{ get; set; }
    public TestType ToNullableEnum{ get; set; }
    public int TestTypeValue { get; set; }
}
