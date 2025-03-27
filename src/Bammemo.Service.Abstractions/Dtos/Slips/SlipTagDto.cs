namespace Bammemo.Service.Abstractions.Dtos.Slips;

public class SlipTagDto
{
    public required int Id { get; set; }
    public required int SlipId { get; set; }
    public required string Tag { get; set; }
}
