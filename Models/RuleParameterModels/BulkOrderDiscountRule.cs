using System.Text.Json.Serialization;

namespace RuleEngine.App.Models.RuleParameterModels
{
  public class BulkOrderDiscountRule : BaseRule
  {
    [JsonPropertyName("ItemCount")]
    public int ItemCount { get; set; }
  }
}