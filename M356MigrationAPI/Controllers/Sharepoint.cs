namespace M356MigrationAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using M356MigrationAPI.Models;
    using Microsoft.SharePoint.Client;
    using System.Collections.Generic;

    [ApiController]
    [Route("api/[controller]")]
    public class SharepointController : ControllerBase
    {
        private readonly GraphClient _graphClient;

        public SharepointController()
        {
            _graphClient = new GraphClient();
        }

        [HttpGet("sites")]
        public async Task<IActionResult> GetSharepointSites([FromHeader] string jwt)
        {
            if (string.IsNullOrEmpty(jwt))
            {
                return BadRequest("JWT token is required.");
            }
            var result = await _graphClient.GetSharepointSites(jwt);
            return Ok(result);
        }

        [HttpGet("sites/{siteId}/lists")]
        public async Task<IActionResult> GetSharepointLists([FromHeader] string jwt, string siteId)
        {
            if (string.IsNullOrEmpty(jwt))
            {
                return BadRequest("JWT token is required.");
            }

            if (string.IsNullOrEmpty(siteId))
            {
                return BadRequest("Site ID is required.");
            }

            var result = await _graphClient.GetSharepointLists(jwt, siteId);
            return Ok(result);
        }

        [HttpPost("migrate")]
        public async Task<IActionResult> MigrateSharepointLists([FromHeader] string sourceJwt, [FromHeader] string targetJwt, MigrationPayload migrationPayload)
        {
            if (string.IsNullOrEmpty(sourceJwt) || string.IsNullOrEmpty(targetJwt))
            {
                return BadRequest("Both source and target JWT tokens are required.");
            }

            string sourceSiteId = migrationPayload.sourceSiteId;
            string targetSiteId = migrationPayload.targetSiteId;
            List<ListGuid> listGuids = migrationPayload.listGuids;

            if (string.IsNullOrEmpty(sourceSiteId) || string.IsNullOrEmpty(targetSiteId) || listGuids == null || listGuids.Count == 0)
            {
                return BadRequest("Source site ID, target site ID, and list of list GUIDs are required.");
            }

            try
            {
                await _graphClient.MigrateListAsync(sourceJwt, targetJwt, sourceSiteId, targetSiteId, listGuids);

                return Ok("Migration completed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during migration: {ex.Message}");
            }
        }
    }
}
