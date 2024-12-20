using System.Text.Json.Serialization;

namespace RuleEngine.App.Models
{
  public class RuleEvaluationResult
  {
    [JsonPropertyName("RuleName")]
    public required string RuleName { get; set; }

    [JsonPropertyName("EvaluationResult")]
    public bool EvaluationResult { get; set; }

    [JsonPropertyName("IsSuccess")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("Message")]
    public required string Message { get; set; }
  }
}