using Microsoft.AspNetCore.Mvc;

namespace Profile.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new RedirectResult("~/swagger");
        }
    }
}