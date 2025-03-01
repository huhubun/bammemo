namespace Bammemo.Service.Abstractions.Dtos;

public class SlipTagDto
{
    public required uint Id { get; set; }
    public required uint SlipId { get; set; }
    public required string Tag { get; set; }
}
