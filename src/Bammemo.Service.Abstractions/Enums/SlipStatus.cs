namespace Bammemo.Service.Abstractions.Enums;

public enum SlipStatus
{
    /// <summary>
    /// 仅自己可见
    /// </summary>
    Private,

    /// <summary>
    /// 公开
    /// </summary>
    Public,

    /// <summary>
    /// 草稿（仅自己可见）
    /// </summary>
    Draft,

    /// <summary>
    /// 归档（仅自己可见）
    /// </summary>
    Archive
}
