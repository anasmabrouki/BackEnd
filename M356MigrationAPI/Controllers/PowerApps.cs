using M356MigrationAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace M356MigrationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class PowerAppsController : Controller
    {
        private readonly AppsClient _appsClient;

        public PowerAppsController()
        {
            _appsClient = new AppsClient();
        }

        [HttpPost("powerApps")]
        public async Task<IActionResult> GetPowerApps([FromBody] string name)
        {
            var response = await _appsClient.GetPowerAppsAsync(name);
            return Ok(response);
        }

        [HttpGet("Migrate/{appId}")]
        public async Task<IActionResult> MigratePowerApps([FromHeader] string sourcJWT, [FromHeader] string targetJWT, string appId)
        {
            await _appsClient.ExportPowerAppsAsync(sourcJWT, appId);
            await _appsClient.ImportPowerAppAsync(targetJWT, appId);
            return Ok();
        }
    }
}
