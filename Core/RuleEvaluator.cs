using System.Linq.Dynamic.Core;
using System.Text.Json;
using RuleEngine.App.Models;
using RuleEngine.App.Models.RuleParameterModels;

namespace RuleEngine.App.Core
{
  public static class RuleEvaluator
  {
    public static object? DeserializeRequestVariables(RuleName ruleName, string requestVariablesJson)
    {
      return ruleName switch
      {
        RuleName.LargeTransactionRule => JsonSerializer.Deserialize<LargeTransactionRule>(requestVariablesJson),
        RuleName.BulkOrderDiscount => JsonSerializer.Deserialize<BulkOrderDiscountRule>(requestVariablesJson),
        _ => null
      };
    }

    public static List<SubruleEvaluationResult> EvaluateSubrules(object requestVariables, List<Subrule> subrules)
    {
      return subrules.Select(x =>
      {
        var lambda = DynamicExpressionParser.ParseLambda(
                  requestVariables.GetType(), typeof(bool), x.Condition);

        var result = lambda.Compile().DynamicInvoke(requestVariables);

        return new SubruleEvaluationResult
        {
          SubruleName = x.SubruleName,
          Condition = x.Condition,
          EvaluationResult = (bool?)result == true,
          Message = (bool?)result == true ? x.SuccessMessage : x.FailureMessage
        };
      }).ToList();
    }
  }
}