using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces.Entities
{
    public interface IBaseEntities
    {
        Int64 AutoID { get; set; }
        bool IsActive { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? DeletedDate { get; set; }
    }
}
