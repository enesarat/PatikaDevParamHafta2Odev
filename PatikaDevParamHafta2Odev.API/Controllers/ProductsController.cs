using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PatikaDevParamHafta2Odev.Business.Abstract;
using PatikaDevParamHafta2Odev.Business.Concrete;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Context;
using PatikaDevParamHafta2Odev.Entity.Concrete.Models;
using System.Reflection;

namespace PatikaDevParamHafta2Odev.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _manageProducts;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductsService manageProducts, ILogger<ProductsController> logger)
        {
            _manageProducts = manageProducts;
            _logger = logger;
        }


        /// <summary>
        /// This endpoint provides to get all data of product entity as a list.
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet,Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await _manageProducts.GetAllElements();
                if (products != null)
                {
                    _logger.LogInformation($"{products.Count} products were successfully returned with the response.");
                    return Ok(products); // 200 + data
                }
                _logger.LogInformation("No products found!");
                return NotFound(); // 404
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest(exp.Message);
            }
        }

        //-------------------------------------------------------------------- GET FROM BODY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to get the data of product which exist with given id information. (with binding over body)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [ActionName("GetAsync")]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var product = await _manageProducts.GetElementById(id);
                if (product != null)
                {
                    _logger.LogInformation($"Product which has id:{product.Id} were successfully returned with the response.");
                    return Ok(product); // 200 + data
                }
                _logger.LogInformation("No product found!");
                return NotFound(); // 404
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest(exp.Message);
            }
        }

        //-------------------------------------------------------------------- GET FROM QUERY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to get the data of product which exist according to given id information. (with binding over query string)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetByIdFromQuery")]
        public async Task<IActionResult> GetAsync([FromQuery] int id)
        {
            try
            {
                var product = await _manageProducts.GetElementById(id);
                if (product != null)
                {
                    return Ok(product); // 200 + data
                }
                return NotFound(); // 404
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        //-------------------------------------------------------------------- FILTER FROM BODY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to get the data of product which exist according to given properties as filtered list. (with binding over body)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryName"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="saleStatus"></param>
        /// <returns></returns>
        
        [HttpGet]
        [Route("{name}/{saleStatus}")]
        [ActionName("GetByFilter")]
        public async Task<IActionResult> GetAsync(string name, string categoryName, int price, int quantity, bool saleStatus)
        {
            try
            {
                List<Product> products = await _manageProducts.GetAllElements();
                if (name != null)
                {
                    products = products.Where(x => x.Name.ToUpper().Contains(name.ToUpper())).ToList();
                }
                if (categoryName != null)
                {
                    products = products.Where(x => x.CategoryName.ToUpper().Contains(categoryName.ToUpper())).ToList();
                }
                if (price != 0)
                {
                    products = products.Where(x => x.Price == price).ToList();
                }
                if (quantity != 0)
                {
                    products = products.Where(x => x.Quantity == quantity).ToList();
                }
                products = products.Where(x => x.SaleStatus == saleStatus).ToList();
                
                _logger.LogInformation($"{products.Count} products were successfully returned with the response.");
                return Ok(products);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest(exp.Message);
            }
        }

        //-------------------------------------------------------------------- FILTER FROM QUERY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to get the data of product which exist according to given properties as filtered list. (with binding over query string)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryName"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="saleStatus"></param>
        /// <returns></returns>
        
        [HttpGet]
        [Route("GetByFilterFromQuery")]
        public async Task<IActionResult> GetByFilterAsync([FromQuery] string name, [FromQuery] string categoryName, [FromQuery] int price, [FromQuery] int quantity, [FromQuery] bool saleStatus)
        {
            try
            {
                List<Product> products = await _manageProducts.GetAllElements();
                if (name != null)
                {
                    products = products.Where(x => x.Name.ToUpper().Contains(name.ToUpper())).ToList();
                }
                if (categoryName != null)
                {
                    products = products.Where(x => x.CategoryName.ToUpper().Contains(categoryName.ToUpper())).ToList();
                }
                if (price != 0)
                {
                    products = products.Where(x => x.Price == price).ToList();
                }
                if (quantity != 0)
                {
                    products = products.Where(x => x.Quantity == quantity).ToList();
                }
                products = products.Where(x => x.SaleStatus == saleStatus).ToList();

                return Ok(products);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        //-------------------------------------------------------------------- CREATE FROM BODY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to create a record of product according to given properties. (with binding over body)
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _manageProducts.InsertElement(product);
                    _logger.LogInformation($"Product was created with id:{product.Id}");
                    return CreatedAtAction(nameof(GetAsync), new { id = product.Id }, product); // 201 + data + header info for data location
                }
                _logger.LogError($"Create product fail:{ModelState}");
                return BadRequest(ModelState); // 400
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest(exp.Message);
            }

        }

        //-------------------------------------------------------------------- CREATE FROM QUERY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to create a record of product according to given properties. (with binding over query string)
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        
        [HttpPost]
        [Route("CreateFromQuery")]

        public async Task<IActionResult> CreateFromQueryAsync([FromQuery] Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _manageProducts.InsertElement(product);
                    return CreatedAtAction(nameof(GetAsync), new { id = product.Id }, product); // 201 + data + header info for data location
                }
                return BadRequest(ModelState); // 400
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }

        //-------------------------------------------------------------------- UPDATE FROM BODY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to update the record of product according to given id which exist with given product data. (with binding over body)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] Product product)
        {
            try
            {
                product.Id = id;
                if (await _manageProducts.UpdateElement(product) != null)
                {
                    _logger.LogInformation("Product updated succesfully.");
                    return Ok(product); // 200 + data
                }
                _logger.LogInformation("No products found to update!");
                return NotFound(); // 404 
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest(exp.Message);
            }

        }

        //-------------------------------------------------------------------- UPDATE FROM QUERY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to update the record of product according to given id which exist with given product data. (with binding over query string)
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        
        [HttpPut]
        [Route("UpdateFromQuery")]
        public async Task<IActionResult> UpdateFromQueryAsync([FromQuery] Product product)
        {
            try
            {
                product.Id = product.Id;
                if (await _manageProducts.UpdateElement(product) != null)
                {
                    return Ok(product); // 200 + data
                }
                return NotFound(); // 404 
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }

        //-------------------------------------------------------------------- DELETE FROM BODY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to delete the record of product according to given id which exist with given product data. (with binding over body)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (await _manageProducts.DeleteItem(id))
                {
                    _logger.LogInformation("Product deleted succesfully.");
                    return Ok(); // 200
                }
                _logger.LogInformation("No products found to delete!");
                return NotFound(); // 404
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest(exp.Message);
            }

        }

        //-------------------------------------------------------------------- DELETE FROM QUERY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to delete the record of product according to given id which exist with given product data. (with binding over query string)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [HttpDelete]
        [Route("DeleteFromQuery")]
        public async Task<IActionResult> DeleteFromQueryAsync([FromQuery] int id)
        {
            try
            {
                if (await _manageProducts.DeleteItem(id))
                {
                    return Ok(); // 200
                }
                return NotFound(); // 404
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }

        //-------------------------------------------------------------------- PATCH FROM BODY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to patch(to local update in this endpoint) the record of product according to given id which exist with given product data. (with binding over body)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<Product> patchDocument)
        {
            try
            {
                if (patchDocument == null)
                {
                    _logger.LogInformation("patchDocument is null!");
                    return BadRequest(ModelState);
                }

                var product = await _manageProducts.GetElementById(id);
                if (await _manageProducts.GetElementById(id) == null)
                {
                    _logger.LogInformation("No products found to patch!");
                    return NotFound();
                }

                patchDocument.ApplyTo(product, ModelState);

                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Product patched succesfully.");
                    await _manageProducts.UpdateElement(product);
                    return Ok(product);
                }
                _logger.LogInformation("ModelState is not valid!");
                return BadRequest(ModelState);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest(exp.Message);
            }
        }

        //-------------------------------------------------------------------- PATCH FROM QUERY ----------------------------------------------------//
        /// <summary>
        /// This endpoint provides to patch(to local update in this endpoint) the record of product according to given id which exist with given product data. (with binding over query string)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        
        [HttpPatch]
        [Route("PatchFromQuery")]
        public async Task<IActionResult> PatchFromQueryAsync([FromQuery] int id, [FromQuery] JsonPatchDocument<Product> patchDocument)
        {
            try
            {
                if (patchDocument == null)
                {
                    return BadRequest(ModelState);
                }

                var product = await _manageProducts.GetElementById(id);
                if (await _manageProducts.GetElementById(id) == null)
                {
                    return NotFound();
                }

                patchDocument.ApplyTo(product, ModelState);

                if (ModelState.IsValid)
                {
                    await _manageProducts.UpdateElement(product);
                    return Ok(product);
                }

                return BadRequest(ModelState);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }

    }
}
