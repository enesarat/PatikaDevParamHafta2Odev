using Microsoft.EntityFrameworkCore;
using PatikaDevParamHafta2Odev.DataAccess.Abstract;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Context;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Exceptions;
using PatikaDevParamHafta2Odev.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatikaDevParamHafta2Odev.DataAccess.Concrete.Repository
{
    public class EfGenericRepository<T> : IGenericEntityDAL<T> where T : class, IEntity, new()
    {
        private readonly AppDbContext _appDbContext;

        public EfGenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task DeleteItem(int id)
        {

            var deleteItem = await GetItemById(id);
            _appDbContext.Set<T>().Remove(deleteItem);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllItems()
        {
            var itemList = await _appDbContext.Set<T>().ToListAsync();
            return itemList;
        }

        public async Task<T> GetItemById(int id)
        {
            var item = await _appDbContext.Set<T>().FindAsync(id);
            if(item == null)
            {
                throw new NotFoundException($"Any entity from the database with ID: {id} could not be found.");
            }
            return item;
        }

        public async Task<T> InsertItem(T item)
        {
            await _appDbContext.Set<T>().AddAsync(item);
            await _appDbContext.SaveChangesAsync();
            return item;
        }

        public async Task<T> UpdateItem(T item)
        {
            _appDbContext.Set<T>().Update(item);
            await _appDbContext.SaveChangesAsync();
            return item;
        }
    }
}
