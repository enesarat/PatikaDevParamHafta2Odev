using PatikaDevParamHafta2Odev.Business.Abstract;
using PatikaDevParamHafta2Odev.DataAccess.Abstract;
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

        public async Task DeleteItem(int id)
        {
            await productsAccess.DeleteItem(id);
        }

        public async Task<List<Product>> GetAllElement()
        {
            var entityList = await productsAccess.GetAllItems();
            return entityList;
        }

        public async Task<Product> GetElementById(int id)
        {
            var entity = await productsAccess.GetItemById(id);
            return entity;
        }

        public async Task<Product> InsertElement(Product item)
        {
            await productsAccess.InsertItem(item);
            return item;
        }

        public async Task<Product> UpdateElement(Product item)
        {
            await productsAccess.UpdateItem(item);
            return item;
        }
    }
}
