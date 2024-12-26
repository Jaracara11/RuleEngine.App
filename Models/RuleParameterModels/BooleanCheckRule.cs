using System.Text.Json.Serialization;

namespace RuleEngine.App.Models.RuleParameterModels
{
  public class BooleanCheckRule : BaseRule
  {
    [JsonPropertyName("IsFlagEnabled")]
    public bool IsFlagEnabled { get; set; }
  }
}