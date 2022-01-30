using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.DataAccessLayer.Helpers
{

    public static class DataRecordExtensions
    {
        public static bool HasColumn(this System.Data.IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
