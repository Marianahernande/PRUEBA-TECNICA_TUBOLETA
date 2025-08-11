var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/healthz", () => Results.Ok("ok"));
app.MapGet("/", () => Results.Ok("Service up"));
app.MapGet("/smoke", () => Results.Ok("smoke: orders"));

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
