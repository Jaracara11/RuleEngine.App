namespace RuleEngine.App.Models
{
  public class SubruleEvaluationResult
  {
    public required string SubruleName { get; set; }
    public required string Condition { get; set; }
    public bool EvaluationResult { get; set; }
    public required string Message { get; set; }
  }
}
