using Microsoft.AspNetCore.Mvc;

namespace Test.API.Controller;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestController:ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetTest()
    {
        Console.WriteLine("istek geldi");
        await Task.Delay(10000);
        throw new Exception();
        return Ok(new { test = "test" });
    }
}