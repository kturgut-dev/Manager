using Manager.Core.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
    public interface IEFDataOperations<T> : IBaseDataOperations<T>
        where T : class, IBaseEntities, new()
    {
        List<T> GetAll(Expression<Func<T, bool>> filter);
        T Get(Expression<Func<T, bool>> filter);
    }
}
