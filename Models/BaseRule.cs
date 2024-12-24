namespace RuleEngine.App.Models
{
  public abstract class BaseRule
  {
    public string RuleName { get; set; } = string.Empty;
    public string SuccessMessage { get; set; } = string.Empty;
    public string FailureMessage { get; set; } = string.Empty;
    public List<Subrule> Subrules { get; set; } = [];
  }
}
