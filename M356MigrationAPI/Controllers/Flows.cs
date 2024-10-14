using M356MigrationAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace M356MigrationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlowsController : Controller
    {
        private readonly FlowClient _flowClient;

        public FlowsController()
        {
            _flowClient = new FlowClient();
        }

        [HttpGet("environments")]
        public async Task<IActionResult> GetEnvironments()
        {
            var response = await _flowClient.GetEnvironmentsAsync();
            return Ok(response);
        }

        [HttpPost("flows")]
        public async Task<IActionResult> GetFlows([FromBody] string name)
        {
            Console.WriteLine($"c:{name}") ;
            var response = await _flowClient.GetFlowsAsync(name);
            return Ok(response);
        }
    }
}
