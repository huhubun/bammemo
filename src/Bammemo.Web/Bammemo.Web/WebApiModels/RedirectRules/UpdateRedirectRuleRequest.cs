namespace Bammemo.Web.WebApiModels.RedirectRules;

public class UpdateRedirectRuleRequest
{
    public required string Source { get; set; }
    public required string Target { get; set; }
    public int HttpStatus { get; set; }
}
