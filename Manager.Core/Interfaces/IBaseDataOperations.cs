using Manager.Core.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
    public interface IBaseDataOperations<T> 
        where T : class, IBaseEntities, new()
    {
        List<T> GetAll();
        T GetByID(Int64 Id);
        Int64 Add(T entity);
        bool Update(T entity);
        bool DeleteByID(Int64 Id);
    }
}
