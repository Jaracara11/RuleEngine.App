using System.Linq.Dynamic.Core;
using System.Text.Json;
using RuleEngine.App.Models;
using RuleEngine.App.Models.RuleModels;

namespace RuleEngine.App.Utils
{
  public class RequestHandlerV1(List<BaseRule> rules)
  {
    private readonly List<BaseRule> _rules = rules;

    public async Task<IResult> HandleRuleEvaluationRequest(HttpContext httpContext)
    {
      var requestBody = await JsonSerializer.DeserializeAsync<
      RuleEvaluationRequest>(httpContext.Request.Body);

      if (requestBody == null)
      {
        return Results.BadRequest(new { Error = "Request body is empty or invalid." });
      }

      var rule = _rules.FirstOrDefault(r => r.RuleName == requestBody.RuleName);

      if (rule == null)
      {
        return Results.NotFound(new { Error = $"Rule '{requestBody.RuleName}' not found." });
      }

      object? requestVariables = null;

      switch (requestBody.RuleName)
      {
        case "LargeTransactionRule":
          requestVariables = JsonSerializer.Deserialize<LargeTransactionRule>(JsonSerializer.Serialize(requestBody.RequestVariables));
          break;
        case "BulkOrderDiscount":
          requestVariables = JsonSerializer.Deserialize<BulkOrderDiscountRule>(JsonSerializer.Serialize(requestBody.RequestVariables));
          break;
        default:
          return Results.BadRequest(new { Error = "Invalid rule" });
      }

      if (requestVariables == null)
      {
        return Results.BadRequest(new { Error = "Deserialization resulted in a null object." });
      }

      var lambda = DynamicExpressionParser.ParseLambda(
        requestVariables.GetType(), typeof(bool), rule.Condition);

      var result = lambda.Compile().DynamicInvoke(requestVariables);

      var evaluationResult = new RuleEvaluationResult
      {
        RuleName = requestBody.RuleName,
        EvaluationResult = (bool?)result ?? false,
        IsSuccess = (bool?)result ?? false,
        Message = (bool?)result == true ? rule.SuccessMessage : rule.FailureMessage
      };

      return Results.Ok(evaluationResult);
    }
  }
}