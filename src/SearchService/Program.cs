
using System.Net;
using MongoDB.Driver;
using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Models;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IMongoClient>(
    sp=>new MongoClient(
        builder.Configuration.GetConnectionString("MongoDbConnection")
    )
);


builder.Services.AddControllers();

builder.Services
.AddHttpClient<AuctionSvcHttpClient>()
.AddPolicyHandler(GetPolicy());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();



// Configure the HTTP request pipeline.



static IAsyncPolicy<HttpResponseMessage>GetPolicy()=>
        HttpPolicyExtensions.HandleTransientHttpError()
        .OrResult(msg=>msg.StatusCode==HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_=>TimeSpan.FromSeconds(3));

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async() =>
{
    //for seeding the initial Data 
try
{
    await DbInitializer.InitDb(app);
}
catch (Exception e)
{
    
    Console.WriteLine(e);
}
});

app.Run();




