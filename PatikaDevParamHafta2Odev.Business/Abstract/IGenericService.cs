using PatikaDevParamHafta2Odev.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PatikaDevParamHafta2Odev.Business.Abstract
{
    public interface IGenericService<T> where T : class, IEntity, new()
    {
        Task<List<T>> GetAllElement();
        Task<T> GetElementById(int id);
        Task<T> InsertElement(T item);
        Task<T> UpdateElement(T item);
        Task DeleteItem(int id);
    }
}
