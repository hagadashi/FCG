using Microsoft.AspNetCore.Mvc;

namespace FCG.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(T result)
    {
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }
    
    protected IActionResult HandleResult<T>(T result, string message)
    {
        if (result == null)
            return NotFound(new { message });
            
        return Ok(new { data = result, message });
    }

}