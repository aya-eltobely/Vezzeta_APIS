using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VezetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        [Authorize]

    public class TestController : ControllerBase
    {

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult getstring()
        {
            return Ok("Hello");
        }
    }
}
