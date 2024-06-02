using Polly.CircuitBreaker;
using Polly.Retry;

namespace Polly.API;

public class App
{
    private readonly HttpClient _httpClient;

    // private static readonly AsyncRetryPolicy<HttpResponseMessage> TransientErrorRetryPolicy = Policy
    //     .HandleResult<HttpResponseMessage>(message => (int)message.StatusCode == 429 || (int)message.StatusCode > 500)
    //     .WaitAndRetryAsync(2, retryAttempt =>
    //     {
    //         Console.WriteLine($"Retrying because of transient error. Attempt {retryAttempt}");
    //         return TimeSpan.FromSeconds(Math.Pow(2,retryAttempt));
    //     });
    //
    // private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy = Policy
    //     .HandleResult<HttpResponseMessage>(message => (int)message.StatusCode == 429 || (int)message.StatusCode > 500)
    //     .CircuitBreakerAsync(2, TimeSpan.FromSeconds(10));
    
    private static readonly AsyncRetryPolicy<HttpResponseMessage> TransientErrorRetryPolicy = Policy
        .HandleResult<HttpResponseMessage>(message => (int)message.StatusCode != 200)
        .WaitAndRetryAsync(2, retryAttempt =>
        {
            Console.WriteLine($"Retrying because of transient error. Attempt {retryAttempt}");
            return TimeSpan.FromSeconds(2);
        });

    private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy = Policy
        .HandleResult<HttpResponseMessage>(message => (int)message.StatusCode != 200)
        .CircuitBreakerAsync(3, TimeSpan.FromSeconds(20));
    
    

    public App(HttpClient client)
    {
        _httpClient = client;
    }
    public async Task<object> GetResponse()
    {
        
        //TODO 1.YOL
        // var retryPolicy = Policy
        //      .Handle<Exception>()
        //      .RetryAsync(5, onRetry: (exception, retryCount) =>
        //      {
        //          Console.WriteLine("Error : "+ exception.Message + "... RetryCount "+ retryCount);
        //      });
        //
        // var circuitBreakerPolicy = Policy.Handle<Exception>()
        //      .CircuitBreakerAsync(3, TimeSpan.FromSeconds(50));
        //
        // var circuitBreakerPolicy = Policy.Handle<Exception>()
        //     .AdvancedCircuitBreakerAsync(
        //         durationOfBreak: TimeSpan.FromSeconds(10),  // the circuit breaks for 10 seconds
        //         failureThreshold: 0.1,                      // if there is a 10% failure rate
        //         samplingDuration: TimeSpan.FromSeconds(60), // in a 60 second window
        //         minimumThroughput: 2                      // with a minimum of 100 requests
        //     );

        //var finallyPolicy = retryPolicy.WrapAsync(circuitBreakerPolicy);
        //var wrapAsync = Policy.WrapAsync(retryPolicy,circuitBreakerPolicy);
        
        //var res = await wrapAsync.ExecuteAsync(async ()=>  await _httpClient.GetFromJsonAsync<object>("api/Test/GetTest"));
        
        //TODO: 2. YOL (PROGRAM.CS ÜZERİNDEN AYARLANDI)
        //var res = await _httpClient.GetFromJsonAsync<object>("api/Test/GetTest");
        
        //TODO: 3. YOL STATİC OLARAK INIT KISMINDA TANIMLANDI
        if (CircuitBreakerPolicy.CircuitState == CircuitState.Open)
        {
            throw new Exception("Service is currently unavailable");
        }
        
        var res = await CircuitBreakerPolicy.ExecuteAsync(async ()=> await TransientErrorRetryPolicy.ExecuteAsync(async ()=>await _httpClient.GetAsync("api/Test/GetTest")));

        if (!res.IsSuccessStatusCode)
            throw new Exception("Service is currently unavailable1");
    
        return res;
    }
}