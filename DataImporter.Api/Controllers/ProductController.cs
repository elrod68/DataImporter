using DataImporter.Core;
using DataImporter.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataImporter.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductController : ControllerBase
  {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
    {
      _logger = logger;
      _productService = productService;
    }

    [HttpGet("{CompanyID},{FeedID}")]
    public IActionResult Get(int CompanyID, int FeedID)
    {
      return Ok();
    }

  }
}
