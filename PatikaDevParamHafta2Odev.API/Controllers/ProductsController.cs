using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PatikaDevParamHafta2Odev.Business.Abstract;
using PatikaDevParamHafta2Odev.Business.Concrete;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Context;
using PatikaDevParamHafta2Odev.Entity.Concrete.Models;

namespace PatikaDevParamHafta2Odev.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _manageProducts;

        public ProductsController(IProductsService manageProducts)
        {
            _manageProducts = manageProducts;
        }

        // This endpoint provides to get all data of product entity as a list.
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await _manageProducts.GetAllElements();
                if (products != null)
                {
                    return Ok(products); // 200 + data
                }
                return NotFound(); // 404
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        //-------------------------------------------------------------------- GET FROM BODY ----------------------------------------------------//
        // This endpoint provides to get the data of product which exist with given id information. (with binding over body)
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
                    return Ok(product); // 200 + data
                }
                return NotFound(); // 404
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        //-------------------------------------------------------------------- GET FROM QUERY ----------------------------------------------------//
        // This endpoint provides to get the data of product which exist according to given id information. (with binding over query string)
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
        // This endpoint provides to get the data of product which exist according to given properties as filtered list. (with binding over body)
        [HttpGet]
        [Route("{name}/{saleStatus}")]
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

                return Ok(products);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        //-------------------------------------------------------------------- FILTER FROM QUERY ----------------------------------------------------//
        // This endpoint provides to get the data of product which exist according to given properties as filtered list. (with binding over query string)
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
        // This endpoint provides to create a record of product according to given properties. (with binding over body)
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Product product)
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

        //-------------------------------------------------------------------- CREATE FROM QUERY ----------------------------------------------------//
        // This endpoint provides to create a record of product according to given properties. (with binding over query string)
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
        // This endpoint provides to update the record of product according to given id which exist with given product data. (with binding over body)
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] Product product)
        {
            try
            {
                product.Id = id;
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

        //-------------------------------------------------------------------- UPDATE FROM QUERY ----------------------------------------------------//
        // This endpoint provides to update the record of product according to given id which exist with given product data. (with binding over query string)
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
        // This endpoint provides to delete the record of product according to given id which exist with given product data. (with binding over body)
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (await _manageProducts.DeleteItem(id))
                {
                    return Ok(); // 200
                }
                return BadRequest(); // 400
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }

        //-------------------------------------------------------------------- DELETE FROM QUERY ----------------------------------------------------//
        // This endpoint provides to delete the record of product according to given id which exist with given product data. (with binding over query string)
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
                return BadRequest(); // 400
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }

        //-------------------------------------------------------------------- PATCH FROM BODY ----------------------------------------------------//
        // This endpoint provides to patch(to local update in this endpoint) the record of product according to given id which exist with given product data. (with binding over body)
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<Product> patchDocument)
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

        //-------------------------------------------------------------------- PATCH FROM QUERY ----------------------------------------------------//
        // This endpoint provides to patch(to local update in this endpoint) the record of product according to given id which exist with given product data. (with binding over query string)
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
