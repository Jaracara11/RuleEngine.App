using RuleEngine.App.Models;
using System.Text.Json;
using RuleEngine.App.Utils;

var builder = WebApplication.CreateBuilder(args);

var rulesFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "rules.json");
var json = File.ReadAllText(rulesFilePath);
var rules = JsonSerializer.Deserialize<List<Rule>>(json) ?? [];

builder.Services.AddSingleton<List<BaseRule>>(rules.Cast<BaseRule>().ToList());
builder.Services.AddScoped<RequestHandlerV1>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapPost("/v1/evaluate-rule", async (HttpContext httpContext, RequestHandlerV1 requestHandler) =>
{
  return await requestHandler.HandleRuleEvaluationRequest(httpContext);
});

app.Run();
