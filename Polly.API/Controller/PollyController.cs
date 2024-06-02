using Microsoft.AspNetCore.Mvc;

namespace Polly.API.Controller;

[Route("api/[controller]/[action]")]
[ApiController]
public class PollyController(App _app):ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetCall()
    {
        var res = await _app.GetResponse();
        return Ok(res);
    }
    
}