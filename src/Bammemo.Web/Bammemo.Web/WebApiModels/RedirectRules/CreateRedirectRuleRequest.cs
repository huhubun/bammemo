namespace Bammemo.Web.WebApiModels.RedirectRules;

public class CreateRedirectRuleRequest
{
    public required string Source { get; set; }
    public required string Target { get; set; }
    public int HttpStatus { get; set; }
}
