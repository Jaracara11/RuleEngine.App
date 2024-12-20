namespace RuleEngine.App.Models
{
  public abstract class BaseRule
  {
    public int RuleID { get; set; }
    public string RuleName { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string SuccessMessage { get; set; } = string.Empty;
    public string FailureMessage { get; set; } = string.Empty;

    protected BaseRule() { }
  }
}
