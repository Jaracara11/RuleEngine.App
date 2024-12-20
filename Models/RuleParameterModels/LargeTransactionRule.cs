namespace RuleEngine.App.Models.RuleModels
{
  public class LargeTransactionRule : BaseRule
  {
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
  }
}