namespace RuleEngine.App.Models.RuleModels
{
  public class BulkOrderDiscountRule : BaseRule
  {
    public List<string> Items { get; set; } = [];
  }
}