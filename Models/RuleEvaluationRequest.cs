using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuleEngine.App.Models
{
    public class RuleEvaluationRequest
    {
        [Required(ErrorMessage = "The rule name to evaluate is required.")]
        [JsonPropertyName("RuleName")]
        public string RuleName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The rule requires at least 1 parameter.")]
        [JsonPropertyName("RequestVariables")]
        public Dictionary<string, object> RequestVariables { get; set; } = [];
    }
}