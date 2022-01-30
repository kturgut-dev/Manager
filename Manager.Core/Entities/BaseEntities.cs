using Manager.Core.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Core.Entities
{
    public class BaseEntities : IBaseEntities
    {
        public long AutoID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
