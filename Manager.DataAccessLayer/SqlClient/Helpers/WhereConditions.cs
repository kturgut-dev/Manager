using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.DataAccessLayer.SqlClient.Helpers
{
    public class WhereConditions
    {
        public string PropertyName { get; set; }
        public ConditionTypes QueryCond { get; set; }
        public string Condition { get; set; }
    }
}
