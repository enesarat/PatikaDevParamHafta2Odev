using PatikaDevParamHafta2Odev.DataAccess.Concrete.Extensions;
using PatikaDevParamHafta2Odev.Entity.Concrete.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatikaDevParamHafta2Odev.DataAccess.Concrete.Extensions
{
    public static class ProductsExtensions
    {
        public static bool IsProductAvailable(this Product product) // Custom extension method to check sale status of product.
        {
            if (product.SaleStatus)
            {
                return true;
            }
            return false;
        }

        public static List<Product> GetAllAvailable(this List<Product> products) // Custom extension method to get all product entities which available.
        {
            List<Product> availableList = new List<Product>();
            foreach (var product in products)
            {
                if (product.IsProductAvailable())
                {
                    availableList.Add(product);
                }
            }
            return availableList;
        }
    }
}
