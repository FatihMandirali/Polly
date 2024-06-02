namespace Polly.API;

public class App
{
    private readonly HttpClient _httpClient;

    public App(HttpClient client)
    {
        _httpClient = client;
    }
    public async Task<object> GetResponse()
    {
        var retryPolicy = Policy
             .Handle<Exception>()
             .RetryAsync(5, onRetry: (exception, retryCount) =>
             {
                 Console.WriteLine("Error : "+ exception.Message + "... RetryCount "+ retryCount);
             });
        
        var circuitBreakerPolicy = Policy.Handle<Exception>()
            .AdvancedCircuitBreakerAsync(
                durationOfBreak: TimeSpan.FromSeconds(10),  // the circuit breaks for 10 seconds
                failureThreshold: 0.1,                      // if there is a 10% failure rate
                samplingDuration: TimeSpan.FromSeconds(60), // in a 60 second window
                minimumThroughput: 2                      // with a minimum of 100 requests
            );

        //var finallyPolicy = retryPolicy.WrapAsync(circuitBreakerPolicy);
        var wrapAsync = Policy.WrapAsync(retryPolicy,circuitBreakerPolicy);
        
        var res = await wrapAsync.ExecuteAsync(async ()=>  await _httpClient.GetFromJsonAsync<object>("api/Test/GetTest"));

        return res;
    }
}