using System.Text.Json;
using System.Linq.Dynamic.Core;
using RuleEngine.App.Models;
using RuleEngine.App.Models.RuleModels;
using RuleEngine.App.Models.RuleParameterModels;

namespace RuleEngine.App.Utils
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

    public static bool EvaluateRuleCondition(object? requestVariables, string condition)
    {
      ArgumentNullException.ThrowIfNull(requestVariables, nameof(requestVariables));

      var lambda = DynamicExpressionParser.ParseLambda(
          requestVariables.GetType(), typeof(bool), condition);

      var result = lambda.Compile().DynamicInvoke(requestVariables);
      return (bool?)result ?? false;
    }
  }
}