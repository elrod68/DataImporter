using Microsoft.AspNetCore.Mvc;

namespace DataImporter.Api.Controllers
{
  [Route("api/[controller]")]
  public class HealthCheckController : ControllerBase
  {
    #region Public Methods

    [HttpGet]
    public IActionResult Index()
    {
      return Ok(new { Status = "All Ok" });
    }

    #endregion Public Methods
  }
}
