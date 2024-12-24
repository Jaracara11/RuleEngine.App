using RuleEngine.App.Models;
using System.Text.Json;
using RuleEngine.App.Core;
using RuleEngine.App;

var builder = WebApplication.CreateBuilder(args);
var rulesFilePath = builder.Configuration["RulesV1FilePath"];
var json = File.ReadAllText(rulesFilePath ?? string.Empty);
var rules = JsonSerializer.Deserialize<List<Rule>>(json) ?? [];

builder.Services.AddSingleton(rules.Cast<BaseRule>().ToList());
builder.Services.AddScoped<RequestHandlerV1>();

builder.Services.AddCors(options =>
{
  options.AddPolicy("CorsPolicy",
      builder => builder.AllowAnyOrigin()
                        .WithMethods("POST", "GET")
                        .WithHeaders("Content-Type", "Accept"));
});

var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseCors();

app.MapGet("/v1/rules", () =>
{
  return Results.Ok(rules);
});

app.MapPost("/v1/evaluate-rule", async (HttpContext httpContext, RequestHandlerV1 requestHandler) =>
{
  return await requestHandler.HandleRuleEvaluationRequest(httpContext);
});

app.Run();