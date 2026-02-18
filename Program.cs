using Nedo.AspNet.ApiContracts.Extensions;
using Nedo.AspNet.ApiContracts.Swagger;
using Nedo.AspNet.Request.Enrichment.DependencyInjection;
using Nedo.AspNet.Request.Enrichment.Middleware;
using Nedo.AspNet.Request.InputValidation;
using Nedo.AspNet.Request.Validation.Extensions;
using Nedo.Asp.Boilerplate.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiContracts();
builder.Services.AddRequestEnrichment(enrichment =>
{
    enrichment.AddRequestId();
    enrichment.AddCorrelationId();
    enrichment.AddClientIp();
    enrichment.AddRequestTime();
    enrichment.AddUserInformation();
});

builder.Services.AddRequestInputValidation(
    builder.Configuration.GetSection("NedoRequestInputValidation")
);

builder.Services.AddRequestValidation();
builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<StandardHeaderFilter>();
    options.AddStandardAuthorization();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();
