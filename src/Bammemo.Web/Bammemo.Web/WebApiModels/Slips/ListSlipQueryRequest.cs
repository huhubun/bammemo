﻿namespace Bammemo.Web.WebApiModels.Slips;

public class ListSlipQueryRequest
{
    public long? StartTime { get; set; }
    public long? EndTime { get; set; }
    public string[]? Tags { get; set; }
    public int[]? Status { get; set; }
    public string? Keyword { get; set; }
}
