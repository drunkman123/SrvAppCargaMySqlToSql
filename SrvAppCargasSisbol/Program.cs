using Polly;
using Polly.Contrib.WaitAndRetry;
using SrvAppCargasSisbol.Data.Repositories;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod()
                                            .AllowAnyHeader()
                                            .SetIsOriginAllowedToAllowWildcardSubdomains();
                      });
});
builder.Services.AddTransient<CargasSisbolRepository>();
//builder.Services.AddHttpClient("HttpAlmanaque", client =>
//{
//    client.BaseAddress = new Uri("http://xxxxxxxxx");
//    //client.DefaultRequestHeaders.Add("Accept", "application/json");
//}
//)
//.AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 3)));   //jeito mais simples de fazer (faz 30 tentativas, 1 apos cada 5seg)
////.AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>()
////            .OrResult(x => x.StatusCode >= System.Net.HttpStatusCode.InternalServerError || x.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
////            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI();
    app.UseSwagger(x => x.SerializeAsV2 = true);
}

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
