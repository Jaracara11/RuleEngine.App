using System.Text.Json;
using RuleEngine.App.Models;

namespace RuleEngine.App.Utils
{
  public class RequestHandlerV1(List<BaseRule> rules)
  {
    private readonly List<BaseRule> _rules = rules;

    public async Task<IResult> HandleRuleEvaluationRequest(HttpContext httpContext)
    {
      var requestBody = await JsonSerializer.DeserializeAsync<RuleEvaluationRequest>(httpContext.Request.Body);

      if (requestBody == null)
      {
        return Results.BadRequest(new { Error = "Invalid request body." });
      }

      if (!Enum.TryParse(requestBody.RuleName, out RuleName ruleName))
      {
        return Results.BadRequest(new { Error = "Invalid rule name." });
      }

      var rule = _rules.FirstOrDefault(r => r.RuleName == ruleName.ToString());

      if (rule == null)
      {
        return Results.NotFound(new { Error = $"Rule '{requestBody.RuleName}' not found." });
      }

      var requestVariables = RuleEvaluator.DeserializeRequestVariables
      (ruleName, JsonSerializer.Serialize(requestBody.RequestVariables));

      bool evaluationResult = RuleEvaluator.EvaluateRuleCondition(requestVariables, rule.Condition);

      var result = new RuleEvaluationResult
      {
        RuleName = requestBody.RuleName,
        EvaluationResult = evaluationResult,
        IsSuccess = evaluationResult,
        Message = evaluationResult ? rule.SuccessMessage : rule.FailureMessage
      };

      return Results.Ok(result);
    }
  }
}