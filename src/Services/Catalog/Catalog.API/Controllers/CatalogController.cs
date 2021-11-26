using Catalog.API.Entities;
using Catalog.API.Data;
using Catalog.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        #region Members
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            this._productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Actions

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(CancellationToken cancellationToken)
        {
            var products = await this._productRepository.GetProductsAsync(cancellationToken);
            return Ok(products);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetById(string id, CancellationToken cancellationToken)
        {
            var product = await this._productRepository.GetProductAsync(id, cancellationToken);
            if (product == default)
            {
                this._logger.LogError($"Product with id of {id}, not found.");
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("[action]/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetByCategoryName(string category, CancellationToken cancellationToken)
        {
            var products = await this._productRepository.GetProductsByCategoryAsync(category, cancellationToken);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Product>> Create(Product product, CancellationToken cancellationToken)
        {
            await this._productRepository.InsertProductAsync(product, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { product.Id }, product);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> Update(Product product, CancellationToken cancellationToken)
        {
            return Ok(await this._productRepository.UpdateProductAsync(product, cancellationToken));
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            return Ok(await this._productRepository.DeleteProductAsync(id, cancellationToken));
        }

        #endregion
    }
}
