using PatikaDevParamHafta2Odev.Business.Abstract;
using PatikaDevParamHafta2Odev.DataAccess.Abstract;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Extensions;
using PatikaDevParamHafta2Odev.Entity.Concrete.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatikaDevParamHafta2Odev.Business.Concrete
{
    public class ProductsManager : IProductsService
    {
        IProductsDAL productsAccess;

        public ProductsManager(IProductsDAL productsAccess)
        {
            this.productsAccess = productsAccess;
        }

        public async Task<bool> DeleteItem(int id)
        {
            var product = await productsAccess.GetItemById(id);
            if (product != null)
            {
                if (product.IsProductAvailable()) // Custom extension method to check sale status of product.
                {
                    product.SaleStatus = false;               // Soft Delete
                    await productsAccess.UpdateItem(product);//  Soft Delete
                    //await productsAccess.DeleteItem(id);  //   Hard Delete
                    return true;
                }
            }
            return false;
        }

        public async Task<List<Product>> GetAllElements()
        {
            var entityList = await productsAccess.GetAllItems();
            entityList = entityList.GetAllAvailable();
            return entityList;
        }

        public async Task<Product> GetElementById(int id)
        {
            var entity = await productsAccess.GetItemById(id);
            if (entity != null)
            {
                if (entity.IsProductAvailable()) // Custom extension method to get all product entities which available.
                {
                    return entity;
                }
            }
            return null;
        }

        public async Task<Product> InsertElement(Product item)
        {
            await productsAccess.InsertItem(item);
            return item;
        }

        public async Task<Product> UpdateElement(Product item)
        {
            var product = await productsAccess.GetItemById(item.Id);
            if (product != null)
            {
                await productsAccess.UpdateItem(item);
                return item;
            }
            return null;
        }
    }
}
