namespace RuleEngine.App.Models
{
  public class Subrule
  {
    public string SubruleName { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string SuccessMessage { get; set; } = string.Empty;
    public string FailureMessage { get; set; } = string.Empty;
  }
}
