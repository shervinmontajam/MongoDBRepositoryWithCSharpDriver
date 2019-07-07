using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDbRepositoryWithCSharpDriver.Data.Entities;
using MongoDbRepositoryWithCSharpDriver.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDbRepositoryWithCSharpDriver.Models.ApiRequests;

namespace MongoDbRepositoryWithCSharpDriver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        [HttpGet]
        [Route(nameof(GetAllProducts))]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productRepository.GetAllAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route(nameof(GetSingleProduct))]
        public async Task<IActionResult> GetSingleProduct([FromQuery] Guid guidId)
        {
            var result = await _productRepository.GetSingleAsync(filter => filter.GuidId == guidId);

            return Ok(result);
        }


        [HttpGet]
        [Route(nameof(GetProductByObjectId))]
        public async Task<IActionResult> GetProductByObjectId([FromQuery] string id)
        {
            var product = await GetProduct(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet]
        [Route(nameof(GetProductBySearch))]
        public async Task<IActionResult> GetProductBySearch([FromQuery] string name)
        {
            var result = await _productRepository.GetAsync(filter => filter.Name == name || string.IsNullOrEmpty(name));
            return Ok(result);
        }



        [HttpPost]
        [Route(nameof(AddProduct))]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductRequestModel createProductRequestModel)
        {
            var product = new ProductEntity
            {
                Id = ObjectId.GenerateNewId(),
                GuidId = Guid.NewGuid(),
                Name = createProductRequestModel.Name,
                LastUpdate = DateTime.Now,
                Price = createProductRequestModel.Price,
                Image = createProductRequestModel.Image
            };

            await _productRepository.AddAsync(product);

            return Ok(product.Id);
        }

        [HttpPost]
        [Route(nameof(AddRangeProduct))]
        public async Task<IActionResult> AddRangeProduct([FromBody] CreateProductRequestModel[] createProductRequestModels)
        {
            var list = createProductRequestModels.Select(item => new ProductEntity
            {
                Id = ObjectId.GenerateNewId(),
                GuidId = Guid.NewGuid(),
                Name = item.Name,
                LastUpdate = DateTime.Now,
                Price = item.Price,
                Image = item.Image
            }).ToList();

            await _productRepository.AddRangeAsync(list);

            return Ok(list.Select(a => a.Id).ToList());
        }


        [HttpPut]
        [Route(nameof(UpdateProduct))]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequestModel updateProductRequestModel)
        {
            var product = await GetProduct(updateProductRequestModel.Id);
            if (product == null)
                return NotFound();

            product.Name = updateProductRequestModel.Name;
            product.Image = updateProductRequestModel.Image;
            product.Price = updateProductRequestModel.Price;
            product.LastUpdate = DateTime.Now;

            var result = await _productRepository.UpdateAsync(filter => filter.Id == product.Id, product);

            return result ? NoContent() : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await GetProduct(id);
            if (product == null)
                return NotFound();

            var result = await _productRepository.Remove(filter => filter.Id == product.Id);

            return result ? NoContent() : StatusCode(StatusCodes.Status500InternalServerError);
        }



        private async Task<ProductEntity> GetProduct(string id)
        {
            ObjectId.TryParse(id, out var objectId);
           return await _productRepository.GetSingleAsync(filter => filter.Id == objectId);
        }

    }
}
