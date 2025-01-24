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
    /// 进行中（仅自己可见）
    /// </summary>
    Workspace,

    /// <summary>
    /// 归档（仅自己可见）
    /// </summary>
    Article
}
