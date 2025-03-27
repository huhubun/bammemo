using Bammemo.CodeAnalysis.Test.Model.Models;
using Bammemo.CodeAnalysis.Test.Model;
using Bammemo.CodeAnalysis.Test.Model.Enums;

namespace Bammemo.CodeAnalysis.Test;

public class MapperGeneratorTest
{
    [Fact]
    public void SameNamePropertyMapTest()
    {
        var source = new SourceModel
        {
            Id = 1,
            Name = "Foo",
            Age = 18,
            CreateTime = new DateTime(2025, 2, 10),
            Remark = nameof(SourceModel.Remark)
        };

        var target = source.MapTo<TargetModel>();

        Assert.NotNull(target);
        Assert.Equal(source.Id, target.Id);
        Assert.Equal(source.Name, target.Name);
        Assert.Equal(source.Age, target.Age);
        Assert.Equal(source.CreateTime, target.CreateTime);
        Assert.Null(target.Note);
    }

    [Fact]
    public void ExistedSameNamePropertyMapTest()
    {
        var source = new SourceModel
        {
            Id = 1,
            Name = "Foo",
            CreateTime = new DateTime(2025, 2, 10),
            Remark = nameof(SourceModel.Remark)
        };

        var target = new TargetModel
        {
            Note = nameof(TargetModel.Note)
        };

        target = source.MapTo(target);

        Assert.NotNull(target);
        Assert.Equal(source.Id, target.Id);
        Assert.Equal(source.Name, target.Name);
        Assert.Equal(source.CreateTime, target.CreateTime);
        Assert.Equal(nameof(TargetModel.Note), target.Note);
    }

    [Fact]
    public async Task ThrowNotSupportedException_WhenNotMappedTest()
    {
        var source = new SourceModel
        {
            Id = 1,
            Name = "Foo",
            CreateTime = new DateTime(2025, 2, 10),
            Remark = nameof(SourceModel.Remark)
        };

        await Assert.ThrowsAsync<NotSupportedException>(() => Task.FromResult(source.MapTo<NotMappedModel>()));
    }

    [Fact]
    public void AfterMapTest()
    {
        const string FIRST_NAME = "Foo";
        const string LAST_NAME = "Bar";

        var source = new SourceModel
        {
            Id = 1,
            Name = $"{FIRST_NAME} {LAST_NAME}",
            CreateTime = new DateTime(2025, 2, 10),
            Remark = nameof(SourceModel.Remark)
        };

        var target = source.MapTo<TargetDifferentPropertyNameModel>();

        Assert.NotNull(target);
        Assert.Equal(source.Id, target.Id);
        Assert.Equal(FIRST_NAME, target.FirstName);
        Assert.Equal(LAST_NAME, target.LastName);
        Assert.Equal(source.CreateTime, target.CreateTime);
    }

    [Fact]
    public void ListMapTest()
    {
        var sourceList = new List<SourceModel>
        {
            new()
            {
                Id = 1,
                Name = "Foo",
                CreateTime = new DateTime(2025, 2, 10),
                Remark = nameof(SourceModel.Remark)
            },
            new()
            {
                Id = 2,
                Name = "Bar",
                CreateTime = new DateTime(2026, 2, 10),
                Remark = nameof(SourceModel.Remark) + "2"
            }
        };

        var targetList = sourceList.MapToList<TargetModel>();

        Assert.NotNull(targetList);
        Assert.Equal(sourceList.Count, targetList.Count);

        for (var i = 0; i < sourceList.Count; i++)
        {
            Assert.Equal(sourceList[i].Id, targetList[i].Id);
            Assert.Equal(sourceList[i].Name, targetList[i].Name);
            Assert.Equal(sourceList[i].CreateTime, targetList[i].CreateTime);
            Assert.Null(targetList[i].Note);
        }
    }

    [Fact]
    public void ListMapByPropertyTest()
    {
        var source = new ListTestSourceModel
        {
            SourceModelListToTargetModelList =
            [
                new()
                {
                    Id = 1,
                    Name = "Foo",
                    CreateTime = new DateTime(2025, 2, 10),
                    Remark = nameof(SourceModel.Remark)
                },
                new()
                {
                    Id = 2,
                    Name = "Bar",
                    CreateTime = new DateTime(2026, 2, 10),
                    Remark = nameof(SourceModel.Remark) + "2"
                }
            ],
            IntListToIntList = [3, 2, 1],
            StringListToStringList = ["c", "b", "a"],
            EnumListToIntList = [TestType.Second, TestType.First],
            IntListToEnumList = [(int)TestType.Second, (int)TestType.First]
        };

        var target = source.MapTo<ListTestTargetModel>();

        Assert.NotNull(target);
        Assert.NotNull(target.SourceModelListToTargetModelList);
        Assert.Equal(source.SourceModelListToTargetModelList.Count, target.SourceModelListToTargetModelList.Count);

        for (var i = 0; i < source.SourceModelListToTargetModelList.Count; i++)
        {
            Assert.Equal(source.SourceModelListToTargetModelList[i].Id, target.SourceModelListToTargetModelList[i].Id);
            Assert.Equal(source.SourceModelListToTargetModelList[i].Name, target.SourceModelListToTargetModelList[i].Name);
            Assert.Equal(source.SourceModelListToTargetModelList[i].CreateTime, target.SourceModelListToTargetModelList[i].CreateTime);
            Assert.Null(target.SourceModelListToTargetModelList[i].Note);
        }

        Assert.Equal(source.IntListToIntList, target.IntListToIntList);
        Assert.Equal(source.StringListToStringList, target.StringListToStringList);
        
        for(var i = 0;i < source.EnumListToIntList.Count; i++)
        {
            Assert.Equal((int)source.EnumListToIntList[i], target.EnumListToIntList[i]);
        }

        for (var i = 0; i < source.IntListToEnumList.Count; i++)
        {
            Assert.Equal(source.IntListToEnumList[i], (int)target.IntListToEnumList[i]);
        }
    }

    [Fact]
    public void ArrayMapByPropertyTest()
    {
        var source = new ArrayTestSourceModel
        {
            Array =
            [
                new()
                {
                    Id = 1,
                    Name = "Foo",
                    CreateTime = new DateTime(2025, 2, 10),
                    Remark = nameof(SourceModel.Remark)
                },
                new()
                {
                    Id = 2,
                    Name = "Bar",
                    CreateTime = new DateTime(2026, 2, 10),
                    Remark = nameof(SourceModel.Remark) + "2"
                }
            ]
        };

        var target = source.MapTo<ArrayTestTargetModel>();

        Assert.NotNull(target);
        Assert.NotNull(target.Array);
        Assert.Equal(source.Array.Length, target.Array.Length);

        for (var i = 0; i < source.Array.Length; i++)
        {
            Assert.Equal(source.Array[i].Id, target.Array[i].Id);
            Assert.Equal(source.Array[i].Name, target.Array[i].Name);
            Assert.Equal(source.Array[i].CreateTime, target.Array[i].CreateTime);
            Assert.Null(target.Array[i].Note);
        }
    }

    [Fact]
    public void EnumMapTest()
    {
        var source = new EnumTestSourceModel
        {
            TestType = TestType.Second,
            ToNullableInt = TestType.Second,
            ToNullableEnum = TestType.Second,
            TestTypeValue = (int)TestType.Second
        };

        var target = source.MapTo<EnumTestTargetModel>();

        Assert.NotNull(target);
        Assert.Equal((int)TestType.Second, target.TestType);
        Assert.NotNull(target.ToNullableInt);
        Assert.Equal((int)TestType.Second, target.ToNullableInt.Value);
        Assert.NotNull(target.ToNullableEnum);
        Assert.Equal(TestType.Second, target.ToNullableEnum.Value);
        Assert.Equal(TestType.Second, target.TestTypeValue);
    }

    [Fact]
    public void NullablePropertyMapTest()
    {
        var source = new NullableTestSourceModel
        {
            SourceNotNullable = 1,
            SourceIsNullableAndNullTargetNeedsDefault = null,
            SourceIsNullableAndHasValue = 1
        };

        var target = source.MapTo<NullableTestTargetModel>();

        Assert.NotNull(target);
        Assert.Equal(source.SourceNotNullable, target.SourceNotNullable);
        Assert.Equal(default(int), target.SourceIsNullableAndNullTargetNeedsDefault);
        Assert.Equal(source.SourceIsNullableAndHasValue, target.SourceIsNullableAndHasValue);
    }

    [Fact]
    public void NestedTypeTest()
    {
        var source = new NestedTypeSourceModel.NestedTypeSourceModelInside
        {
            Id = 1,
            Name = "Foo"
        };

        var target = source.MapTo<TargetModel>();

        Assert.NotNull(target);
        Assert.Equal(source.Id, target.Id);
        Assert.Equal(source.Name, target.Name);
    }
}
