namespace Bammemo.Service.Abstractions.WebApiModels.Slips;

public class ListSlipResponse
{
    public required SlipModel[] Data { get; set; }

    public class SlipModel
    {
        public required string Id { get; set; }
        public required string Content { get; set; }
        public long CreatedAt { get; set; }
        public long? UpdateAt { get; set; }
        public string? FriendlyUrl { get; set; }
        public int Status { get; set; }
    }
}
