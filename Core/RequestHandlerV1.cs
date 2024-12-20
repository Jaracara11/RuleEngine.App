using System.Text.Json;
using RuleEngine.App.Models;

namespace RuleEngine.App.Core
{
  public class RequestHandlerV1(List<BaseRule> rules)
  {
    private readonly List<BaseRule> _rules = rules;

    public async Task<IResult> HandleRuleEvaluationRequest(HttpContext httpContext)
    {
      var requestBody = await JsonSerializer.DeserializeAsync<RuleEvaluationRequest>(httpContext.Request.Body);

      if (requestBody == null || string.IsNullOrEmpty(requestBody.RuleName) || requestBody.RequestVariables == null)
      {
        return Results.BadRequest(new { Error = "Invalid request data: Ensure RuleName and RequestVariables are provided." });
      }

      if (!Enum.TryParse<RuleName>(requestBody.RuleName, ignoreCase: true, out var ruleName))
      {
        return Results.BadRequest(new { Error = $"Invalid RuleName '{requestBody.RuleName}'. Ensure the value is a valid enum." });
      }

      var rule = _rules.FirstOrDefault(r => r.RuleName.Equals(ruleName.ToString(), StringComparison.OrdinalIgnoreCase));

      if (rule == null)
      {
        return Results.NotFound(new { Error = $"Rule '{requestBody.RuleName}' not found." });
      }

      var requestVariablesJson = JsonSerializer.Serialize(requestBody.RequestVariables);
      var requestVariables = RuleEvaluator.DeserializeRequestVariables(ruleName, requestVariablesJson);

      if (requestVariables == null)
      {
        return Results.BadRequest(new { Error = "Failed to deserialize request variables." });
      }

      var subruleResults = RuleEvaluator.EvaluateSubrules(requestVariables, rule.Subrules);

      var overallResult = subruleResults.All(x => x.EvaluationResult);
      var failureMessages = subruleResults.Where(x => !x.EvaluationResult)
                                           .Select(x => x.Message)
                                           .ToList();

      var result = new RuleEvaluationResult
      {
        RuleName = rule.RuleName,
        IsSuccess = overallResult,
        Message = failureMessages.Count > 0 ? string.Join(", ", failureMessages) : rule.SuccessMessage,
        SubruleResults = subruleResults
      };

      return Results.Ok(result);
    }
  }
}
