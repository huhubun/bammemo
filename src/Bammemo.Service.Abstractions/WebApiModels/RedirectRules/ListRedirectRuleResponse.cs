namespace Bammemo.Service.Abstractions.WebApiModels.RedirectRules
{
    public class ListRedirectRuleResponse
    {
        public required RedirectRuleModel[] RedirectRules { get; set; }

        public class RedirectRuleModel
        {
            public int Id { get; set; }
            public required string Source { get; set; }
            public required string Target { get; set; }
            public int HttpStatus { get; set; }
        }
    }
}
