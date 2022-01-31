using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.DataAccessLayer.SqlClient.Helpers
{
    public static class QueryTypeFormats
    {
        public static string SelectFormat => "SELECT * FROM {1} ";
        public static string UpdateFormat => "UPDATE {0} SET {1} ";
        public static string DeleteFormat => "DELETE {0} ";
    }
}
