using System.Net.Http.Headers;
using Polly.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services
    .AddHttpClient<App>(x =>
    {
        x.BaseAddress = new Uri("http://localhost:5248/");
        x.DefaultRequestHeaders.Accept.Clear();
        x.Timeout=TimeSpan.FromSeconds(2);
        x.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    });
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// app.MapGet("/retry", async () =>
//     {
//
//         var retryPolicy = Policy
//             .Handle<Exception>()
//             .RetryAsync(5, onRetry: (exception, retryCount) =>
//             {
//                 Console.WriteLine("Error : "+ exception.Message + "... RetryCount "+ retryCount);
//             });
//         
//         var res =await retryPolicy.ExecuteAsync(async () =>
//         {
//             HttpClient httpClient = new HttpClient();
//             var message = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/userss");
//             var content = message.Content.ReadAsStringAsync();
//             Console.WriteLine(content.Result);
//             if (!message.IsSuccessStatusCode)
//                 throw new Exception(content.Exception?.Message??"Beklenmeyen bir hata oluştu");
//             return "success";
//         });
//
//         return res;
//     })
//     .WithName("retry")
//     .WithOpenApi();
//
// app.MapGet("/waitAndRetry", async () =>
//     {
//         var amountToPause = TimeSpan.FromSeconds(4);
//         var retryPolicy = Policy
//             .Handle<Exception>()
//             .WaitAndRetryAsync(5,i=>amountToPause, onRetry: (exception, retryCount) =>
//             {
//                 Console.WriteLine("Error : "+ exception.Message + "... RetryCount "+ retryCount + ".. Wait second" + amountToPause);
//             });
//         
//         var res =await retryPolicy.ExecuteAsync(async () =>
//         {
//             HttpClient httpClient = new HttpClient();
//             var message = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/userss");
//             var content = message.Content.ReadAsStringAsync();
//             Console.WriteLine(content.Result);
//             if (!message.IsSuccessStatusCode)
//                 throw new Exception(content.Exception?.Message??"Beklenmeyen bir hata oluştu");
//             return "success";
//         });
//
//         return res;
//     })
//     .WithName("waitAndRetry")
//     .WithOpenApi();
//
//
// app.MapGet("/circuitBreakerWaitAndRetry", async () =>
//     {
//         var amountToPause = TimeSpan.FromSeconds(4);
//         var retryPolicy = Policy
//             .Handle<Exception>()
//             .WaitAndRetryAsync(5,i=>amountToPause, onRetry: (exception, retryCount) =>
//             {
//                 Console.WriteLine("Error : "+ exception.Message + "... RetryCount "+ retryCount + ".. Wait second" + amountToPause);
//             });
//
//         var circuitBreakerPolicy = Policy.Handle<Exception>()
//             .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
//
//         var finallyPolicy = retryPolicy.WrapAsync(circuitBreakerPolicy);
//         
//         var res =await finallyPolicy.ExecuteAsync(async () =>
//         {
//             HttpClient httpClient = new HttpClient();
//             var message = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/userss");
//             var content = message.Content.ReadAsStringAsync();
//             Console.WriteLine(content.Result);
//             if (!message.IsSuccessStatusCode)
//                 throw new Exception(content.Exception?.Message??"Beklenmeyen bir hata oluştu");
//             return "success";
//         });
//
//         return res;
//     })
//     .WithName("circuitBreakerWaitAndRetry")
//     .WithOpenApi();

app.UseRouting();
app.MapControllers();
app.Run();
