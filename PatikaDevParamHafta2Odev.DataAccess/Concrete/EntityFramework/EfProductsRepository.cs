using PatikaDevParamHafta2Odev.DataAccess.Abstract;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Context;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Repository;
using PatikaDevParamHafta2Odev.Entity.Concrete.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatikaDevParamHafta2Odev.DataAccess.Concrete.EntityFramework
{
    public class EfProductsRepository : EfGenericRepository<Product>, IProductsDAL
    {
        public EfProductsRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
