using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.DataAccessLayer.SqlClient.Helpers
{
    public static class QueryTypeFormats
    {
        public static string SelectFormat => $"SELECT {FormatTypes.Fields} FROM {FormatTypes.TableName} ";
        public static string UpdateFormat => $"UPDATE {FormatTypes.TableName} SET {FormatTypes.Fields} ";
        public static string DeleteFormat => $"DELETE FROM {FormatTypes.TableName} ";
    }
}
