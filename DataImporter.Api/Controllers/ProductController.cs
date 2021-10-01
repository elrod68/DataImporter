using DataImporter.Core;
using DataImporter.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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
            _logger.LogDebug(1, "ProductController initialized");
        }

        [HttpGet("{CompanyID},{FeedID}")]
        public IActionResult Get(int CompanyID, int FeedID)
        {
            try
            {
                _logger.LogInformation($"Get Products for CompanyID {CompanyID} and FeedID {FeedID}");

                var products = _productService.GetProducts(CompanyID, FeedID);
                if (products == null) return NotFound();
                if (products.Count==0) return NotFound();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

    }
}
