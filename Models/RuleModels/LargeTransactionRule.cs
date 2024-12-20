namespace RuleEngine.App.Models
{
  public class LargeTransactionRule : BaseRule
  {
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
  }
}