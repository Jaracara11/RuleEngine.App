using System.Text.Json.Serialization;

namespace RuleEngine.App.Models.RuleParameterModels
{
  public class LargeTransactionRule : BaseRule
  {
    [JsonPropertyName("Amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("Category")]
    public string Category { get; set; } = string.Empty;
  }
}